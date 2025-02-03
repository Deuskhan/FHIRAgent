using System;

namespace AgentClient.Monitoring
{
    public interface IAgentStatusReporter
    {
        void ReportStatus(string status, DateTime timestamp);
    }
} 