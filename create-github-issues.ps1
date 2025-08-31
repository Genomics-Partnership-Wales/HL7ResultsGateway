# GitHub Issues Creation Script for Azure Function API Implementation
# Based on plan/feature-azure-function-api-1.md

# Ensure we're in the correct directory
Set-Location "c:\Users\Public\WEBDEV\HL7ResultsGateway"

# Check if gh CLI is authenticated
Write-Host "Checking GitHub CLI authentication..." -ForegroundColor Yellow
gh auth status

# Create Issue 1: Prerequisites and Environment Setup
Write-Host "Creating Issue 1: Prerequisites and Environment Setup..." -ForegroundColor Green
$issue1Body = @"
## 🎯 Overview
Set up the development environment prerequisites for scaffolding Azure Function API with .NET 10 Preview Isolated Worker Runtime.

## 🎯 Goal
GOAL-001: Install and verify Azure Functions Core Tools and prepare development environment

## 📋 Requirements
- **REQ-001**: Create Azure Function project using .NET 10 Preview Isolated Worker Runtime for better performance and latest language features
- **REQ-003**: Use Azure Functions Core Tools v4.x for project scaffolding and local development

## ✅ Tasks
- [ ] **TASK-001**: Verify Azure Functions Core Tools v4.x installation
- [ ] **TASK-002**: Verify .NET 10 Preview SDK installation and compatibility
- [ ] **TASK-003**: Navigate to project root directory (HL7ResultsGateway)
- [ ] **TASK-004**: Create src directory if not exists for Azure Functions project

## 🔗 Dependencies
- **DEP-001**: Azure Functions Core Tools v4.x must be installed on development machine
- **DEP-002**: .NET 10 Preview SDK must be installed and configured
- **DEP-005**: PowerShell or Command Prompt access for running CLI commands

## ✅ Acceptance Criteria
- [ ] Azure Functions Core Tools v4.x is installed and functional
- [ ] .NET 10 Preview SDK is installed and can target net10.0 framework
- [ ] Project directory structure is prepared for Azure Functions scaffolding
- [ ] All tooling is verified and ready for development

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 1
"@

$issue1 = gh issue create `
    --title "Phase 1: Prerequisites and Environment Setup for Azure Function API" `
    --body $issue1Body `
    --label "feature,azure-functions,environment-setup,dotnet10-preview"

Write-Host "Created Issue 1: $issue1" -ForegroundColor Green

# Create Issue 2: Azure Function Project Scaffolding
Write-Host "Creating Issue 2: Azure Function Project Scaffolding..." -ForegroundColor Green
$issue2Body = @"
## 🎯 Overview
Create and configure Azure Function project with .NET 10 Preview Isolated Worker Runtime.

## 🎯 Goal
GOAL-002: Create and configure Azure Function project with .NET Isolated Worker Runtime

## 📋 Requirements
- **REQ-001**: Create Azure Function project using .NET 10 Preview Isolated Worker Runtime
- **REQ-002**: Project must be named ``HL7ResultsGateway.API`` to align with existing solution structure
- **REQ-007**: Follow C# naming conventions and .NET project structure standards

## ✅ Tasks
- [ ] **TASK-005**: Execute ``func init src/HL7ResultsGateway.API --worker-runtime dotnet-isolated --target-framework net10.0``
- [ ] **TASK-006**: Navigate to the newly created project directory
- [ ] **TASK-007**: Verify project structure and generated files
- [ ] **TASK-008**: Update .csproj file to target .NET 10 Preview and align with solution standards

## 🔗 Dependencies
- **Depends on**: $issue1 (Prerequisites and Environment Setup)
- **DEP-001**: Azure Functions Core Tools v4.x functional
- **DEP-002**: .NET 10 Preview SDK configured

## ✅ Acceptance Criteria
- [ ] Azure Function project ``HL7ResultsGateway.API`` is created in ``src/`` directory
- [ ] Project targets .NET 10 Preview framework (net10.0)
- [ ] Project uses Isolated Worker Runtime model
- [ ] Generated project structure follows Azure Functions standards
- [ ] .csproj file is properly configured

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 2
"@

$issue2 = gh issue create `
    --title "Phase 2: Azure Function Project Scaffolding with .NET 10 Preview Isolated Worker" `
    --body $issue2Body `
    --label "feature,azure-functions,scaffolding,dotnet10-preview,dotnet-isolated"

Write-Host "Created Issue 2: $issue2" -ForegroundColor Green

# Create Issue 3: HTTP Trigger Function Creation
Write-Host "Creating Issue 3: HTTP Trigger Function Creation..." -ForegroundColor Green
$issue3Body = @"
## 🎯 Overview
Create HTTP trigger functions for HL7 message processing and health checks.

## 🎯 Goal
GOAL-003: Create HTTP trigger functions for HL7 message processing

