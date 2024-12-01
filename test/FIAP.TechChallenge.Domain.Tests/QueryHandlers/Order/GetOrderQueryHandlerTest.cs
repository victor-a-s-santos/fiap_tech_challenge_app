using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Order;

public class GetOrderQueryHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetOrderQueryHandler _handler;

    public GetOrderQueryHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetOrderQueryHandler(_mapper, _mediator, _orderRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedOrder_WhenOrderExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetOrderQuery { Id = id };
        var orderEntity = new Entities.Order { Id = id, Total = 100 };
        var orderDto = new OrderDto { Id = id, Total = 100 };

        _orderRepository.GetByIdAsync(query.Id).Returns(orderEntity);
        _mapper.Map<OrderDto>(orderEntity).Returns(orderDto);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderDto.Id);
        result.Total.Should().Be(orderDto.Total);

        await _orderRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<OrderDto>(orderEntity);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetOrderQuery { Id = id };

        _orderRepository.GetByIdAsync(query.Id).ReturnsNull();

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().BeNull();

        await _orderRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<OrderDto>(Arg.Any<Entities.Order>());
    }
}
