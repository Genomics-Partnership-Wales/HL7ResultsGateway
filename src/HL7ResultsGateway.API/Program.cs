using HL7ResultsGateway.Application.UseCases.ProcessHL7Message;
using HL7ResultsGateway.Domain.Services;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Configure Function Worker Defaults
builder.Services.AddApplicationInsightsTelemetryWorkerService();
builder.Services.ConfigureFunctionsApplicationInsights();

// Register application services
builder.Services.AddScoped<IHL7MessageParser, HL7MessageParser>();
builder.Services.AddScoped<IProcessHL7MessageHandler, ProcessHL7MessageHandler>();

// Configure logging
builder.Services.AddLogging();

builder.Build().Run();
