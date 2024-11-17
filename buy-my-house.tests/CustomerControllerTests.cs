using System;
using System.Threading.Tasks;
using BuyMyHouse.Controllers;
using BuyMyHouse.DAL;
using BuyMyHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CustomerControllerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepository;
    private readonly CustomerController _controller;

    public CustomerControllerTests()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _controller = new CustomerController(_mockCustomerRepository.Object);
    }

    [Fact]
    public async Task UpdateFinancialInfo_ShouldReturnOk_WhenValidDataProvided()
    {
        // Arrange
        var customerId = 1;
        var model = new FinancialInformation
        {
            Income = 50000,
            CreditScore = 700
        };

        _mockCustomerRepository
            .Setup(repo => repo.UpdateCustomerFinancialInfoAsync(customerId, model.Income.Value, model.CreditScore.Value))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateFinancialInfo(customerId, model);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        _mockCustomerRepository.Verify(repo => repo.UpdateCustomerFinancialInfoAsync(customerId, model.Income.Value, model.CreditScore.Value), Times.Once);
    }

    [Fact]
    public async Task UpdateFinancialInfo_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var customerId = 1;
        var model = new FinancialInformation
        {
            Income = -1000, // Invalid Income
            CreditScore = 700
        };

        // Act
        var result = await _controller.UpdateFinancialInfo(customerId, model);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Contains("Invalid financial information provided.", badRequestResult.Value.ToString());

        _mockCustomerRepository.Verify(repo => repo.UpdateCustomerFinancialInfoAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateFinancialInfo_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = 1;
        var model = new FinancialInformation
        {
            Income = 50000,
            CreditScore = 700
        };

        _mockCustomerRepository
            .Setup(repo => repo.UpdateCustomerFinancialInfoAsync(customerId, model.Income.Value, model.CreditScore.Value))
            .Throws(new KeyNotFoundException("Customer not found"));

        // Act
        var result = await _controller.UpdateFinancialInfo(customerId, model);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Contains("Customer not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task UpdateFinancialInfo_ShouldReturnServerError_WhenExceptionOccurs()
    {
        // Arrange
        var customerId = 1;
        var model = new FinancialInformation
        {
            Income = 50000,
            CreditScore = 700
        };

        _mockCustomerRepository
            .Setup(repo => repo.UpdateCustomerFinancialInfoAsync(customerId, model.Income.Value, model.CreditScore.Value))
            .Throws(new Exception("Unexpected error"));

        // Act
        var result = await _controller.UpdateFinancialInfo(customerId, model);

        // Assert
        var serverErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, serverErrorResult.StatusCode);
        Assert.Contains("An error occurred while updating financial information", serverErrorResult.Value.ToString());
    }
}
