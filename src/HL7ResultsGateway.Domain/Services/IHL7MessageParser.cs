using HL7ResultsGateway.Domain.Entities;

namespace HL7ResultsGateway.Domain.Services;

public interface IHL7MessageParser
{
    HL7Result ParseMessage(string message);
}
