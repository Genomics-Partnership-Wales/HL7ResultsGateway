# Copilot Instructions for HL7 Results Gateway

## Architecture Overview
This is a **Clean Architecture** HL7 message processing system with **Azure Functions** as the API layer:

- **Domain** (`src/HL7ResultsGateway.Domain/`): Core entities, value objects, and domain services
- **Application** (`src/HL7ResultsGateway.Application/`): Use cases and handlers (follows CQRS pattern)
- **API** (`src/HL7ResultsGateway.API/`): Azure Functions HTTP triggers (.NET Isolated Worker)
- **Infrastructure** (`src/HL7ResultsGateway.Infrastructure/`): External concerns (placeholder)

## Key Patterns & Conventions

### Testing Standards
- **All tests use FluentAssertions** - Replace `Assert.Equal()` with `.Should().Be()`
- **xUnit with Moq** for mocking dependencies
- **Test structure**: Arrange/Act/Assert with clear comments
- **Example pattern**:
```csharp
result.Should().NotBeNull();
result.Success.Should().BeTrue();
result.ParsedMessage!.MessageType.Should().Be(HL7MessageType.ORU_R01);
```

### Code Style (.editorconfig enforced)
- **4-space indentation** for C#
- **Using directives**: Sort system first, separate groups with blank lines
- **Explicit accessibility modifiers** required (public/private/internal)
- **File-scoped namespaces** preferred
- **Remove unnecessary usings** (IDE0005 warning enabled)

### Domain Layer Patterns
- **HL7MessageParser**: Parses raw HL7 strings into strongly-typed `HL7Result` entities
- **Value Objects**: `HL7MessageType`, `Gender`, `ObservationStatus` (immutable enums)
- **Entities**: `HL7Result`, `Patient`, `Observation` (mutable domain objects)
- **Custom Exceptions**: `HL7ParseException` for domain-specific errors

### Application Layer (CQRS)
- **Commands**: `ProcessHL7MessageCommand` with message content and source
- **Handlers**: Implement `IProcessHL7MessageHandler` interface
- **Results**: Return success/failure objects with parsed data and timestamps
- **DI Pattern**: Constructor injection for all dependencies

### Azure Functions Specifics
- **Isolated Worker Model** (.NET 9.0 due to Azure Functions compatibility)
- **HTTP Triggers**: Use `[HttpTrigger(AuthorizationLevel.Function)]` for API endpoints
- **DI Registration**: Configure in `Program.cs` using `builder.Services.AddScoped<>()`
- **Health Check**: Anonymous auth level, returns service status with timestamp
- **Error Handling**: Return appropriate HTTP status codes (400/500) with error objects

## Development Workflow

### Build & Test Commands
```bash
# Build entire solution
dotnet build

# Run all tests
dotnet test

# Start Azure Functions locally
cd src/HL7ResultsGateway.API
func start
```

### Testing Endpoints
- Use `.http` files in API project: `health-check.http`, `process-hl7-message.http`
- Health check: `GET http://localhost:7071/api/health`
- HL7 processing: `POST http://localhost:7071/api/hl7/process` (Content-Type: text/plain)

### Adding New Features
1. **Domain First**: Create entities/value objects in Domain layer
2. **Application Logic**: Add use case handlers in Application layer
3. **API Endpoints**: Create HTTP trigger functions in API layer
4. **Tests**: Write tests for each layer using FluentAssertions
5. **HTTP Tests**: Add endpoint tests to appropriate `.http` file

## Project Dependencies
- **Target Framework**: .NET 9.0 (all projects)
- **Azure Functions**: Isolated Worker model with Core Tools v4.2.2+
- **Testing**: xUnit + FluentAssertions + Moq + Coverlet
- **Logging**: Microsoft.Extensions.Logging with Application Insights

## Critical Integration Points
- **DI Container**: Services registered in `Program.cs` must match interface implementations
- **HL7 Parser**: `IHL7MessageParser` is the core domain service for message processing
- **Layer Dependencies**: API → Application → Domain (never reverse)
- **Test Projects**: Mirror source structure and reference appropriate layers

## Common Gotchas
- **Azure Functions**: Use .NET 9.0, not .NET 10+ (compatibility limitation)
- **EditorConfig**: Auto-format on save to match 4-space indentation rules
- **FluentAssertions**: Always prefer `.Should()` syntax over Assert methods
- **Null Safety**: Enable nullable reference types, use `!` operator when needed for tests
