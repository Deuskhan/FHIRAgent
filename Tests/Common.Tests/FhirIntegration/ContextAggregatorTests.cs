using System.Threading;
using System.Threading.Tasks;
using AsyncTask = System.Threading.Tasks.Task;
using Common.FhirIntegration;
using FluentAssertions;
using Hl7.Fhir.Model;
using Moq;
using Xunit;

namespace Common.Tests.FhirIntegration
{
    public class ContextAggregatorTests
    {
        private readonly Mock<FhirClientWrapper> _mockFhirClient;
        private readonly ContextAggregator _aggregator;

        public ContextAggregatorTests()
        {
            _mockFhirClient = new Mock<FhirClientWrapper>("http://test.fhir.org", "http://test.fhir.org");
            _aggregator = new ContextAggregator(_mockFhirClient.Object);
        }

        [Fact]
        [Trait("Category", "ContextAggregator")]
        public async AsyncTask AggregatePatientContextAsync_ShouldReturnCompleteContext()
        {
            // Arrange
            var testPatientId = "test123";
            var testPatient = new Patient { Id = testPatientId };
            
            _mockFhirClient.Setup(x => x.GetPatientContextAsync(testPatientId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(testPatient);

            var emptyBundle = new Bundle { Type = Bundle.BundleType.Searchset };
            _mockFhirClient.Setup(x => x.SearchResourceAsync<Resource>(testPatientId, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(emptyBundle);

            // Act
            var result = await _aggregator.AggregatePatientContextAsync(testPatientId, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Patient.Should().BeSameAs(testPatient);
            result.Conditions.Should().NotBeNull();
            result.Medications.Should().NotBeNull();
            result.Observations.Should().NotBeNull();
        }
    }
} 