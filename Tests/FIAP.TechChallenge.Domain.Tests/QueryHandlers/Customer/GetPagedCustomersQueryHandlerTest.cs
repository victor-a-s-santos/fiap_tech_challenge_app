using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FluentAssertions;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers;

public class GetPagedCustomersQueryHandlerTest
{
    private readonly ICustomerRepository _customerRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly IMediatorHandler _mediatorMock;
    private readonly GetPagedCustomersQueryHandler _handler;

    public GetPagedCustomersQueryHandlerTest()
    {
        _customerRepositoryMock = Substitute.For<ICustomerRepository>();
        _mapperMock = Substitute.For<IMapper>();
        _mediatorMock = Substitute.For<IMediatorHandler>();

        _handler = new GetPagedCustomersQueryHandler(_customerRepositoryMock, _mapperMock, _mediatorMock);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnPagedResult_WhenCustomersExist()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            Pagination = new PaginationObject { Page = 1, Take = 10 }
        };

        var customers = new PagedResult<Entities.Customer>
        {
            Results = new List<Entities.Customer>
            {
                new() { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com" },
                new() { Id = Guid.NewGuid(), Name = "Jane Doe", Email = "jane@example.com" }
            },
            TotalRecords = 2,
            PageCount = 1
        };

        var expectedResult = new PagedResult<CustomerDto>
        {
            Results = new List<CustomerDto>
            {
                new() { Id = customers.Results[0].Id, Name = "John Doe", Email = "john@example.com" },
                new() { Id = customers.Results[1].Id, Name = "Jane Doe", Email = "jane@example.com" }
            },
            TotalRecords = 2,
            PageCount = 1
        };

        _customerRepositoryMock.PagedListAsync(query.Pagination).Returns(customers);
        _mapperMock.Map<PagedResult<CustomerDto>>(customers).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);

        await _customerRepositoryMock.Received(1).PagedListAsync(query.Pagination);
        _mapperMock.Received(1).Map<PagedResult<CustomerDto>>(customers);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnEmptyPagedResult_WhenNoCustomersExist()
    {
        // Arrange
        var query = new GetPagedCustomersQuery
        {
            Pagination = new PaginationObject { Page = 1, Take = 10 }
        };

        var emptyCustomers = new PagedResult<Domain.Entities.Customer>
        {
            Results = new List<Entities.Customer>(),
            TotalRecords = 0,
            PageCount = 0
        };

        var expectedResult = new PagedResult<CustomerDto>
        {
            Results = new List<CustomerDto>(),
            TotalRecords = 0,
            PageCount = 0
        };

        _customerRepositoryMock.PagedListAsync(query.Pagination).Returns(emptyCustomers);
        _mapperMock.Map<PagedResult<CustomerDto>>(emptyCustomers).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);

        await _customerRepositoryMock.Received(1).PagedListAsync(query.Pagination);
        _mapperMock.Received(1).Map<PagedResult<CustomerDto>>(emptyCustomers);
    }
}
