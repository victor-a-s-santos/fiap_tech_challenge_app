using System.Linq.Expressions;
using FIAP.Crosscutting.Domain.Events.Notifications;
using NSubstitute;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using FIAP.Crosscutting.Domain.MediatR;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Product;

public class RemoveProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly RemoveProductCommandHandler _handler;

    public RemoveProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new RemoveProductCommandHandler(_productRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task ShouldNotifyError_WhenProductNotFound()
    {
        // Arrange
        var command = new RemoveProductCommand { Id = Guid.NewGuid() };
        _productRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mediator.Received(1).PublishEvent(Arg.Any<DomainNotification>());
        _productRepository.DidNotReceive().DeleteByIdAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task ShouldRemoveProduct_WhenProductExists()
    {
        // Arrange
        var command = new RemoveProductCommand { Id = Guid.NewGuid() };
        _productRepository
            .ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>()).Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _productRepository.Received(1).DeleteByIdAsync(command.Id);
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task ShouldNotifyError_WhenCommitFails()
    {
        // Arrange
        var command = new RemoveProductCommand { Id = Guid.NewGuid() };
        _productRepository
            .ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>()).Returns(true);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mediator.Received(1).PublishEvent(Arg.Any<DomainNotification>());
        await _productRepository.Received(1).DeleteByIdAsync(command.Id);
    }
}

