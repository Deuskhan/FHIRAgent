using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Models;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace AdminPanel.Services
{
    public class FirebaseService : IDisposable
    {
        private readonly FirestoreDb _db;
        private FirestoreChangeListener? _agentsListener;
        private FirestoreChangeListener? _logsListener;
        private FirestoreChangeListener? _configsListener;
        private readonly ILogger<FirebaseService> _logger;
        private bool _isReconnecting;
        private readonly SemaphoreSlim _reconnectSemaphore = new(1, 1);
        private CancellationTokenSource? _reconnectCts;

        public event Action<List<Agent>>? OnAgentsChanged;
        public event Action<List<LogEntry>>? OnLogsChanged;
        public event Action<bool>? OnConnectionStateChanged;
        public event Action<AgentConfiguration?>? OnConfigurationChanged;

        public FirebaseService(string projectId, ILogger<FirebaseService> logger)
        {
            _db = FirestoreDb.Create(projectId);
            _logger = logger;
        }

        public async Task StartRealtimeUpdatesAsync()
        {
            try
            {
                await Task.Run(() => SetupListenersAsync());
                OnConnectionStateChanged?.Invoke(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting real-time updates");
                await InitiateReconnectionAsync();
            }
        }

        private void SetupListenersAsync()
        {
            // Listen to agents collection
            var agentsRef = _db.Collection("agents");
            var agentsListener = agentsRef.Listen(async snapshot =>
            {
                try
                {
                    var agents = new List<Agent>();
                    foreach (var doc in snapshot.Documents)
                    {
                        var agent = doc.ConvertTo<Agent>();
                        agent.AgentId = int.Parse(doc.Id);
                        agents.Add(agent);
                    }
                    OnAgentsChanged?.Invoke(agents);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing agents snapshot");
                    await InitiateReconnectionAsync();
                }
            });
            _agentsListener = agentsListener;

            // Listen to logs collection
            var logsRef = _db.Collection("logs").OrderByDescending("Timestamp").Limit(100);
            var logsListener = logsRef.Listen(async snapshot =>
            {
                try
                {
                    var logs = snapshot.Documents
                        .Select(d => d.ConvertTo<LogEntry>())
                        .ToList();
                    OnLogsChanged?.Invoke(logs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing logs snapshot");
                    await InitiateReconnectionAsync();
                }
            });
            _logsListener = logsListener;
        }

        private async Task InitiateReconnectionAsync()
        {
            if (_isReconnecting) return;

            await _reconnectSemaphore.WaitAsync();
            try
            {
                if (_isReconnecting) return;
                _isReconnecting = true;
                OnConnectionStateChanged?.Invoke(false);

                _reconnectCts?.Cancel();
                _reconnectCts = new CancellationTokenSource();
                var token = _reconnectCts.Token;

                // Start reconnection loop
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        StopRealtimeUpdates();
                        SetupListenersAsync();
                        _isReconnecting = false;
                        OnConnectionStateChanged?.Invoke(true);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Reconnection attempt failed");
                        await Task.Delay(5000, token); // Wait 5 seconds before retrying
                    }
                }
            }
            finally
            {
                _reconnectSemaphore.Release();
            }
        }

        public void StopRealtimeUpdates()
        {
            _agentsListener?.StopAsync();
            _logsListener?.StopAsync();
            _configsListener?.StopAsync();
            _agentsListener = null;
            _logsListener = null;
            _configsListener = null;
        }

        public void Dispose()
        {
            _reconnectCts?.Cancel();
            _reconnectCts?.Dispose();
            StopRealtimeUpdates();
            _reconnectSemaphore.Dispose();
        }

        public async Task<List<Agent>> GetAgentsAsync()
        {
            var snapshot = await _db.Collection("agents").GetSnapshotAsync();
            var agents = new List<Agent>();

            foreach (var doc in snapshot.Documents)
            {
                var agent = doc.ConvertTo<Agent>();
                agent.AgentId = int.Parse(doc.Id);
                agents.Add(agent);
            }

            return agents;
        }

        public async Task UpdateAgentAsync(Agent agent)
        {
            await _db.Collection("agents").Document(agent.AgentId.ToString())
                .SetAsync(agent);
        }

        public async Task<List<LogEntry>> GetLogsAsync()
        {
            var snapshot = await _db.Collection("logs").GetSnapshotAsync();
            return snapshot.Documents.Select(d => d.ConvertTo<LogEntry>()).ToList();
        }

        public async Task<AgentConfiguration?> GetAgentConfigurationAsync(string agentId)
        {
            var docRef = _db.Collection("configurations").Document(agentId);
            var snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<AgentConfiguration>() : null;
        }

        public async Task UpdateAgentConfigurationAsync(AgentConfiguration config)
        {
            await _db.Collection("configurations").Document(config.AgentId)
                .SetAsync(config);
        }

        public async Task StartConfigurationListenerAsync(string agentId)
        {
            var docRef = _db.Collection("configurations").Document(agentId);
            var configsListener = await Task.Run(() => docRef.Listen(snapshot =>
            {
                var config = snapshot.Exists ? 
                    snapshot.ConvertTo<AgentConfiguration>() : null;
                OnConfigurationChanged?.Invoke(config);
            }));
            _configsListener = configsListener;
        }
    }
} 