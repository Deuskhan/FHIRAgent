using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Agent.UI
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up Serilog logger
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();

            // Configure dependency injection
            var services = new ServiceCollection();

            // Add logging to the container (using Serilog)
            services.AddLogging(configure => configure.AddSerilog());

            // Register application services (from Agent.Core and Agent.Services)
            services.AddSingleton<Agent.Core.FhirClientWrapper>(provider =>
                new Agent.Core.FhirClientWrapper("https://ehr.example.com/fhir", "https://hapi.fhir.org/baseR4"));
            services.AddSingleton<Agent.Core.ContextAggregator>();
            services.AddSingleton<Agent.Core.DataPusher>();
            services.AddSingleton<Agent.Core.EHRWriter>();
            services.AddSingleton<Agent.Services.PatientContextListener>();

            // Register the MainWindow
            services.AddSingleton<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();

            // Resolve and show the MainWindow via DI
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
} 