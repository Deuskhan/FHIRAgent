# Seed test data script
$baseUrl = "http://hapi.fhir.org/baseR4"

# Create test patient
$patientJson = Get-Content "../Tests/TestData/test-patient.json"
$response = Invoke-RestMethod -Uri "$baseUrl/Patient" -Method Post -Body $patientJson -ContentType "application/json"
Write-Host "Created test patient with ID: $($response.id)"

# Add test condition
$conditionJson = @"
{
    "resourceType": "Condition",
    "subject": {
        "reference": "Patient/$($response.id)"
    },
    "code": {
        "coding": [
            {
                "system": "http://snomed.info/sct",
                "code": "73211009",
                "display": "Diabetes mellitus"
            }
        ]
    },
    "verificationStatus": {
        "coding": [
            {
                "system": "http://terminology.hl7.org/CodeSystem/condition-ver-status",
                "code": "confirmed"
            }
        ]
    }
}
"@

$response = Invoke-RestMethod -Uri "$baseUrl/Condition" -Method Post -Body $conditionJson -ContentType "application/json"
Write-Host "Created test condition with ID: $($response.id)" 