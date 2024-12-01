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

public class GetCustomerQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediatorHandler;
    private readonly GetCustomerQueryHandler _queryHandler;

    public GetCustomerQueryHandlerTest()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediatorHandler = Substitute.For<IMediatorHandler>();

        _queryHandler = new GetCustomerQueryHandler(_customerRepository, _mapper, _mediatorHandler);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnMappedCustomerDto_WhenCustomerExists()
    {
        // Arrange
        var query = new GetCustomerQuery { Id = Guid.NewGuid() };
        var customer = new Entities.Customer { Id = query.Id, Name = "John Doe" };
        var customerDto = new CustomerDto { Id = query.Id, Name = "John Doe" };

        _customerRepository.GetByIdAsync(query.Id).Returns(customer);
        _mapper.Map<CustomerDto>(customer).Returns(customerDto);

        // Act
        var result = await _queryHandler.AfterValidation(query);

        // Assert
        result.Should().BeEquivalentTo(customerDto);
        await _customerRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<CustomerDto>(customer);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnNull_WhenCustomerDoesNotExist()
    {
        // Arrange
        var query = new GetCustomerQuery { Id = Guid.NewGuid() };

        _customerRepository.GetByIdAsync(query.Id).ReturnsNull();

        // Act
        var result = await _queryHandler.AfterValidation(query);

        // Assert
        result.Should().BeNull();
        await _customerRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<CustomerDto>(default);
    }
}
