
using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.Domain.Entities;

public class Patient
{
    public string PatientId { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
}