## 📋 Requirements
- **REQ-004**: Implement HTTP trigger function for HL7 message processing endpoint
- **GUD-003**: Implement health check endpoints for monitoring
- **PAT-003**: Follow RESTful API design patterns for HTTP endpoints

## ✅ Tasks
- [ ] **TASK-009**: Create HTTP trigger function for HL7 message processing using ``func new --template "Http Trigger" --name ProcessHL7Message``
- [ ] **TASK-010**: Create health check HTTP trigger function using ``func new --template "Http Trigger" --name HealthCheck``
- [ ] **TASK-011**: Configure function authorization levels appropriately
- [ ] **TASK-012**: Update function signatures to use proper request/response types

## 🔗 Dependencies
- **Depends on**: $issue2 (Azure Function Project Scaffolding)
- **CON-004**: .NET 10 Preview compatibility with Azure Functions runtime and tooling

## ✅ Acceptance Criteria
- [ ] ProcessHL7Message HTTP trigger function is created and functional
- [ ] HealthCheck HTTP trigger function is created and functional
- [ ] Functions use appropriate authorization levels
- [ ] Function signatures use proper HTTP request/response types
- [ ] Functions follow RESTful API patterns

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 3
"@

$issue3 = gh issue create `
    --title "Phase 3: HTTP Trigger Function Creation for HL7 Processing" `
    --body $issue3Body `
    --label "feature,azure-functions,http-triggers,hl7-processing"

Write-Host "Created Issue 3: $issue3" -ForegroundColor Green

# Create Issue 4: Project Integration and Configuration
Write-Host "Creating Issue 4: Project Integration and Configuration..." -ForegroundColor Green
$issue4Body = @"
## 🎯 Overview
Integrate Azure Function project with existing solution and configure dependencies.

## 🎯 Goal
GOAL-004: Integrate Azure Function project with existing solution and configure dependencies

## 📋 Requirements
- **REQ-005**: Configure proper project structure to integrate with existing domain and application layers
- **REQ-006**: Include proper configuration for local development and Azure deployment
- **CON-001**: Must integrate with existing HL7ResultsGateway solution structure
- **CON-002**: Function app must support ASP.NET Core integration for dependency injection
- **PAT-001**: Use dependency injection pattern for service registration

## ✅ Tasks
- [ ] **TASK-013**: Add project reference to HL7ResultsGateway.Application layer
- [ ] **TASK-014**: Add project reference to HL7ResultsGateway.Domain layer
- [ ] **TASK-015**: Configure dependency injection in Program.cs
- [ ] **TASK-016**: Update local.settings.json with required configuration
- [ ] **TASK-017**: Create .funcignore file to exclude unnecessary files from deployment
- [ ] **TASK-018**: Add Azure Function project to solution file

## 🔗 Dependencies
- **Depends on**: $issue3 (HTTP Trigger Function Creation)
- **DEP-003**: Existing HL7ResultsGateway.Application project
- **DEP-004**: Existing HL7ResultsGateway.Domain project

## 🔐 Security Considerations
- **SEC-001**: Configure authorization levels appropriately for production and development environments
- **SEC-002**: Implement secure connection string management using local.settings.json
- **SEC-003**: Ensure sensitive configuration is excluded from version control

## ✅ Acceptance Criteria
- [ ] Azure Function project references Application and Domain layers
- [ ] Dependency injection is properly configured in Program.cs
- [ ] local.settings.json contains required configuration
- [ ] .funcignore file excludes unnecessary deployment files
- [ ] Azure Function project is added to HL7ResultsGateway.sln
- [ ] Configuration follows security best practices

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 4
"@

$issue4 = gh issue create `
    --title "Phase 4: Project Integration and Dependency Configuration" `
    --body $issue4Body `
    --label "feature,azure-functions,integration,dependency-injection,configuration"

Write-Host "Created Issue 4: $issue4" -ForegroundColor Green

# Create Issue 5: Azure Functions Test Project Creation
Write-Host "Creating Issue 5: Azure Functions Test Project Creation..." -ForegroundColor Green
$issue5Body = @"
## 🎯 Overview
Create dedicated test project for Azure Functions with .NET 10 Preview and Azure Functions test framework.

## 🎯 Goal
GOAL-005: Create dedicated test project for Azure Functions with .NET 10 Preview

## 📋 Requirements
- **REQ-008**: Create dedicated Azure Functions test project ``HL7ResultsGateway.API.Tests`` for comprehensive testing

## ✅ Tasks
- [ ] **TASK-019**: Create Azure Functions test project using ``dotnet new xunit -n HL7ResultsGateway.API.Tests`` in tests directory
- [ ] **TASK-020**: Update test project to target .NET 10 Preview framework
- [ ] **TASK-021**: Add Microsoft.Azure.Functions.Worker.TestFramework NuGet package
- [ ] **TASK-022**: Add project reference from test project to HL7ResultsGateway.API
- [ ] **TASK-023**: Add project references to HL7ResultsGateway.Application and Domain for integration tests
- [ ] **TASK-024**: Configure test project with proper test categories and organizational structure
- [ ] **TASK-025**: Add Azure Functions test project to solution file

