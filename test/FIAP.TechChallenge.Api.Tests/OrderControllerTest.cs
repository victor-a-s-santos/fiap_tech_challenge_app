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

public class OrderControllerTest
{
    private readonly IOrderServiceApp _orderServiceApp;
    private readonly IMediatorHandler _mediator;
    private readonly OrderController _controller;
    private readonly DomainNotificationHandler _domainNotification;

    public OrderControllerTest()
    {
        _orderServiceApp = Substitute.For<IOrderServiceApp>();
        _domainNotification = Substitute.For<DomainNotificationHandler>();
        _mediator = Substitute.For<IMediatorHandler>();
        _mediator.GetNotificationHandler().Returns(_domainNotification);
        _controller = new OrderController(_orderServiceApp, _mediator);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOkResultWithOrders_WhenOrdersExist()
    {
        // Arrange
        var orders = new List<OrderResponseViewModel>
        {
            new()
            {
                Id = Guid.NewGuid(), 
                CreatedAt = DateTime.Now,
                CustomerName = "John Doe",  
                Total = 100.50m, 
                Situation = "Completed",
                OrderItems = new List<OrderItemViewModel>
                {
                    new() { Quantity = 1, ProductId = Guid.NewGuid(), ProductName = "Sandwich"}
                }
            }
        };
        
        _orderServiceApp.GetOrders().Returns(orders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(orders);
        await _orderServiceApp.Received(1).GetOrders();
    }

    [Fact]
    public async Task Get_ShouldReturnOkResultWithOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new OrderResponseViewModel { Id = orderId, CustomerName = "John Doe", Total = 100.50m };
        _orderServiceApp.GetOrder(orderId.ToString()).Returns(order);

        // Act
        var result = await _controller.Get(orderId.ToString());

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(order);
        await _orderServiceApp.Received(1).GetOrder(orderId.ToString());
    }

    // [Fact]
    // public async Task GetByCustomerId_ShouldReturnPagedOrders_WhenValidCustomerIdIsProvided()
    // {
    //     // Arrange
    //     var customerId = "123";
    //     var pagedOrders = new PagedResult<OrderResponseViewModel>
    //     {
    //         Results = new List<OrderResponseViewModel>
    //         {
    //             new() { Id = Guid.NewGuid(), CustomerName = "John Doe", Total = 200.75m }
    //         },
    //         CurrentPage = 1,
    //         PageSize = 10,
    //         TotalRecords = 1
    //     };
    //     _orderServiceApp.GetPagedOrdersByCustomerId(customerId, 1, 10, "created_at", true).Returns(pagedOrders);
    //
    //     // Act
    //     var result = await _controller.GetByCustomerId(customerId);
    //
    //     // Assert
    //     var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
    //     okResult.Value.Should().BeEquivalentTo(pagedOrders);
    //     await _orderServiceApp.Received(1).GetPagedOrdersByCustomerId(customerId, 1, 10, "created_at", true);
    // }

    [Fact]
    public async Task Post_ShouldCallSaveOrder_WhenOrderRequestIsValid()
    {
        // Arrange
        var orderItems = new List<OrderItemRequestViewModel>
        {
            new() {Quantity = 1,ProductId = Guid.NewGuid()},
            new() {Quantity = 2,ProductId = Guid.NewGuid()},
        };
        
        var orderRequest = new OrderRequestViewModel { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Total = 200.75m, OrderItems = orderItems };

        // Act
        var result = await _controller.Post(orderRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _orderServiceApp.Received(1).SaveOrder(orderRequest);
    }

    [Fact]
    public async Task Put_ShouldCallSaveOrderWithUpdateFlag_WhenOrderRequestIsValid()
    {
        // Arrange
        var orderItems = new List<OrderItemRequestViewModel>
        {
            new() {Quantity = 1,ProductId = Guid.NewGuid()},
            new() {Quantity = 2,ProductId = Guid.NewGuid()},
        };
        
        var orderRequest = new OrderRequestViewModel { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Total = 250.00m, OrderItems = orderItems };

        // Act
        var result = await _controller.Put(orderRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _orderServiceApp.Received(1).SaveOrder(orderRequest, true);
    }

    [Fact]
    public async Task Delete_ShouldCallRemoveOrder_WhenValidOrderIdIsProvided()
    {
        // Arrange
        var orderId = Guid.NewGuid().ToString();

        // Act
        var result = await _controller.Delete(orderId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.StatusCode.Should().Be(200);
        
        await _orderServiceApp.Received(1).RemoveOrder(orderId);
    }
}