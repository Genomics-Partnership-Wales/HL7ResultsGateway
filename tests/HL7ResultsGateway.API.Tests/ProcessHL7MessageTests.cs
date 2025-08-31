using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Text;
using HL7ResultsGateway.API;
using HL7ResultsGateway.Application.UseCases.ProcessHL7Message;
using HL7ResultsGateway.Domain.Entities;
using HL7ResultsGateway.Domain.ValueObjects;

namespace HL7ResultsGateway.API.Tests;

public class ProcessHL7MessageTests
{
    private readonly Mock<ILogger<ProcessHL7Message>> _mockLogger;
    private readonly Mock<IProcessHL7MessageHandler> _mockHandler;
    private readonly ProcessHL7Message _processHL7Message;

    public ProcessHL7MessageTests()
    {
        _mockLogger = new Mock<ILogger<ProcessHL7Message>>();
        _mockHandler = new Mock<IProcessHL7MessageHandler>();
        _processHL7Message = new ProcessHL7Message(_mockLogger.Object, _mockHandler.Object);
    }

    [Fact]
    public async Task Run_WithValidHL7Message_ShouldReturnSuccess()
    {
        // Arrange
        var hl7Message = "MSH|^~\\&|LAB|HOSPITAL|LIS|HOSPITAL|20240101120000||ORU^R01|123|P|2.5\r\nPID|||12345||Doe^John^||19800101|M\r\nOBX|1|NM|GLU^Glucose||100|mg/dL|70-100|N|||F";

        var mockRequest = CreateMockRequest(hl7Message, "TestSource");

        var expectedResult = new ProcessHL7MessageResult(
            Success: true,
            ParsedMessage: new HL7Result
            {
                MessageType = HL7MessageType.ORU_R01,
                Patient = new Patient { PatientId = "12345", FirstName = "John", LastName = "Doe" },
                Observations = new List<Observation> { new Observation { ObservationId = "GLU", Value = "100" } }
            },
            ErrorMessage: null,
            ProcessedAt: DateTime.UtcNow);

        _mockHandler.Setup(h => h.Handle(It.IsAny<ProcessHL7MessageCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _processHL7Message.Run(mockRequest, CancellationToken.None);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;

        var response = okResult.Value;
        Assert.NotNull(response);

        // Verify handler was called with correct parameters
        _mockHandler.Verify(h => h.Handle(
            It.Is<ProcessHL7MessageCommand>(cmd => cmd.HL7Message == hl7Message && cmd.Source == "TestSource"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_WithEmptyRequestBody_ShouldReturnBadRequest()
    {
        // Arrange
        var mockRequest = CreateMockRequest("", "TestSource");

        // Act
        var result = await _processHL7Message.Run(mockRequest, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;

        var response = badRequestResult.Value;
        Assert.NotNull(response);

        // Check that error message is present
        var responseType = response.GetType();
        var errorProperty = responseType.GetProperty("error");
        Assert.NotNull(errorProperty);
        Assert.Equal("Request body cannot be empty", errorProperty.GetValue(response));
    }

    [Fact]
    public async Task Run_WithFailedProcessing_ShouldReturnBadRequest()
    {
        // Arrange
        var hl7Message = "Invalid HL7 message";
        var mockRequest = CreateMockRequest(hl7Message, "TestSource");

        var expectedResult = new ProcessHL7MessageResult(
            Success: false,
            ParsedMessage: null,
            ErrorMessage: "Invalid HL7 message format",
            ProcessedAt: DateTime.UtcNow);

        _mockHandler.Setup(h => h.Handle(It.IsAny<ProcessHL7MessageCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedResult);

        // Act
        var result = await _processHL7Message.Run(mockRequest, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;

        var response = badRequestResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var successProperty = responseType.GetProperty("success");
        var errorProperty = responseType.GetProperty("error");

        Assert.NotNull(successProperty);
        Assert.NotNull(errorProperty);
        Assert.Equal(false, successProperty.GetValue(response));
        Assert.Equal("Invalid HL7 message format", errorProperty.GetValue(response));
    }

    [Fact]
    public async Task Run_WithUnexpectedException_ShouldReturnInternalServerError()
    {
        // Arrange
        var hl7Message = "MSH|^~\\&|LAB|HOSPITAL|LIS|HOSPITAL|20240101120000||ORU^R01|123|P|2.5";
        var mockRequest = CreateMockRequest(hl7Message, "TestSource");

        _mockHandler.Setup(h => h.Handle(It.IsAny<ProcessHL7MessageCommand>(), It.IsAny<CancellationToken>()))
                   .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _processHL7Message.Run(mockRequest, CancellationToken.None);

        // Assert
        Assert.IsType<StatusCodeResult>(result);
        var statusResult = (StatusCodeResult)result;
        Assert.Equal(500, statusResult.StatusCode);
    }

    private static HttpRequest CreateMockRequest(string body, string source)
    {
        var mockRequest = new Mock<HttpRequest>();
        var mockQuery = new Mock<IQueryCollection>();
        var mockHeaders = new Mock<IHeaderDictionary>();

        // Setup request body
        var bodyStream = new MemoryStream(Encoding.UTF8.GetBytes(body));
        mockRequest.Setup(r => r.Body).Returns(bodyStream);

        // Setup query parameters
        mockQuery.Setup(q => q["source"]).Returns(new StringValues(source));
        mockRequest.Setup(r => r.Query).Returns(mockQuery.Object);

        // Setup headers
        mockHeaders.Setup(h => h["X-Source"]).Returns(new StringValues(source));
        mockRequest.Setup(r => r.Headers).Returns(mockHeaders.Object);

        return mockRequest.Object;
    }
}
