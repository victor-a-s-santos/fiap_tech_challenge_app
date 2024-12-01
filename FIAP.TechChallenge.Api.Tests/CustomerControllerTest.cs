using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Api.Controllers;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace FIAP.TechChallenge.Api.Tests;

public class CustomerControllerTest
{
    private readonly ICustomerServiceApp _customerServiceApp;
    private readonly IMediatorHandler _mediator;
    private readonly CustomerController _controller;
    private readonly DomainNotificationHandler _domainNotification;

    public CustomerControllerTest()
    {
        _customerServiceApp = Substitute.For<ICustomerServiceApp>();
        _domainNotification = Substitute.For<DomainNotificationHandler>();
        _mediator = Substitute.For<IMediatorHandler>();
        _mediator.GetNotificationHandler().Returns(_domainNotification);
        _controller = new CustomerController(_customerServiceApp, _mediator);
    }

    [Fact]
    public async Task GetPagedCustomers_ShouldReturnOk_WithPagedResult()
    {   
        // Arrange
        var pagedResult = new PagedResult<CustomerResponseViewModel>
        {
            Results = new[] { new CustomerResponseViewModel { Id = Guid.NewGuid(), Name = "John Doe" } },
            TotalRecords = 1
        };

        _customerServiceApp.GetPagedCustomers(1, 20, "created_at", true)
            .Returns(pagedResult);

        // Act
        var result = await _controller.Get(1, 20, "created_at", true);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(pagedResult);
        okResult.StatusCode.Should().Be(200);

        await _customerServiceApp.Received(1).GetPagedCustomers(1, 20, "created_at", true);
    }

    [Fact]
    public async Task GetCustomerById_ShouldReturnOk_WhenValidId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new CustomerResponseViewModel { Id = customerId, Name = "John Doe" };

        _customerServiceApp.GetCustomer(customerId.ToString()).Returns(customer);

        // Act
        var result = await _controller.Get(customerId.ToString());

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(customer);
        okResult.StatusCode.Should().Be(200);

        await _customerServiceApp.Received(1).GetCustomer(customerId.ToString());
    }

    [Fact]
    public async Task GetCustomerByDocument_ShouldReturnOk_WhenValidDocument()
    {
        // Arrange
        var document = "12345678901";
        var customer = new CustomerResponseViewModel { Id = Guid.NewGuid(), Name = "John Doe" };

        _customerServiceApp.GetCustomerByDocument(document).Returns(customer);

        // Act
        var result = await _controller.GetByDocument(document);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(customer);
        okResult.StatusCode.Should().Be(200);

        await _customerServiceApp.Received(1).GetCustomerByDocument(document);
    }

    [Fact]
    public async Task Post_ShouldReturnNoContent_WhenValidCustomer()
    {
        // Arrange
        var customerViewModel = new CustomerRequestViewModel { Name = "John Doe", Document = "12345678901" };

        // Act
        var result = await _controller.Post(customerViewModel);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _customerServiceApp.Received(1).SaveCustomer(customerViewModel);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContent_WhenUpdatingCustomer()
    {
        // Arrange
        var customerViewModel = new CustomerRequestViewModel { Name = "John Doe", Document = "12345678901" };

        // Act
        var result = await _controller.Put(customerViewModel);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _customerServiceApp.Received(1).SaveCustomer(customerViewModel, true);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenValidCustomerId()
    {
        // Arrange
        var customerId = Guid.NewGuid().ToString();

        // Act
        var result = await _controller.Delete(customerId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _customerServiceApp.Received(1).RemoveCustomer(customerId);
    }
}
