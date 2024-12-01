using System.Linq.Expressions;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Helpers.Constants;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Customer;

public class UpdateCustomerCommandHandlerTest
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerCommandHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new UpdateCustomerCommandHandler(_customerRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task Should_NotifyError_When_CustomerNotFound()
    {
        // Arrange
        var command = new UpdateCustomerCommand { Id = Guid.NewGuid(), Document = "12345678901", Email = "test@example.com", Name = "John Doe" };
        _customerRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .ReturnsNull();

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n => 
            n.Value == "O cliente informado não foi encontrado"));
    }

    [Fact]
    public async Task Should_NotifyError_When_CustomerWithSameDocumentOrEmailExists()
    {
        // Arrange
        var existingCustomer = new Entities.Customer { Id = Guid.NewGuid(), Document = "12345678901", Email = "test1@example.com" };
        var command = new UpdateCustomerCommand { Id = existingCustomer.Id, Document = "12345678901", Email = "test@example.com", Name = "John Doe" };

        _customerRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(existingCustomer);
        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n => 
            n.Value == "Já existe um cliente com o CPF ou e-mail informado"));
    }

    [Fact]
    public async Task Should_UpdateCustomer_When_ValidationPasses()
    {
        // Arrange
        var existingCustomer = new Entities.Customer { Id = Guid.NewGuid(), Document = "12345678901", Email = "test@example.com" };
        var command = new UpdateCustomerCommand { Id = existingCustomer.Id, Document = "98765432109", Email = "updated@example.com", Name = "Jane Doe" };

        _customerRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(existingCustomer);
        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(false);
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _customerRepository.Received(1).UpdateAsync(Arg.Is<Entities.Customer>(c =>
            c.Name == "Jane Doe" &&
            c.Document == "98765432109" &&
            c.Email == "updated@example.com" &&
            c.UpdatedAt != null));
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Should_NotifyError_When_CommitFails()
    {
        // Arrange
        var existingCustomer = new Entities.Customer { Id = Guid.NewGuid(), Document = "12345678901", Email = "test@example.com" };
        var command = new UpdateCustomerCommand { Id = existingCustomer.Id, Document = "98765432109", Email = "updated@example.com", Name = "Jane Doe" };

        _customerRepository.GetFirstByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(existingCustomer);
        _customerRepository.ExistsByExpressionAsync(Arg.Any<Expression<Func<Entities.Customer, bool>>>())
            .Returns(false);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n => 
            n.Value == Values.Message.DefaultError));
    }
}
