using FluentAssertions;

using HL7ResultsGateway.API;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Moq;

namespace HL7ResultsGateway.API.Tests;

public class HealthCheckTests
{
    private readonly Mock<ILogger<HealthCheck>> _mockLogger;
    private readonly HealthCheck _healthCheck;

    public HealthCheckTests()
    {
        _mockLogger = new Mock<ILogger<HealthCheck>>();
        _healthCheck = new HealthCheck(_mockLogger.Object);
    }

    [Fact]
    public void Run_ShouldReturnHealthyStatus()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequest>();

        // Act
        var result = _healthCheck.Run(mockRequest.Object);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result.As<OkObjectResult>();

        // Verify the response contains expected properties
        var response = okResult.Value;
        response.Should().NotBeNull();

        // Check that the response has the expected structure using reflection
        var responseType = response!.GetType();
        var statusProperty = responseType.GetProperty("status");
        var timestampProperty = responseType.GetProperty("timestamp");
        var serviceProperty = responseType.GetProperty("service");
        var versionProperty = responseType.GetProperty("version");

        statusProperty.Should().NotBeNull();
        timestampProperty.Should().NotBeNull();
        serviceProperty.Should().NotBeNull();
        versionProperty.Should().NotBeNull();

        statusProperty!.GetValue(response).Should().Be("healthy");
        serviceProperty!.GetValue(response).Should().Be("HL7 Results Gateway API");
        versionProperty!.GetValue(response).Should().Be("1.0.0");
    }

    [Fact]
    public void Run_ShouldLogInformation()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequest>();

        // Act
        _healthCheck.Run(mockRequest.Object);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Health check endpoint called")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
