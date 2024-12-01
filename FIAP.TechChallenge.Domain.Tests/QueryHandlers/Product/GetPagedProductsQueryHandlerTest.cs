using AutoMapper;
using FIAP.Crosscutting.Domain.Helpers.Pagination;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FluentAssertions;
using NSubstitute;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Product;

public class GetPagedProductsQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetPagedProductsQueryHandler _handler;

    public GetPagedProductsQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetPagedProductsQueryHandler(_productRepository, _mapper, _mediator);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnPagedResult_WhenProductsExist()
    {
        // Arrange
        var query = new GetPagedProductsQuery
        {
            Pagination = new PaginationObject() { Page = 1, Take = 10 }
        };

        var products = new PagedResult<Entities.Product>
        {
            Results = new List<Entities.Product>
            {
                new() { Id = Guid.NewGuid(), Name = "Product 1" },
                new() { Id = Guid.NewGuid(), Name = "Product 2" }
            },
            TotalRecords = 2,
            PageSize = 10,
            CurrentPage = 1,
            PageCount = 1
        };

        var expectedResult = new PagedResult<ProductDto>
        {
            Results = new List<ProductDto>
            {
                new() { Id = products.Results[0].Id, Name = "Product 1" },
                new() { Id = products.Results[1].Id, Name = "Product 2" }
            },
            TotalRecords = 2,
            PageSize = 10,
            CurrentPage = 1,
            PageCount = 1
        };

        _productRepository.PagedListAsync(query.Pagination).Returns(products);
        _mapper.Map<PagedResult<ProductDto>>(products).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).PagedListAsync(query.Pagination);
        _mapper.Received(1).Map<PagedResult<ProductDto>>(products);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnEmptyPagedResult_WhenNoProductsExist()
    {
        // Arrange
        var query = new GetPagedProductsQuery
        {
            Pagination = new PaginationObject() { Page = 1, Take = 10 }
        };

        var products = new PagedResult<Entities.Product>
        {
            Results = new List<Entities.Product>(),
            TotalRecords = 0,
            PageSize = 0,
            CurrentPage = 0,
            PageCount = 0
        };

        var expectedResult = new PagedResult<ProductDto>
        {
            Results = new List<ProductDto>(),
            TotalRecords = 0,
            PageSize = 0,
            CurrentPage = 0,
            PageCount = 0
        };

        _productRepository.PagedListAsync(query.Pagination).Returns(products);
        _mapper.Map<PagedResult<ProductDto>>(products).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1).PagedListAsync(query.Pagination);
        _mapper.Received(1).Map<PagedResult<ProductDto>>(products);
    }
}
