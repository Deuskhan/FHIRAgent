using System;
using System.Threading;
using System.Threading.Tasks;
using Common.FhirIntegration;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace AgentClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                // Load configuration
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                // Initialize FHIR client wrapper
                var ehrEndpoint = configuration["FHIR:EhrEndpoint"];
                var secondaryEndpoint = configuration["FHIR:SecondaryEndpoint"];
                
                var fhirWrapper = new FhirClientWrapper(ehrEndpoint, secondaryEndpoint);
                await fhirWrapper.InitializeAsync(CancellationToken.None);

                // Initialize context aggregator
                var contextAggregator = new ContextAggregator(fhirWrapper);

                Log.Information("FHIR Agent initialized successfully");

                // TODO: Implement main agent loop
                // This could involve:
                // 1. Monitoring for new patient context
                // 2. Aggregating data using contextAggregator
                // 3. Pushing data to secondary FHIR server
                // 4. Reporting status back to admin panel

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while running the FHIR agent");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
} 