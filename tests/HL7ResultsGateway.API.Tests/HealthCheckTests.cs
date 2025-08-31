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
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;

        // Verify the response contains expected properties
        var response = okResult.Value;
        Assert.NotNull(response);

        // Check that the response has the expected structure using reflection
        var responseType = response.GetType();
        var statusProperty = responseType.GetProperty("status");
        var timestampProperty = responseType.GetProperty("timestamp");
        var serviceProperty = responseType.GetProperty("service");
        var versionProperty = responseType.GetProperty("version");

        Assert.NotNull(statusProperty);
        Assert.NotNull(timestampProperty);
        Assert.NotNull(serviceProperty);
        Assert.NotNull(versionProperty);

        Assert.Equal("healthy", statusProperty!.GetValue(response));
        Assert.Equal("HL7 Results Gateway API", serviceProperty!.GetValue(response));
        Assert.Equal("1.0.0", versionProperty!.GetValue(response));
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
