# FHIRagent

**FHIRagent** is a comprehensive FHIR integration solution designed to facilitate seamless data exchange between healthcare systems. It consists of two primary components:

1. **Desktop Agent:** A WPF desktop application that integrates in real time with Athenahealth's EHR via SMART on FHIR. It harvests and synchronizes patient data from the EHR and pushes aggregated context to a secondary FHIR server.
2. **Admin Panel:** A web-based (Blazor WebAssembly + Firebase) administration interface for monitoring, configuration, logging, and analytics.

## System Architecture

```plaintext
            +---------------------+         +--------------------------+
            |     Agent.UI        |<------->|      Agent.Services      |
            | (WPF Desktop App)   |         | PatientContextListener   |
            +----------+----------+         +-------------+------------+
                       |                                |
                       v                                v
             +---------+---------+         +----------------------+
             |   Primary EHR     |         | Secondary FHIR Server|
             |  (Athenahealth)   |         | (e.g. HAPI FHIR Test)|
             +-------------------+         +----------------------+
                       ^                                ^
                       |                                |
                    +--+--------------------------------+--+
                    |            Admin Panel               |
                    |    (Blazor WebAssembly + Firebase)  |
                    +-------------------------------------+
```

## Components

### Desktop Agent
- **Monitors EHR Activity:** Listens for patient context changes
- **Data Harvesting:** Uses FHIR R4 queries to retrieve patient data
- **Data Synchronization:** Pushes aggregated data to secondary FHIR server
- **Real-Time Overlay:** Launches embedded web overlay displaying patient context
- **EHR Write Operations:** Writes updates back to the EHR via FHIR

### Admin Panel
- **Monitoring:** Real-time tracking of agent status and data flow
- **Configuration:** Manage endpoints, credentials, and polling intervals
- **Analytics:** System logs and deployment status
- **Web Access:** Available at https://fhiragent.web.app

## Project Structure

### Agent.Core/
FHIR integration and business logic:
- `FhirClientWrapper.cs` - Manages FHIR interactions
- `ContextAggregator.cs` - Aggregates patient context
- `DataPusher.cs` - Handles secondary server sync
- `EHRWriter.cs` - Manages EHR write operations

### Agent.UI/
WPF desktop application:
- `MainWindow.xaml` - Main UI with embedded web overlay

### Agent.Services/
Background services:
- `PatientContextListener.cs` - Context change monitoring

### AdminPanel/
Blazor WebAssembly admin interface:
- Real-time monitoring dashboard
- Configuration management
- System logging and analytics

### Tests/
Test projects:
- Unit and integration tests
- FHIR interaction verification

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Node.js and npm
- Firebase CLI

### Development Setup

1. Clone the repository:
```bash
git clone https://github.com/BigRedOak/FHIRagent.git
cd FHIRagent
```

2. Build the solution:
```bash
dotnet build
```

3. Run components:
```bash
# Run desktop agent
cd Agent.UI
dotnet run

# Run admin panel
cd ../AdminPanel
dotnet run
```

### Configuration
Update `appsettings.json` with:
- EHR endpoint URLs
- SMART on FHIR credentials
- Secondary FHIR server details
- Polling parameters

## Deployment

### Desktop Agent
```bash
dotnet publish Agent.UI -c Release
# Distribute output to workstations
```

### Admin Panel
```bash
dotnet publish AdminPanel -c Release
firebase deploy
```

## Security Features
- SMART on FHIR authentication
- Role-based access control
- Audit logging
- Encrypted data transmission
- HIPAA compliance measures

## Contributing
1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## Support
- [Documentation Wiki](https://github.com/BigRedOak/FHIRagent/wiki)
- [Issue Tracker](https://github.com/BigRedOak/FHIRagent/issues)
- [Discussions](https://github.com/BigRedOak/FHIRagent/discussions)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
