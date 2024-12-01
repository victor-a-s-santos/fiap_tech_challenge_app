using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Order;

public class GetOrdersQueryHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMediatorHandler _mediator;
    private readonly GetOrdersQueryHandler _handler;

    public GetOrdersQueryHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetOrdersQueryHandler(_orderRepository, _mediator);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnListOfOrders_WhenOrdersExist()
    {
        // Arrange
        var query = new GetOrdersQuery();
        var orders = new List<OrderDto>
        {
            new() { Id = Guid.NewGuid(), CustomerName = "Jonh B", Total = 100 },
            new() { Id = Guid.NewGuid(), CustomerName = "Michel", Total = 200 }
        };

        _orderRepository.GetOrders().Returns(orders);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeEquivalentTo(orders);

        await _orderRepository.Received(1).GetOrders();
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnEmptyList_WhenNoOrdersExist()
    {
        // Arrange
        var query = new GetOrdersQuery();
        var orders = new List<OrderDto>();

        _orderRepository.GetOrders().Returns(orders);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull()
            .And.BeEmpty();

        await _orderRepository.Received(1).GetOrders();
    }

    [Fact]
    public async Task AfterValidation_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        var query = new GetOrdersQuery();
        _orderRepository.GetOrders().Throws(new Exception("Database error"));

        // Act
        var act = async () => await _handler.AfterValidation(query);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database error");

        await _orderRepository.Received(1).GetOrders();
    }
}
