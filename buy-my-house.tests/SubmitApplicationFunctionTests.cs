using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;
using BuyMyHouse.Functions;
using BuyMyHouse.Models;

public class SubmitApplicationFunctionTests
{
    private readonly Mock<ILogger<SubmitApplicationFunction>> _mockLogger;
    private readonly Mock<TableClient> _mockTableClient;
    private readonly SubmitApplicationFunction _function;

    public SubmitApplicationFunctionTests()
    {
        _mockLogger = new Mock<ILogger<SubmitApplicationFunction>>();
        _mockTableClient = new Mock<TableClient>();
        _function = new SubmitApplicationFunction(_mockLogger.Object);
    }

    private DefaultHttpContext CreateHttpContext(string requestBody)
    {
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        context.Request.ContentLength = requestBody.Length;
        return context;
    }

    [Fact]
    public async Task Run_ShouldReturnBadRequest_WhenApplicationDataIsInvalid()
    {
        // Arrange
        var invalidRequestBody = JsonConvert.SerializeObject(new
        {
            CustomerID = "", // Missing or invalid fields
            HouseID = 0,
            Income = -1,
            CreditScore = 0,
            CustomerEmail = "invalid-email"
        });
        var httpContext = CreateHttpContext(invalidRequestBody);

        // Act
        var result = await _function.Run(httpContext.Request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Contains("Invalid application data.", badRequestResult.Value.ToString());
    }

    // [Fact]
    // public async Task Run_ShouldReturnInternalServerError_WhenStorageConnectionIsMissing()
    // {
    //     // Arrange
    //     var validRequestBody = JsonConvert.SerializeObject(new
    //     {
    //         CustomerID = "12345",
    //         HouseID = 1,
    //         Income = 50000,
    //         CreditScore = 750,
    //         CustomerEmail = "customer@example.com"
    //     });
    //     var httpContext = CreateHttpContext(validRequestBody);

    //     // Clear AzureWebJobsStorage environment variable
    //     Environment.SetEnvironmentVariable("AzureWebJobsStorage", null);

    //     // Act
    //     var result = await _function.Run(httpContext.Request);

    //     // Assert
    //     var serverErrorResult = Assert.IsType<StatusCodeResult>(result);
    //     Assert.Equal(500, serverErrorResult.StatusCode);
    //     _mockLogger.Verify(log => log.LogError(It.Is<string>(msg => msg.Contains("AzureWebJobsStorage environment variable is not set."))));
    // }

    // [Fact]
    // public async Task Run_ShouldReturnOk_WhenApplicationIsValid()
    // {
    //     // Arrange
    //     var validRequestBody = JsonConvert.SerializeObject(new
    //     {
    //         CustomerID = "12345",
    //         HouseID = 1,
    //         Income = 50000,
    //         CreditScore = 750,
    //         CustomerEmail = "customer@example.com"
    //     });
    //     var httpContext = CreateHttpContext(validRequestBody);

    //     Environment.SetEnvironmentVariable("AzureWebJobsStorage", "UseDevelopmentStorage=true");

    //     // Mock TableClient creation and behavior
    //     _mockTableClient
    //         .Setup(tc => tc.CreateIfNotExistsAsync(It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(Mock.Of<Azure.Response<Azure.Data.Tables.Models.TableItem>>());
    //     _mockTableClient
    //         .Setup(tc => tc.AddEntityAsync(It.IsAny<TableEntity>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(Mock.Of<Azure.Response>());

    //     // Act
    //     var result = await _function.Run(httpContext.Request);

    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     Assert.Equal(200, okResult.StatusCode);
    //     Assert.Contains("Application submitted successfully.", okResult.Value.ToString());
    // }
}
