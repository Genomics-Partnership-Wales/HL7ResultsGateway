namespace HL7ResultsGateway.Domain.ValueObjects;

public enum HL7MessageType
{
    Unknown,
    ORU_R01, // Observation Result
    ADT_A01, // Admit Patient
    ADT_A03, // Discharge Patient
    ORM_O01  // Order Message
}
