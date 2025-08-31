using HL7ResultsGateway.Application.UseCases.ProcessHL7Message;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using System.Net;
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
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "hl7/process")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing HL7 message request");

        try
        {
            // Read the request body
            var requestBody = await req.ReadAsStringAsync();

            if (string.IsNullOrEmpty(requestBody))
            {
                _logger.LogWarning("Received empty request body");
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await badResponse.WriteAsJsonAsync(new { error = "Request body cannot be empty" });
                return badResponse;
            }

            // Get source from query parameters or headers
            var source = req.Query["source"]
                        ?? req.Headers.GetValues("X-Source")?.FirstOrDefault()
                        ?? "Unknown";

            // Create command and process
            var command = new ProcessHL7MessageCommand(requestBody, source);
            var result = await _handler.Handle(command, cancellationToken);

            var response = req.CreateResponse(result.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (result.Success)
            {
                _logger.LogInformation("Successfully processed HL7 message from source: {Source}", source);
                await response.WriteAsJsonAsync(new
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
                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    error = result.ErrorMessage,
                    processedAt = result.ProcessedAt
                });
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing HL7 message");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await errorResponse.WriteAsJsonAsync(new { error = "Internal server error" });
            return errorResponse;
        }
    }
}
