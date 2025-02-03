# FHIRagent

**FHIRagent** is a comprehensive FHIR integration solution designed to facilitate seamless data exchange between healthcare systems. It consists of two primary components:

1. **Desktop Agent:** A WPF desktop application that integrates in real time with Athenahealth's EHR via SMART on FHIR. It harvests and synchronizes patient data from the EHR and pushes aggregated context to a secondary FHIR server.
2. **Admin Panel:** A web-based (Blazor WebAssembly + Firebase) administration interface for monitoring, configuration, logging, and analytics.

---

## System Architecture

```plaintext
            +---------------------+         +--------------------------+
            |     Agent.UI        |<------->|      Agent.Services      |
            | (WPF Desktop App)   |   UI Overlay & PatientContextListener  |
            +----------+----------+         +-------------+------------+
                       |                                |
                       |                                v
             +---------+---------+         +----------------------+
             |   Primary EHR     |         | Secondary FHIR Server|
             |  (Athenahealth)   |         | (e.g. HAPI FHIR Test)|
             +-------------------+         +----------------------+
                                   ^
                                   |
                    +--------------+--------------+
                    |      Admin Panel          |
                    | (Blazor WebAssembly + Firebase) |
                    +-----------------------------+


Components
Desktop Agent
Monitors EHR Activity: Listens for patient context changes when a physician accesses a patient record.
Data Harvesting: Uses FHIR R4 queries to retrieve patient data (demographics, conditions, medications, observations, etc.) from the EHR.
Data Synchronization: Pushes aggregated patient data to a secondary FHIR server.
Real-Time Overlay: Launches an embedded web overlay displaying patient context on top of the EHR.
EHR Write Operations: Writes updates back to the EHR via FHIR.
Admin Panel
Monitoring: Real-time tracking of agent status and patient data flow.
Configuration Management: Adjust settings for endpoints, SMART on FHIR credentials, and polling intervals.
Logging & Analytics: Displays system logs, deployment status, and analytics.
Web Access: Accessible at https://fhiragent.web.app.
Project Structure
Agent.Core/
Contains FHIR integration and business logic.

FhirClientWrapper.cs – Manages FHIR interactions using the Firely .NET SDK.
ContextAggregator.cs – Aggregates patient context from the EHR.
DataPusher.cs – Synchronizes data with the secondary FHIR server.
EHRWriter.cs – Handles FHIR write operations to the EHR.
Agent.UI/
The WPF desktop application.

MainWindow.xaml – Main UI with an embedded WebBrowser control for the patient context overlay.
Agent.Services/
Contains background services.

PatientContextListener.cs – Monitors patient context changes (via FHIR Subscriptions or polling).
AdminPanel/
The Blazor WebAssembly admin interface.

Provides dashboards for real-time monitoring, configuration, and system logs.
Tests/
Contains unit and integration tests (using xUnit) to verify FHIR interactions and business logic.

Configuration Files:

appsettings.json – Stores EHR endpoints, SMART on FHIR credentials, secondary FHIR server details, and polling parameters.
.cursorrules – Contains context rules and links to key documentation (e.g., Firely .NET SDK, SMART on FHIR spec, Athenahealth developer docs).
Getting Started
Prerequisites
.NET 8.0 SDK (or later as required)
Visual Studio 2022 or VS Code
Node.js and npm (for Firebase tools)
Firebase CLI
Development Setup
Clone the Repository:

bash
Copy
git clone https://github.com/BigRedOak/FHIRagent.git
cd FHIRagent
Build the Solution:

bash
Copy
dotnet build
Run the Components:

Desktop Agent:
bash
Copy
cd Agent.UI
dotnet run
Admin Panel:
bash
Copy
cd ../AdminPanel
dotnet run
Testing:

Run tests from the root directory:

bash
Copy
dotnet test
Configuration
Update appsettings.json with:
Primary EHR endpoint URLs and SMART on FHIR credentials (for Athenahealth).
Secondary FHIR server endpoint (e.g., HAPI FHIR Public Test Server).
Polling intervals or FHIR Subscription parameters.
Use the .cursorrules file to include links and context rules for development.
Deployment
Desktop Agent
Publish the desktop agent and distribute it to workstations:

bash
Copy
dotnet publish Agent.UI -c Release
Admin Panel
Publish the admin panel and deploy via Firebase:

bash
Copy
dotnet publish AdminPanel -c Release
firebase deploy
Security Features
SMART on FHIR authentication flow
Role-based access control
Audit logging
Encrypted data transmission (HTTPS)
HIPAA compliance measures
Contributing
Fork the repository.
Create a feature branch.
Submit a pull request.
Support
Documentation Wiki
Issue Tracker
Discussions
License
This project is licensed under the MIT License – see the LICENSE file for details.

Overview of the SMART on FHIR Agent
This workspace contains a desktop agent application designed for primary care physicians to interface with Athenahealth's EHR using SMART on FHIR parameters. The agent is responsible for:

Detecting when a physician launches a patient record.
Harvesting and aggregating patient context (demographics, conditions, medications, observations, etc.) via FHIR R4 queries.
Pushing the aggregated data to a secondary FHIR server (e.g., HAPI FHIR Public Test Server).
Writing updates or additional notes back to the EHR.
Launching a responsive web overlay that displays updated patient context.
Listening for patient context changes (via FHIR Subscriptions or polling) to keep the UI and backend synchronized.
