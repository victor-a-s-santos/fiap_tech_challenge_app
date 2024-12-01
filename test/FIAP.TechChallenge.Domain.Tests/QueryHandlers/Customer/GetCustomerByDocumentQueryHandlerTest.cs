using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Entities;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Customer;

public class GetCustomerByDocumentQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly GetCustomerByDocumentQueryHandler _handler;

    public GetCustomerByDocumentQueryHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediatorHandler = Substitute.For<IMediatorHandler>();
        _handler = new GetCustomerByDocumentQueryHandler(_customerRepository, _mapper, _mediatorHandler);
    }

    [Fact]
    public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
    {
        // Arrange
        var document = "12345678900";
        var query = new GetCustomerByDocumentQuery { Document = document };

        var customerEntity = new Entities.Customer
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Document = document,
            Email = "john.doe@example.com"
        };

        var customerDto = new CustomerDto
        {
            Id = customerEntity.Id,
            Name = customerEntity.Name,
            Document = customerEntity.Document,
            Email = customerEntity.Email
        };

        _customerRepository.GetFirstByExpressionAsync(x => x.Document == document).Returns(customerEntity);
        _mapper.Map<CustomerDto>(Arg.Any<CustomerDto>()).Returns(customerDto);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(customerDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var document = "12345678900";
        var query = new GetCustomerByDocumentQuery { Document = document };

        _customerRepository.GetFirstByExpressionAsync(x => x.Document == document).ReturnsNull();

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().BeNull();
    }
}
