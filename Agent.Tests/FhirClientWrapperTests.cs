using Xunit;
using Agent.Core;

namespace Agent.Tests
{
    public class FhirClientWrapperTests
    {
        [Fact]
        public void TestInitialization()
        {
            // Arrange
            string ehrEndpoint = "https://ehr.example.com/fhir";
            string secondaryEndpoint = "https://hapi.fhir.org/baseR4";
            var clientWrapper = new FhirClientWrapper(ehrEndpoint, secondaryEndpoint);

            // Act
            clientWrapper.Initialize();

            // Assert
            Assert.NotNull(clientWrapper);
            // Further assertions can be added once the methods are fully implemented.
        }
    }
} 