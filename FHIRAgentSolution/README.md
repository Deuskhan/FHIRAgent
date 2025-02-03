# FHIR Agent Solution

A comprehensive FHIR integration solution consisting of a desktop agent for healthcare providers and a centralized admin panel. The solution enables automated patient data synchronization between EHR systems (via SMART on FHIR) and secondary FHIR servers.

## Solution Components

### 1. AgentClient
Desktop/service application that runs on physicians' computers.
- Monitors EHR system for patient context
- Harvests patient data using SMART on FHIR
- Synchronizes data with secondary FHIR server
- Reports status to central admin panel

### 2. AdminPanel
Web-based administration interface for monitoring and managing FHIR agents.
- Dashboard showing agent status and statistics
- FHIR server configuration management
- Error and operational logging
- Agent deployment tracking

### 3. Common
Shared library containing core FHIR integration logic and models.
- FHIR client implementations
- Data models and DTOs
- Shared utilities and interfaces

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- Visual Studio 2022 or VS Code
- SQL Server (or SQLite for development)
- Access to a FHIR server (HAPI FHIR test server can be used for development)

### Setup Instructions

1. **Clone the Repository**
```bash
git clone [repository-url]
cd FHIRAgentSolution
```

2. **Restore Dependencies**
```bash
dotnet restore
```

3. **Configure Connection Strings**
- Update AdminPanel/appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=AdminPanel.db"
  },
  "FHIR": {
    "TestEndpoint": "http://hapi.fhir.org/baseR4"
  }
}
```
- Update AgentClient/appsettings.json:
```json
{
  "FHIR": {
    "EhrEndpoint": "http://hapi.fhir.org/baseR4",
    "SecondaryEndpoint": "http://hapi.fhir.org/baseR4"
  }
}
```

4. **Initialize Database**
```bash
cd AdminPanel
dotnet ef database update
```

5. **Build Solution**
```bash
dotnet build
```

## Running the Solution

### Running the Admin Panel
```bash
cd AdminPanel
dotnet run
```
Access the admin panel at https://localhost:5001

### Running the Agent Client
```bash
cd AgentClient
dotnet run
```

## Testing

### Unit Tests
```bash
dotnet test
```

### Integration Testing with HAPI FHIR
1. Configure test endpoints in appsettings.json to use HAPI FHIR test server
2. Run the integration test suite:
```bash
dotnet test --filter "Category=Integration"
```

### Manual Testing Steps
1. Start the AdminPanel
2. Launch an AgentClient instance
3. Verify agent registration in admin panel
4. Test FHIR synchronization:
   - Create test patient in HAPI FHIR
   - Verify data harvesting
   - Check secondary FHIR server for synchronized data

## Development Guidelines

### FHIR Integration
- Use Firely SDK for FHIR operations
- Follow SMART on FHIR authentication flows
- Implement proper error handling
- Log all FHIR operations

### Security
- Implement SMART on FHIR security
- Follow HIPAA compliance requirements
- Secure all PHI data
- Use proper authentication/authorization

### Coding Standards
- Follow SOLID principles
- Use async/await for I/O operations
- Implement comprehensive logging
- Write unit tests for business logic

## Project Structure
```
FHIRAgentSolution/
├── AgentClient/               # Desktop agent application
│   ├── Program.cs            # Entry point
│   └── appsettings.json      # Agent configuration
├── AdminPanel/               # Web admin interface
│   ├── Controllers/          # MVC controllers
│   ├── Views/                # Razor views
│   └── wwwroot/             # Static files
└── Common/                   # Shared library
    ├── FhirIntegration/     # FHIR connectivity
    └── Models/              # Shared data models
