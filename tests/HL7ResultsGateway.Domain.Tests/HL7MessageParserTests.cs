namespace HL7ResultsGateway.Domain.Tests;

public class HL7MessageParserTests
{
    private readonly IHL7MessageParser _parser;

    public HL7MessageParserTests()
    {
        _parser = new HL7MessageParser();
    }

    [Fact]
    public void ParseMessage_WithValidORUMessage_ShouldReturnParsedResult()
    {
        // Arrange
        var hl7Message = @"MSH|^~\&|LAB|HOSPITAL|EMR|CLINIC|202508291030||ORU^R01|12345|P|2.5
                            PID|1||123456789||DOE^JOHN^M||19850315|M|||123 MAIN ST^^ANYTOWN^ST^12345
                            OBR|1||LAB001|CBC^Complete Blood Count
                            OBX|1|NM|WBC^White Blood Cell Count|1|7.5|10*3/uL|4.0-11.0|N|||F
                            OBX|2|NM|RBC^Red Blood Cell Count|1|4.5|10*6/uL|4.2-5.4|N|||F
                            OBX|3|NM|HGB^Hemoglobin|1|14.2|g/dL|12.0-16.0|N|||F";

        // Act
        var result = _parser.ParseMessage(hl7Message);

        // Assert - RED test - these will fail until we implement the classes
        result.Should().NotBeNull();
        result.MessageType.Should().Be(HL7MessageType.ORU_R01);
        result.Patient.Should().NotBeNull();
        result.Patient.PatientId.Should().Be("123456789");
        result.Patient.LastName.Should().Be("DOE");
        result.Patient.FirstName.Should().Be("JOHN");
        result.Patient.MiddleName.Should().Be("M");
        result.Patient.DateOfBirth.Should().Be(new DateTime(1985, 3, 15));
        result.Patient.Gender.Should().Be(Gender.Male);

        result.Observations.Should().HaveCount(3);
        var observation1 = result.Observations[0];
        observation1.ObservationId.Should().Be("WBC");
        observation1.Description.Should().Be("White Blood Cell Count");
        observation1.Value.Should().Be("7.5");
        observation1.Units.Should().Be("10*3/uL");
        observation1.ReferenceRange.Should().Be("4.0-11.0");
        observation1.Status.Should().Be(ObservationStatus.Normal);

        var observation2 = result.Observations[1];
        observation2.ObservationId.Should().Be("RBC");
        var observation3 = result.Observations[2];
        observation3.ObservationId.Should().Be("HGB");
        observation3.ValueType.Should().Be("NM");
        observation3.Value.Should().Be("14.2");
    }

    [Fact]
    public void ParseMessage_WithInvalidMessage_ShouldThrowHL7ParseException()
    {
        // Arrange
        var invalidHl7Message = "INVALID MESSAGE";

        // Act & Assert
        Action act = () => _parser.ParseMessage(invalidHl7Message);
        act.Should().Throw<HL7ParseException>()
            .WithMessage("*Invalid HL7 message format*");
    }

    [Fact]
    public void ParseMessage_WithEmptyMessage_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyMessage = string.Empty;

        // Act & Assert
        Action act = () => _parser.ParseMessage(emptyMessage);
        act.Should().Throw<ArgumentException>()
            .WithMessage("HL7 message cannot be null or empty*");
    }
}
