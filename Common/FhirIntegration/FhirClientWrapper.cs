using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Serilog;

namespace Common.FhirIntegration
{
    public class FhirClientWrapper
    {
        private readonly string _ehrEndpoint;
        private readonly string _secondaryFhirServerEndpoint;
        private FhirClient? _ehrClient;
        private FhirClient? _secondaryClient;

        public FhirClientWrapper(string ehrEndpoint, string secondaryFhirServerEndpoint)
        {
            _ehrEndpoint = ehrEndpoint;
            _secondaryFhirServerEndpoint = secondaryFhirServerEndpoint;
        }

        // Initialize FHIR clients and verify connectivity
        public async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken)
        {
            _ehrClient = new FhirClient(_ehrEndpoint);

            _secondaryClient = new FhirClient(_secondaryFhirServerEndpoint);

            try
            {
                var capabilityStatement = await _secondaryClient.CapabilityStatementAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving capability statement from the secondary FHIR server.");
                throw;
            }
        }

        public async System.Threading.Tasks.Task<Patient> GetPatientContextAsync(string patientId, CancellationToken cancellationToken)
        {
            try
            {
                var patient = await _secondaryClient!.ReadAsync<Patient>($"Patient/{patientId}");
                Log.Information("Successfully retrieved patient with id {PatientId}", patientId);
                return patient ?? throw new Exception($"Patient {patientId} not found");
            }
            catch (FhirOperationException fex)
            {
                Log.Error(fex, "FHIR Operation error while getting patient {PatientId}", patientId);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting patient {PatientId}", patientId);
                throw;
            }
        }

        public async System.Threading.Tasks.Task<Bundle> SearchResourceAsync<T>(string patientId, CancellationToken cancellationToken) where T : Resource
        {
            try
            {
                var searchParams = new SearchParams().Where($"patient=Patient/{patientId}");
                var result = await _ehrClient!.SearchAsync<T>(searchParams);
                return result ?? new Bundle();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error searching for {ResourceType} for patient {PatientId}", typeof(T).Name, patientId);
                throw;
            }
        }

        public async System.Threading.Tasks.Task WritePatientDataAsync(Resource patientData, CancellationToken cancellationToken)
        {
            try
            {
                Resource? result;
                if (string.IsNullOrWhiteSpace(patientData.Id))
                {
                    result = await _secondaryClient!.CreateAsync(patientData);
                    Log.Information("Successfully created resource. New id: {Id}", result?.Id);
                }
                else
                {
                    result = await _secondaryClient!.UpdateAsync(patientData);
                    Log.Information("Successfully updated resource with id {Id}", result?.Id);
                }
            }
            catch (FhirOperationException fex)
            {
                Log.Error(fex, "FHIR Operation error while writing patient data.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "General error while writing patient data.");
                throw;
            }
        }

        public async System.Threading.Tasks.Task WriteDataToEhrAsync(Resource patientData, CancellationToken cancellationToken)
        {
            try
            {
                Resource? result;
                if (string.IsNullOrWhiteSpace(patientData.Id))
                {
                    result = await _ehrClient!.CreateAsync(patientData);
                    Log.Information("Successfully created resource on EHR. New id: {Id}", result?.Id);
                }
                else
                {
                    result = await _ehrClient!.UpdateAsync(patientData);
                    Log.Information("Successfully updated resource on EHR with id {Id}", result?.Id);
                }
            }
            catch (FhirOperationException fex)
            {
                Log.Error(fex, "FHIR Operation error while writing data to EHR");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "General error while writing data to EHR");
                throw;
            }
        }
    }
} 