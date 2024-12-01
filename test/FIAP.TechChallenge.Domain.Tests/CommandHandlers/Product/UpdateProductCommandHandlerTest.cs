using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Product;

public class UpdateProductCommandHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new UpdateProductCommandHandler(_productRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenProductNotFound()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "New Product",
            Description = "Updated Description",
            Price = 100.0m,
            Category = "Electronics"
        };

        _productRepository.GetByIdAsync(command.Id).ReturnsNull();

        // Act
        await _handler.AfterValidation(command);

        // Assert
        _mediator.Received().PublishEvent(Arg.Is<DomainNotification>(x => x.Value == "O produto informado não foi encontrado"));
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 200.0m,
            Category = "Electronics"
        };

        var existingProduct = new Entities.Product
        {
            Id = command.Id,
            Name = "Old Product",
            Description = "Old Description",
            Price = 100.0m,
            Category = "Electronics"
        };

        _productRepository.GetByIdAsync(command.Id).Returns(existingProduct);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _productRepository.Received(1).UpdateAsync(Arg.Any<Entities.Product>());
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Handle_ShouldNotifyError_WhenUpdateFails()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 200.0m,
            Category = "Electronics"
        };

        var existingProduct = new Entities.Product
        {
            Id = command.Id,
            Name = "Old Product",
            Description = "Old Description",
            Price = 100.0m,
            Category = "Electronics"
        };

        _productRepository.GetByIdAsync(command.Id).Returns(Task.FromResult(existingProduct));
        _unitOfWork.CommitAsync().Returns(Task.FromResult(false)); 

        // Act
        await _handler.AfterValidation(command);

        // Assert
        _mediator.Received().PublishEvent(Arg.Is<DomainNotification>(x => x.Value == Values.Message.DefaultError));
    }

    [Fact]
    public async Task Handle_ShouldNotNotifyError_WhenUpdateSucceeds()
    {
        // Arrange
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 200.0m,
            Category = "Electronics"
        };

        var existingProduct = new Entities.Product
        {
            Id = command.Id,
            Name = "Old Product",
            Description = "Old Description",
            Price = 100.0m,
            Category = "Electronics"
        };

        _productRepository.GetByIdAsync(command.Id).Returns(existingProduct);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        _mediator.DidNotReceive().PublishEvent(Arg.Is<DomainNotification>(x => x.Value == Values.Message.DefaultError));
    }
}