## 🔗 Dependencies
- **Depends on**: $issue4 (Project Integration and Configuration)
- **DEP-006**: Microsoft.Azure.Functions.Worker.TestFramework NuGet package for testing

## ✅ Acceptance Criteria
- [ ] Test project ``HL7ResultsGateway.API.Tests`` is created in ``tests/`` directory
- [ ] Test project targets .NET 10 Preview framework
- [ ] Azure Functions Worker Test Framework is installed and configured
- [ ] Test project references main Azure Function project and dependencies
- [ ] Test project is added to solution file
- [ ] Test project structure supports unit and integration tests

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 5
"@

$issue5 = gh issue create `
    --title "Phase 5: Azure Functions Test Project Creation with .NET 10 Preview" `
    --body $issue5Body `
    --label "feature,testing,azure-functions,dotnet10-preview,test-project"

Write-Host "Created Issue 5: $issue5" -ForegroundColor Green

# Create Issue 6: Testing and Validation
Write-Host "Creating Issue 6: Testing and Validation..." -ForegroundColor Green
$issue6Body = @"
## 🎯 Overview
Verify Azure Function project setup, local execution, and comprehensive testing.

## 🎯 Goal
GOAL-006: Verify Azure Function project setup and local execution

## 📋 Requirements
- **GUD-002**: Use structured logging and proper error handling patterns
- **CON-003**: Local development must work with existing project dependencies

## ✅ Tasks
- [ ] **TASK-026**: Build the Azure Function project successfully
- [ ] **TASK-027**: Start local Azure Functions runtime using ``func start``
- [ ] **TASK-028**: Test HTTP trigger endpoints using curl or HTTP client
- [ ] **TASK-029**: Verify integration with existing application layers
- [ ] **TASK-030**: Validate project structure and naming conventions
- [ ] **TASK-031**: Run Azure Functions test suite and verify all tests pass
- [ ] **TASK-032**: Validate .NET 10 Preview compatibility with Azure Functions runtime

## 🔗 Dependencies
- **Depends on**: $issue5 (Azure Functions Test Project Creation)

## 🧪 Testing Requirements
- **TEST-001**: Unit tests for HTTP trigger functions using Azure Functions test framework
- **TEST-002**: Integration tests for Azure Function startup and dependency injection
- **TEST-003**: Local runtime testing using ``func start`` command
- **TEST-004**: HTTP endpoint testing using curl or Postman for ProcessHL7Message function
- **TEST-005**: HTTP endpoint testing for HealthCheck function
- **TEST-006**: Validation of project build and compilation
- **TEST-007**: Azure Functions Worker test framework integration tests
- **TEST-008**: Mock HTTP request/response testing for all function endpoints
- **TEST-009**: Dependency injection container validation tests
- **TEST-010**: .NET 10 Preview feature compatibility tests with Azure Functions runtime

## ✅ Acceptance Criteria
- [ ] Azure Function project builds without errors
- [ ] Local Azure Functions runtime starts successfully
- [ ] HTTP trigger endpoints respond correctly to test requests
- [ ] Integration with existing application layers is functional
- [ ] All unit and integration tests pass
- [ ] .NET 10 Preview compatibility is validated
- [ ] Project follows naming conventions and best practices

## 📄 Related Implementation Plan
From: ``plan/feature-azure-function-api-1.md`` - Phase 6
"@

$issue6 = gh issue create `
    --title "Phase 6: Testing and Validation of Azure Function API" `
    --body $issue6Body `
    --label "feature,testing,validation,azure-functions,quality-assurance"

Write-Host "Created Issue 6: $issue6" -ForegroundColor Green

# Summary
Write-Host "`n✅ Successfully created all 6 GitHub issues!" -ForegroundColor Green
Write-Host "Issues created:" -ForegroundColor Yellow
Write-Host "1. $issue1" -ForegroundColor Cyan
Write-Host "2. $issue2" -ForegroundColor Cyan
Write-Host "3. $issue3" -ForegroundColor Cyan
Write-Host "4. $issue4" -ForegroundColor Cyan
Write-Host "5. $issue5" -ForegroundColor Cyan
Write-Host "6. $issue6" -ForegroundColor Cyan

Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Review the created issues on GitHub" -ForegroundColor White
Write-Host "2. Assign team members to appropriate issues" -ForegroundColor White
Write-Host "3. Begin with Issue 1 (Prerequisites and Environment Setup)" -ForegroundColor White
Write-Host "4. Follow the dependency chain: 1 → 2 → 3 → 4 → 5 → 6" -ForegroundColor White
