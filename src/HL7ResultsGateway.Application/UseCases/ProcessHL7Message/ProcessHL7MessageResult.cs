using HL7ResultsGateway.Domain.Entities;

namespace HL7ResultsGateway.Application.UseCases.ProcessHL7Message;

public record ProcessHL7MessageResult(
    bool Success,
    HL7Result? ParsedMessage,
    string? ErrorMessage,
    DateTime ProcessedAt);