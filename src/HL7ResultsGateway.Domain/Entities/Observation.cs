using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.Domain.Entities;

public class Observation
{
    public string ObservationId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
    public string ReferenceRange { get; set; } = string.Empty;
    public ObservationStatus Status { get; set; }
    public string ValueType { get; set; } = string.Empty;
}