using System.Linq.Expressions;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Enumerators;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Order;

public class AddOrderCommandHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly AddOrderCommandHandler _handler;

    public AddOrderCommandHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _orderItemRepository = Substitute.For<IOrderItemRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();

        _handler = new AddOrderCommandHandler(
            _orderRepository,
            _unitOfWork,
            _mediator,
            _orderItemRepository,
            _customerRepository
        );
    }

    [Fact]
    public async Task Should_NotifyError_When_CustomerNotFound()
    {
        // Arrange
        var command = new AddOrderCommand
        {
            CustomerId = Guid.NewGuid(),
            Total = 100,
            OrderItems = new List<OrderItemCommand>()
        };

        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n =>
            n.Value == "O código do cliente informado não foi encontrado"));
    }

    [Fact]
    public async Task Should_AddOrderAndItems_When_ValidRequest()
    {
        // Arrange
        var command = new AddOrderCommand
        {
            CustomerId = Guid.NewGuid(),
            Total = 200,
            OrderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 2 },
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(true);
        _orderRepository.CountAllAsync().Returns(10);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _orderRepository.Received(1).InsertAsync(Arg.Is<Entities.Order>(o =>
            o.CustomerId == command.CustomerId &&
            o.Number == 11 &&
            o.Total == 200 &&
            o.OrderSituationEnum == OrderSituationEnum.Received));
        
        await _orderItemRepository.Received(1).InsertRangeAsync(Arg.Is<List<OrderItem>>(items =>
            items.Count == 2 &&
            items[0].ProductId == command.OrderItems[0].ProductId &&
            items[0].Quantity == 2 &&
            items[1].ProductId == command.OrderItems[1].ProductId &&
            items[1].Quantity == 1));
        
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Should_NotifyError_When_CommitFails()
    {
        // Arrange
        var command = new AddOrderCommand
        {
            CustomerId = Guid.NewGuid(),
            Total = 150,
            OrderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 3 }
            }
        };

        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(true);
        _orderRepository.CountAllAsync().Returns(5);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n =>
            n.Value == Values.Message.DefaultError));
    }

    [Fact]
    public async Task Should_HandleRequest_When_CustomerIdIsNull()
    {
        // Arrange
        var command = new AddOrderCommand
        {
            CustomerId = null,
            Total = 300,
            OrderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        _orderRepository.CountAllAsync().Returns(20);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _orderRepository.Received(1).InsertAsync(Arg.Is<Entities.Order>(o =>
            o.CustomerId == null &&
            o.Number == 21 &&
            o.Total == 300 &&
            o.OrderSituationEnum == OrderSituationEnum.Received));
        await _unitOfWork.Received(1).CommitAsync();
    }
}
