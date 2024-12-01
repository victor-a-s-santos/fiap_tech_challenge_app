using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.CommandHandlers;
using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Interfaces.UnitOfWork;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.CommandHandlers.Customer;

public class AddCustomerCommandHandlerTest
{

   private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediatorHandler _mediator;
    private readonly AddCustomerCommandHandler _handler;

    public AddCustomerCommandHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new AddCustomerCommandHandler(_customerRepository, _unitOfWork, _mediator);
    }

    [Fact]
    public async Task Should_NotifyError_When_CustomerAlreadyExists()
    {
        // Arrange
        var command = new AddCustomerCommand { Document = "12345678901", Email = "test@example.com", Name = "John Doe" };
        _customerRepository.ExistsByExpressionAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Entities.Customer, bool>>>())
            .Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n => 
            n.Value == "Já existe um cliente com o CPF ou e-mail informado"));
    }

    [Fact]
    public async Task Should_InsertCustomer_When_ValidationPasses()
    {
        // Arrange
        var command = new AddCustomerCommand { Document = "12345678901", Email = "test@example.com", Name = "John Doe" };
        _customerRepository.ExistsByExpressionAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Entities.Customer, bool>>>())
            .Returns(false);
        
        _unitOfWork.CommitAsync().Returns(true);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _customerRepository.Received(1).InsertAsync(Arg.Any<Entities.Customer>());
        await _unitOfWork.Received(1).CommitAsync();
    }

    [Fact]
    public async Task Should_NotifyError_When_CommitFails()
    {
        // Arrange
        var command = new AddCustomerCommand { Document = "12345678901", Email = "test@example.com", Name = "John Doe" };
        _customerRepository.ExistsByExpressionAsync(Arg.Any<System.Linq.Expressions.Expression<System.Func<Entities.Customer, bool>>>())
            .Returns(false);
        _unitOfWork.CommitAsync().Returns(false);

        // Act
        await _handler.AfterValidation(command);

        // Assert
        await _mediator.Received(1).PublishEvent(Arg.Is<DomainNotification>(n => 
            n.Value == Helpers.Constants.Values.Message.DefaultError));
    }
}