using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.Domain.Entities;

public class HL7Result
{
    public HL7MessageType MessageType { get; set; }
    public Patient Patient { get; set; } = new();
    public List<Observation> Observations { get; set; } = new();
}