```

## Configuration

### FHIR Server Settings
- EHR Endpoint: Primary FHIR server (e.g., Athenahealth)
- Secondary Endpoint: Destination FHIR server
- Authentication: SMART on FHIR credentials

### Logging
- Uses Serilog for structured logging
- Console and file sinks configured
- Different log levels for development/production

## Troubleshooting

### Common Issues
1. FHIR Connection Issues
   - Verify endpoints in configuration
   - Check SMART on FHIR credentials
   - Confirm network connectivity

2. Database Issues
   - Verify connection string
   - Ensure database is initialized
   - Check EF Core migrations

3. Agent Registration Problems
   - Verify admin panel is running
   - Check network connectivity
   - Review agent logs

## References

- [Firely .NET SDK Documentation](https://docs.fire.ly/)
- [SMART on FHIR Specification](https://smarthealthit.org/)
- [HL7 FHIR R4 Specification](https://hl7.org/fhir/R4/index.html)
- [HAPI FHIR Public Test Server](http://hapi.fhir.org/baseR4)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

[Specify your license here]

## Comprehensive Testing Guide

### Prerequisites Setup
1. **HAPI FHIR Test Server Access**
   ```bash
   # Verify HAPI FHIR server is accessible
   curl http://hapi.fhir.org/baseR4/metadata
   ```

2. **Database Setup**
   ```bash
   # Navigate to AdminPanel directory
   cd AdminPanel
   
   # Apply EF Core migrations
   dotnet ef database update
   
   # Verify database creation
   sqlite3 AdminPanel.db ".tables"
   ```

### 1. Component Testing

#### Test Common Library
1. **Test FHIR Client Wrapper**
   ```bash
   # Navigate to test project
   cd Tests/Common.Tests
   
   # Run FHIR client tests
   dotnet test --filter "Category=FhirClient"
   ```

2. **Test Context Aggregator**
   ```bash
   # Test patient context aggregation
   dotnet test --filter "Category=ContextAggregator"
   ```

#### Test Admin Panel
1. **Start Admin Panel in Test Mode**
   ```bash
   cd AdminPanel
   dotnet run --environment "Testing"
   ```

2. **Run Admin Panel Tests**
   ```bash
   cd Tests/AdminPanel.Tests
   dotnet test --filter "Category=AdminPanel"
   ```

#### Test Agent Client
1. **Run Agent Client Tests**
   ```bash
   cd Tests/AgentClient.Tests
   dotnet test --filter "Category=AgentClient"
   ```

### 2. Integration Testing

#### Setup Test Environment
1. **Configure Test Settings**
   Update `appsettings.Testing.json`:
   ```json
   {
     "FHIR": {
       "EhrEndpoint": "http://hapi.fhir.org/baseR4",
       "SecondaryEndpoint": "http://hapi.fhir.org/baseR4"
     }
   }
   ```

2. **Prepare Test Data**
   ```bash
   # Run data seeding script
   cd Scripts
   ./seed-test-data.ps1
   ```

#### End-to-End Testing
1. **Start Admin Panel**
   ```bash
   cd AdminPanel
   dotnet run
   ```

2. **Start Agent Client**
   ```bash
   cd AgentClient
   dotnet run
   ```

3. **Test Workflow**
   - Create test patient in HAPI FHIR:
     ```bash
     curl -X POST http://hapi.fhir.org/baseR4/Patient \
       -H "Content-Type: application/json" \
       -d @Tests/TestData/test-patient.json
     ```
   
   - Monitor agent registration in admin panel
   - Verify data synchronization
   - Check logs for successful operations

### 3. Manual Testing Scenarios

#### Scenario 1: Agent Registration
1. Launch Admin Panel
2. Start new Agent Client instance
3. Verify in Admin Panel:
   - Agent appears in dashboard
   - Status shows as "Connected"
   - Machine info is correct

#### Scenario 2: Patient Data Synchronization
1. Use HAPI FHIR test server UI (http://hapi.fhir.org)
2. Create test patient with conditions and observations
3. Verify in Agent Client:
   - Patient context is detected
   - Resources are harvested
   - Data is pushed to secondary server

#### Scenario 3: Error Handling
1. Test network interruption:
   - Disconnect network while agent is running
   - Verify error logging
   - Check recovery when connection restored

2. Test invalid FHIR resources:
   - Submit malformed FHIR data
   - Verify error handling
   - Check error logs in Admin Panel

### 4. Performance Testing

#### Load Testing
1. **Multiple Agents**
   ```bash
   # Launch multiple agent instances
   for i in {1..5}; do
     start AgentClient/bin/Debug/net7.0/AgentClient.exe
   done
   ```

2. **Bulk Data Operations**
   - Monitor system performance
   - Check database response times
   - Verify admin panel responsiveness

### 5. Security Testing

1. **SMART on FHIR Authentication**
   - Verify OAuth2 flow
   - Test token refresh
   - Check scope enforcement

2. **Data Protection**
   - Verify PHI encryption
   - Test secure communication
   - Validate audit logging

### Troubleshooting Tests

Common testing issues and solutions:

1. **Database Connection Issues**
   ```bash
   # Verify SQLite database
   sqlite3 AdminPanel.db ".tables"
   ```

2. **FHIR Server Connectivity**
   ```bash
   # Test FHIR server connection
   curl -v http://hapi.fhir.org/baseR4/metadata
   ```

3. **Agent Registration Problems**
   - Check network connectivity
   - Verify configuration settings
   - Review agent logs

### Test Reporting

Generate test reports:
```bash
# Generate coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Generate HTML report
reportgenerator -reports:"./coverage.opencover.xml" -targetdir:"./coveragereport"
```

### Continuous Integration Testing

The solution includes GitHub Actions workflows for automated testing:
- Unit tests run on every push
- Integration tests run on PR to main
- Full test suite runs on release tags 