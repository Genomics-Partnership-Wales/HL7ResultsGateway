using FluentAssertions;

using HL7ResultsGateway.Application.UseCases.ProcessHL7Message;
using HL7ResultsGateway.Domain.Entities;
using HL7ResultsGateway.Domain.Services;
using HL7ResultsGateway.Domain.ValueObjects;

using Microsoft.Extensions.Logging;

using Moq;

namespace HL7ResultsGateway.Application.Tests.UseCases.ProcessHL7Message;

public class ProcessHL7MessageHandlerTests
{
    private readonly Mock<IHL7MessageParser> _mockParser;
    private readonly Mock<ILogger<ProcessHL7MessageHandler>> _mockLogger;
    private readonly ProcessHL7MessageHandler _handler;

    public ProcessHL7MessageHandlerTests()
    {
        _mockParser = new Mock<IHL7MessageParser>();
        _mockLogger = new Mock<ILogger<ProcessHL7MessageHandler>>();
        _handler = new ProcessHL7MessageHandler(_mockParser.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_WithValidHL7Message_ShouldReturnSuccessResult()
    {
        // Arrange
        var command = new ProcessHL7MessageCommand(
            "MSH|^~\\&|LAB|HOSPITAL|EMR|CLINIC|202508291030||ORU^R01|12345|P|2.5\r\nPID|1||123456789||DOE^JOHN^M||19850315|M|||123 MAIN ST^^ANYTOWN^ST^12345",
            "TestSource"
        );

        var expectedParsedResult = new HL7Result
        {
            MessageType = HL7MessageType.ORU_R01,
            Patient = new Patient
            {
                PatientId = "123456789",
                FirstName = "JOHN",
                LastName = "DOE"
            }
        };

        _mockParser.Setup(x => x.ParseMessage(command.HL7Message))
                  .Returns(expectedParsedResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.ParsedMessage.Should().NotBeNull();
        result.ParsedMessage!.MessageType.Should().Be(HL7MessageType.ORU_R01);
        result.ParsedMessage.Patient.PatientId.Should().Be("123456789");
        result.ErrorMessage.Should().BeNull();
        result.ProcessedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _mockParser.Verify(x => x.ParseMessage(command.HL7Message), Times.Once);
    }
}
