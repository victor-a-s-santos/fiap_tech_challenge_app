using System.Linq.Expressions;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Events.Notifications;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Product;

public class AddProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly AddProductCommandHandler _handler;

    public AddProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();

        _handler = new AddProductCommandHandler(_productRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenProductAlreadyExists()
    {
        // Arrange
        var command = new AddProductCommand
        {
            Name = "Product A",
            Category = "Category 1",
            Description = "Test description",
            Price = 100.0M
        };

        _productRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .Returns(new Entities.Product());

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received()
            .PublishEvent(Arg.Is<DomainNotification>(x => x.Value == "O produto informado já existe"));
        await _productRepository.DidNotReceive().InsertAsync(Arg.Any<Entities.Product>());
    }

    [Fact]
    public async Task Handle_ShouldInsertProduct_WhenProductDoesNotExist()
    {
        // Arrange
        var command = new AddProductCommand
        {
            Name = "Product B",
            Category = "Category 2",
            Description = "New description",
            Price = 150.0M
        };

        _productRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .ReturnsNull();

        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _productRepository.Received(1).InsertAsync(Arg.Any<Entities.Product>());
        await _unitOfWork.Received(1).CommitAsync();
        await _mediator.DidNotReceive().PublishEvent(Arg.Any<DomainNotification>());
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenCommitFails()
    {
        // Arrange
        var command = new AddProductCommand
        {
            Name = "Product C",
            Category = "Category 3",
            Description = "Another description",
            Price = 200.0M
        };

        _productRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .ReturnsNull();

        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1)
            .PublishEvent(Arg.Is<DomainNotification>(x => x.Value == Values.Message.DefaultError));
    }
}