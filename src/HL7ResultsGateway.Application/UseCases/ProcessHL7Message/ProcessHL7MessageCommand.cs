namespace HL7ResultsGateway.Application.UseCases.ProcessHL7Message;

public record ProcessHL7MessageCommand(string HL7Message, string Source);
