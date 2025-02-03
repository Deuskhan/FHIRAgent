using AdminPanel.Pages;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using AdminPanel.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AdminPanel.Tests.Components
{
    public class DashboardTests : TestContext
    {
        private readonly Mock<FirebaseService> _mockFirebaseService;
        private readonly Mock<ILogger<FirebaseService>> _mockLogger;
        private readonly List<Agent> _testAgents;

        public DashboardTests()
        {
            _mockLogger = new Mock<ILogger<FirebaseService>>();
            _mockFirebaseService = new Mock<FirebaseService>("test-project", _mockLogger.Object);
            Services.AddScoped<AuthenticationStateProvider, TestAuthenticationStateProvider>();
            Services.AddScoped<FirebaseService>(_ => _mockFirebaseService.Object);

            // Setup test data
            _testAgents = new List<Agent>
            {
                new Agent { AgentId = 1, MachineName = "Test1", PatientMatchCount = 10, ExtractedDataCount = 20 },
                new Agent { AgentId = 2, MachineName = "Test2", PatientMatchCount = 15, ExtractedDataCount = 25 }
            };
        }

        [Fact]
        public async Task Dashboard_ShouldRenderStatistics()
        {
            // Arrange
            _mockFirebaseService.Setup(x => x.GetAgentsAsync())
                .ReturnsAsync(_testAgents);
            _mockFirebaseService.Setup(x => x.StartRealtimeUpdatesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var cut = RenderComponent<Dashboard>();

            // Wait for any async operations to complete
            await Task.Delay(100);

            // Assert
            cut.Find("h1").TextContent.Should().Be("Agent Dashboard");
            cut.FindAll("tr").Count.Should().Be(3); // Header + 2 agents
            cut.Find("p:contains('Total Patient Matches')").TextContent.Should().Contain("25"); // 10 + 15
            cut.Find("p:contains('Total Extracted Data')").TextContent.Should().Contain("45"); // 20 + 25
        }

        // Test authentication provider
        private class TestAuthenticationStateProvider : AuthenticationStateProvider
        {
            public override Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "test@example.com")
                }, "test");
                var user = new ClaimsPrincipal(identity);
                return Task.FromResult(new AuthenticationState(user));
            }
        }
    }
} 