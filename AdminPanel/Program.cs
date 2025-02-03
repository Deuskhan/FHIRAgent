using AdminPanel;
using AdminPanel.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AdminPanel.Services;
using System;
using System.Threading.Tasks;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Load configuration
var config = new AppSettings();
builder.Configuration.Bind(config);

if (string.IsNullOrEmpty(config.Firebase?.ProjectId))
{
    throw new Exception("Firebase ProjectId not configured");
}

builder.RootComponents.Add<App>("#app");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, FirebaseAuthenticationStateProvider>();
builder.Services.AddLogging(logging => 
{
    logging.SetMinimumLevel(LogLevel.Information);
    logging.AddFilter("Microsoft", LogLevel.Warning);
    logging.AddFilter("System", LogLevel.Warning);
});
builder.Services.AddScoped(sp => new FirebaseService(
    config.Firebase.ProjectId,
    sp.GetRequiredService<ILogger<FirebaseService>>()));

await builder.Build().RunAsync(); 