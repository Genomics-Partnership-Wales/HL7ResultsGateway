namespace HL7ResultsGateway.Domain.Services;

public class HL7MessageParser : IHL7MessageParser
{
    public HL7Result ParseMessage(string message)
    {
        // This will make the test fail (RED phase) - we'll implement this properly in GREEN phase
        throw new NotImplementedException("Parser not implemented yet");
    }
}