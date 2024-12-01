using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FIAP.TechChallenge.Domain.Queries;
using FluentAssertions;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Order;


public class GetPagedOrdersQueryHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetPagedOrdersQueryHandler _handler;

    public GetPagedOrdersQueryHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetPagedOrdersQueryHandler(_orderRepository, _mapper, _mediator);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnPagedOrders_WhenRepositoryReturnsData()
    {
        // Arrange
        var query = new GetPagedOrdersQuery
        {
            Pagination = new PaginationObject() { Page = 1, Take = 10 }
        };

        var orders = new PagedResult<Entities.Order>
        {
            Results = new List<Entities.Order>
            {
                new() { Id = Guid.NewGuid(), CustomerId = query.CustomerId, Total = 100 },
                new() { Id = Guid.NewGuid(), CustomerId = query.CustomerId, Total = 200 }
            },
            TotalRecords = 2,
            PageSize = 10,
            CurrentPage = 1,
            PageCount = 1
        };
        
        var expectedResult = new PagedResult<OrderDto>
        {
            Results = new List<OrderDto>
            {
                new() { Id = Guid.NewGuid(), CustomerName = "Jonh B", Total = 100 },
                new() { Id = Guid.NewGuid(), CustomerName = "Michel", Total = 200 }
            },
            TotalRecords = 2,
            PageSize = 10,
            CurrentPage = 1,
            PageCount = 1
        };
        
        _orderRepository.PagedListAsync(query.Pagination).Returns(orders);
        _mapper.Map<PagedResult<OrderDto>>(orders).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _orderRepository.Received(1).PagedListAsync(query.Pagination);
        _mapper.Received(1).Map<PagedResult<OrderDto>>(orders);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnEmptyPagedOrders_WhenRepositoryReturnsNoData()
    {
        // Arrange
        var query = new GetPagedOrdersQuery
        {
            Pagination = new PaginationObject() { Page = 1, Take = 10 }
        };
        
        var emptyOrders = new PagedResult<Entities.Order>
        {
            Results = new List<Entities.Order>(),
            TotalRecords = 0,
            PageSize = 0,
            CurrentPage = 0,
            PageCount = 0
        };
        
        var expectedResult = new PagedResult<OrderDto>
        {
            Results = new List<OrderDto>(),
            TotalRecords = 0,
            PageSize = 0,
            CurrentPage = 0,
            PageCount = 0
        };

        _orderRepository.PagedListAsync(query.Pagination).Returns(emptyOrders);
        _mapper.Map<PagedResult<OrderDto>>(emptyOrders).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);

        await _orderRepository.Received(1).PagedListAsync(query.Pagination);
        _mapper.Received(1).Map<PagedResult<OrderDto>>(emptyOrders);
    }
}
