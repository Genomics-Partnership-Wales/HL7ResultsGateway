using HL7ResultsGateway.Application.UseCases.ProcessHL7Message;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using System.Text;

namespace HL7ResultsGateway.API;

public class ProcessHL7Message
{
    private readonly ILogger<ProcessHL7Message> _logger;
    private readonly IProcessHL7MessageHandler _handler;

    public ProcessHL7Message(
        ILogger<ProcessHL7Message> logger,
        IProcessHL7MessageHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [Function("ProcessHL7Message")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "hl7/process")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing HL7 message request");

        try
        {
            // Read the request body
            using var reader = new StreamReader(req.Body, Encoding.UTF8);
            var requestBody = await reader.ReadToEndAsync(cancellationToken);

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogWarning("Received empty request body");
                return new BadRequestObjectResult(new { error = "Request body cannot be empty" });
            }

            // Get source from query parameters or headers
            var source = req.Query["source"].FirstOrDefault()
                        ?? req.Headers["X-Source"].FirstOrDefault()
                        ?? "Unknown";

            // Create command and process
            var command = new ProcessHL7MessageCommand(requestBody, source);
            var result = await _handler.Handle(command, cancellationToken);

            if (result.Success)
            {
                _logger.LogInformation("Successfully processed HL7 message from source: {Source}", source);
                return new OkObjectResult(new
                {
                    success = true,
                    processedAt = result.ProcessedAt,
                    messageType = result.ParsedMessage?.MessageType.ToString(),
                    patientId = result.ParsedMessage?.Patient?.PatientId,
                    observationCount = result.ParsedMessage?.Observations.Count ?? 0
                });
            }
            else
            {
                _logger.LogWarning("Failed to process HL7 message: {Error}", result.ErrorMessage);
                return new BadRequestObjectResult(new
                {
                    success = false,
                    error = result.ErrorMessage,
                    processedAt = result.ProcessedAt
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing HL7 message");
            return new StatusCodeResult(500);
        }
    }
}
