using System.Linq.Expressions;
using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Order;

public class GetPagedOrdersByDocumentQueryHandlerTest
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetPagedOrdersByDocumentQueryHandler _handler;

    public GetPagedOrdersByDocumentQueryHandlerTest()
    {
        _orderRepository = Substitute.For<IOrderRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();

        _handler = new GetPagedOrdersByDocumentQueryHandler(_orderRepository, _mapper, _mediator);
    }

    [Fact]
    public async Task Handle_WhenOrdersExist_ReturnsPagedResult()
    {
        // Arrange
        var query = new GetPagedOrdersQuery
        {
            CustomerId = Guid.NewGuid(),
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

        _orderRepository.PagedListAsync(Arg.Any<PaginationObject>(), Arg.Any<Expression<Func<Entities.Order, bool>>>()).Returns(orders);
        _mapper.Map<PagedResult<OrderDto>>(orders).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        
        await _orderRepository.Received(1).PagedListAsync(Arg.Any<PaginationObject>(), Arg.Any<Expression<Func<Entities.Order, bool>>>());
        _mapper.Received(1).Map<PagedResult<OrderDto>>(orders);
    }

    [Fact]
    public async Task Handle_WhenNoOrdersExist_ReturnsEmptyPagedResult()
    {
        // Arrange
        var query = new GetPagedOrdersQuery
        {
            CustomerId = Guid.NewGuid(),
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
        
        _orderRepository.PagedListAsync(Arg.Any<PaginationObject>(), Arg.Any<Expression<Func<Entities.Order, bool>>>()).Returns(emptyOrders);
        _mapper.Map<PagedResult<OrderDto>>(emptyOrders).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        
        await _orderRepository.Received(1).PagedListAsync(Arg.Any<PaginationObject>(), Arg.Any<Expression<Func<Entities.Order, bool>>>());
        _mapper.Received(1).Map<PagedResult<OrderDto>>(emptyOrders);
    }
}

