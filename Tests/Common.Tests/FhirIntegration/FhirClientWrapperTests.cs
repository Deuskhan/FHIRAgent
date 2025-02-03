using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncTask = System.Threading.Tasks.Task;
using Common.FhirIntegration;
using FluentAssertions;
using Hl7.Fhir.Model;
using Xunit;

namespace Common.Tests.FhirIntegration
{
    public class FhirClientWrapperTests
    {
        private readonly string _testEhrEndpoint = "http://hapi.fhir.org/baseR4";
        private readonly string _testSecondaryEndpoint = "http://hapi.fhir.org/baseR4";

        [Fact]
        [Trait("Category", "FhirClient")]
        public async AsyncTask InitializeAsync_WithValidEndpoints_ShouldSucceed()
        {
            // Arrange
            var wrapper = new FhirClientWrapper(_testEhrEndpoint, _testSecondaryEndpoint);

            // Act
            await wrapper.InitializeAsync(CancellationToken.None);

            // Assert
            // If no exception is thrown, the test passes
        }

        [Fact]
        [Trait("Category", "FhirClient")]
        public async AsyncTask GetPatientContextAsync_WithValidId_ShouldReturnPatient()
        {
            // Arrange
            var wrapper = new FhirClientWrapper(_testEhrEndpoint, _testSecondaryEndpoint);
            await wrapper.InitializeAsync(CancellationToken.None);
            var testPatientId = "example"; // Use a known test patient ID

            // Act
            var patient = await wrapper.GetPatientContextAsync(testPatientId, CancellationToken.None);

            // Assert
            patient.Should().NotBeNull();
            patient.Id.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [Trait("Category", "FhirClient")]
        public async AsyncTask SearchResourceAsync_ForConditions_ShouldReturnBundle()
        {
            // Arrange
            var wrapper = new FhirClientWrapper(_testEhrEndpoint, _testSecondaryEndpoint);
            await wrapper.InitializeAsync(CancellationToken.None);
            var testPatientId = "example";

            // Act
            var bundle = await wrapper.SearchResourceAsync<Condition>(testPatientId, CancellationToken.None);

            // Assert
            bundle.Should().NotBeNull();
            bundle.Type.Should().Be(Bundle.BundleType.Searchset);
        }
    }
} 