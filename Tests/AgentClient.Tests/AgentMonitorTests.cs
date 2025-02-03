using System;
using System.Threading;
using System.Threading.Tasks;
using AgentClient.Monitoring;
using Common.FhirIntegration;
using Moq;
using Xunit;

namespace AgentClient.Tests
{
    public class AgentMonitorTests
    {
        private readonly Mock<FhirClientWrapper> _mockFhirClient;
        private readonly Mock<IAgentStatusReporter> _mockStatusReporter;

        public AgentMonitorTests()
        {
            _mockFhirClient = new Mock<FhirClientWrapper>("test", "test");
            _mockStatusReporter = new Mock<IAgentStatusReporter>();
        }

        [Fact]
        [Trait("Category", "AgentMonitoring")]
        public async Task StartMonitoring_ShouldReportAgentStatus()
        {
            // Arrange
            var monitor = new AgentMonitor(_mockFhirClient.Object, _mockStatusReporter.Object);
            var cancellationTokenSource = new CancellationTokenSource();

            // Act
            await monitor.StartAsync(cancellationTokenSource.Token);

            // Assert
            _mockStatusReporter.Verify(
                x => x.ReportStatus(
                    It.Is<string>(s => s == "Connected"),
                    It.IsAny<DateTime>()
                ),
                Times.Once
            );
        }

        [Fact]
        [Trait("Category", "AgentMonitoring")]
        public async Task OnPatientContextChanged_ShouldTriggerDataHarvesting()
        {
            // Arrange
            var monitor = new AgentMonitor(_mockFhirClient.Object, _mockStatusReporter.Object);
            var testPatientId = "test123";

            // Act
            await monitor.HandlePatientContextChange(testPatientId);

            // Assert
            _mockFhirClient.Verify(
                x => x.GetPatientContextAsync(
                    It.Is<string>(id => id == testPatientId),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
} 