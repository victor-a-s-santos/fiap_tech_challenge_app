using System.Linq.Expressions;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers.Order;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Order;

public class RemoveOrderCommandHandlerTest
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly IOrderItemRepository _orderItemRepository = Substitute.For<IOrderItemRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IMediatorHandler _mediator = Substitute.For<IMediatorHandler>();
    private readonly RemoveOrderCommandHandler _handler;

    public RemoveOrderCommandHandlerTest()
    {
        _handler = new RemoveOrderCommandHandler(_orderRepository, _orderItemRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenOrderDoesNotExist()
    {
        // Arrange
        var command = new RemoveOrderCommand { Id = Guid.NewGuid() };
        _orderRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Order, bool>>>())
            .Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(
            d => d.Value == "O pedido informado não foi encontrado"));
    }

    [Fact]
    public async Task Handle_ShouldDeleteOrderAndItems_WhenOrderExists()
    {
        // Arrange
        var command = new RemoveOrderCommand { Id = Guid.NewGuid() };
        _orderRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Order, bool>>>())
            .Returns(true);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _orderItemRepository.Received(1).DeleteByExpressionAsync(Arg.Any<Expression<Func<Entities.OrderItem, bool>>>());
        await _orderRepository.Received(1).DeleteByIdAsync(command.Id);
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenCommitFails()
    {
        // Arrange
        var command = new RemoveOrderCommand { Id = Guid.NewGuid() };
        _orderRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Order, bool>>>())
            .Returns(true);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(
            d => d.Value == Values.Message.DefaultError));
    }
}
