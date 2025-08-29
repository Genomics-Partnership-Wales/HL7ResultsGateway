namespace HL7ResultsGateway.Domain.Exceptions;

public class HL7ParseException : Exception
{
    public HL7ParseException(string message) : base(message)
    {
    }

    public HL7ParseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}