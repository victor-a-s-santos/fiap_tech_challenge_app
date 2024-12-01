using FIAP.TechChallenge.Domain.Commands;
using FIAP.TechChallenge.Domain.Validations;
using FluentValidation.TestHelper;

namespace FIAP.TechChallenge.Domain.Tests.Validations;

public class CustomerValidationTest
{
    private class TestCustomerCommand : CustomerCommand
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }

    private readonly CustomerValidation<TestCustomerCommand> _validator;

    public CustomerValidationTest()
    {
        _validator = new CustomerValidation<TestCustomerCommand>();
        _validator.ValidateCustomerId();
        _validator.ValidateCustomer();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new TestCustomerCommand { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("O código do cliente é obrigatório");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var command = new TestCustomerCommand { Name = null };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("O nome do cliente é obrigatório");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_Max_Length()
    {
        var command = new TestCustomerCommand { Name = new string('A', 151) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("O nome do cliente não pode conter mais de 150 caracteres");
    }

    [Fact]
    public void Should_Have_Error_When_Document_Is_Invalid()
    {
        var command = new TestCustomerCommand { Document = "123" }; // CPF inválido
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Document)
            .WithErrorMessage("O CPF informado é inválido");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = new TestCustomerCommand { Email = "invalid-email" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("O e-mail informado é inválido");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Command_Is_Valid()
    {
        var command = new TestCustomerCommand
        {
            Id = Guid.NewGuid(),
            Name = "Cliente Válido",
            Document = "12345678909", 
            Email = "cliente@valido.com"
        };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
