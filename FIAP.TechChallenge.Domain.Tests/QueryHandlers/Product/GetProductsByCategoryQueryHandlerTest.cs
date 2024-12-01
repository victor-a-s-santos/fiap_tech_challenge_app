using System.Linq.Expressions;
using AutoMapper;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Enumerators;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Product;

public class GetProductsByCategoryQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetProductsByCategoryQueryHandler _handler;

    public GetProductsByCategoryQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetProductsByCategoryQueryHandler(_productRepository, _mapper, _mediator);
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnMappedProductsByCategory_WhenProductsExist()
    {
        // Arrange
        var query = new GetProductsByCategoryQuery { Category = "Foods" };
        
        var products = new List<Entities.Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Category = "Foods", Price = 100, Description = "Product 1", ProductCategoryEnum = ProductCategoryEnum.Accompaniment },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Category = "Foods", Price = 100, Description = "Product 2", ProductCategoryEnum = ProductCategoryEnum.Sandwich }, 
        };
        
        var expectedResult = new List<ProductDto>
        {
            new() { Id = products[0].Id, Name = "Product 1", Category = "Foods", Price = 100, Description = "Product 1" },
            new() { Id = products[1].Id, Name = "Product 2", Category = "Foods", Price = 100, Description = "Product 2" }, 
        };

        _productRepository
            .ListByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .Returns(products);
        _mapper.Map<List<ProductDto>>(Arg.Any<List<Entities.Product>>()).Returns(expectedResult);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _productRepository.Received(1)
            .ListByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>());
        _mapper.Received(1).Map<List<ProductDto>>(Arg.Any<List<Entities.Product>>());
    }

    [Fact]
    public async Task AfterValidation_ShouldReturnNull_WhenNoProductsExist()
    {
        // Arrange
        var query = new GetProductsByCategoryQuery { Category = "NonExistentCategory" };
        _productRepository
            .ListByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>())
            .ReturnsNull();

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().BeNull();
        await _productRepository.Received(1)
            .ListByExpressionAsync(Arg.Any<Expression<Func<Entities.Product, bool>>>());
        _mapper.Received(1).Map<List<ProductDto>>(Arg.Any<List<Entities.Product>>());
    }
}

