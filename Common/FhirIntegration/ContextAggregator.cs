using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace Common.FhirIntegration
{
    public class AggregatedPatientContext
    {
        public Patient Patient { get; set; } = null!;
        public List<AllergyIntolerance> Allergies { get; set; } = new();
        public List<Condition> Conditions { get; set; } = new();
        public List<MedicationStatement> Medications { get; set; } = new();
        public List<Observation> Observations { get; set; } = new();
        public List<Immunization> Immunizations { get; set; } = new();
        public List<Procedure> Procedures { get; set; } = new();
        public List<CarePlan> CarePlans { get; set; } = new();
    }

    public class ContextAggregator
    {
        private readonly FhirClientWrapper _clientWrapper;

        public ContextAggregator(FhirClientWrapper clientWrapper)
        {
            _clientWrapper = clientWrapper;
        }

        public async Task<AggregatedPatientContext> AggregatePatientContextAsync(string patientId, CancellationToken cancellationToken)
        {
            var patient = await _clientWrapper.GetPatientContextAsync(patientId, cancellationToken);

            var allergyTask = _clientWrapper.SearchResourceAsync<AllergyIntolerance>(patientId, cancellationToken);
            var conditionTask = _clientWrapper.SearchResourceAsync<Condition>(patientId, cancellationToken);
            var medicationTask = _clientWrapper.SearchResourceAsync<MedicationStatement>(patientId, cancellationToken);
            var observationTask = _clientWrapper.SearchResourceAsync<Observation>(patientId, cancellationToken);
            var immunizationTask = _clientWrapper.SearchResourceAsync<Immunization>(patientId, cancellationToken);
            var procedureTask = _clientWrapper.SearchResourceAsync<Procedure>(patientId, cancellationToken);
            var carePlanTask = _clientWrapper.SearchResourceAsync<CarePlan>(patientId, cancellationToken);

            await Task.WhenAll(allergyTask, conditionTask, medicationTask, observationTask, 
                             immunizationTask, procedureTask, carePlanTask);

            return new AggregatedPatientContext
            {
                Patient = patient,
                Allergies = ExtractResources<AllergyIntolerance>(allergyTask.Result),
                Conditions = ExtractResources<Condition>(conditionTask.Result),
                Medications = ExtractResources<MedicationStatement>(medicationTask.Result),
                Observations = ExtractResources<Observation>(observationTask.Result),
                Immunizations = ExtractResources<Immunization>(immunizationTask.Result),
                Procedures = ExtractResources<Procedure>(procedureTask.Result),
                CarePlans = ExtractResources<CarePlan>(carePlanTask.Result)
            };
        }

        private static List<T> ExtractResources<T>(Bundle bundle) where T : Resource
        {
            var list = new List<T>();
            if (bundle?.Entry != null)
            {
                foreach (var entry in bundle.Entry)
                {
                    if (entry.Resource is T resource)
                    {
                        list.Add(resource);
                    }
                }
            }
            return list;
        }
    }
} 