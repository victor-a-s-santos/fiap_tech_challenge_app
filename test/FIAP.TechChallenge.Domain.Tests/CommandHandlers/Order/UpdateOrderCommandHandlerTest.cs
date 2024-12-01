using System.Linq.Expressions;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Order;

public class UpdateOrderCommandHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly UpdateOrderCommandHandler _handler;

    public UpdateOrderCommandHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderItemRepository = Substitute.For<IOrderItemRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new UpdateOrderCommandHandler(_orderRepository, _unitOfWork, _mediator, _orderItemRepository);
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenOrderNotFound()
    {
        // Arrange
        var command = new UpdateOrderCommand { Id = Guid.NewGuid() };
        _orderRepository.GetOrderItemsById(command.Id).ReturnsNull();

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1)
            .PublishEvent(Arg.Is<DomainNotification>(x => x.Value == "O pedido informado não foi encontrado"));
    }

    [Fact]
    public async Task Handle_ShouldUpdateOrderAndCommit_WhenValidRequest()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Total = 100,
            OrderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        var existingOrder = new Entities.Order
        {
            Id = command.Id,
            CustomerId = Guid.NewGuid(),
            Total = 50,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        _orderRepository.GetOrderItemsById(command.Id).Returns(existingOrder);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _orderRepository.Received(1).UpdateAsync(Arg.Is<Entities.Order>(x =>
            x.Id == command.Id &&
            x.CustomerId == command.CustomerId &&
            x.Total == command.Total));

        await _orderItemRepository.Received(1).DeleteByExpressionAsync(Arg.Any<Expression<Func<OrderItem, bool>>>());
        await _orderItemRepository.Received(1).InsertRangeAsync(Arg.Is<List<OrderItem>>(x =>
            x.Count == 1 &&
            x.First().ProductId == command.OrderItems.First().ProductId &&
            x.First().Quantity == command.OrderItems.First().Quantity));

        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenCommitFails()
    {
        // Arrange
        var command = new UpdateOrderCommand
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Total = 100,
            OrderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 2 }
            }
        };

        var existingOrder = new Entities.Order
        {
            Id = command.Id,
            CustomerId = Guid.NewGuid(),
            Total = 50,
            OrderItems = new List<OrderItem>()
        };

        _orderRepository.GetOrderItemsById(command.Id).Returns(existingOrder);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1)
            .PublishEvent(Arg.Is<DomainNotification>(x => x.Value == Values.Message.DefaultError));
    }
}
