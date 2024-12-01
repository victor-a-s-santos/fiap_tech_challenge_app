using AutoMapper;
using FIAP.TechChallenge.Domain.DataTransferObjects;
using FIAP.TechChallenge.Domain.Interfaces.Repositories;
using FIAP.TechChallenge.Domain.Queries;
using FIAP.TechChallenge.Domain.QueryHandlers;
using FIAP.Crosscutting.Domain.MediatR;
using NSubstitute;
using FluentAssertions;
using NSubstitute.ReturnsExtensions;

namespace FIAP.TechChallenge.Domain.Tests.QueryHandlers.Product;

public class GetProductQueryHandlerTest
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IMediatorHandler _mediator;
    private readonly GetProductQueryHandler _handler;

    public GetProductQueryHandlerTest()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _mediator = Substitute.For<IMediatorHandler>();
        _handler = new GetProductQueryHandler(_productRepository, _mapper, _mediator);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        var query = new GetProductQuery { Id = Guid.NewGuid() };
        var product = new ProductDto { Id = query.Id, Name = "Product 1" };
        var productEntity = new Entities.Product(){ Id = query.Id, Name = "Product 1" }; 

        _productRepository.GetByIdAsync(query.Id).Returns(productEntity);
        _mapper.Map<ProductDto>(productEntity).Returns(product);

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().BeEquivalentTo(product);
        await _productRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<ProductDto>(productEntity);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var query = new GetProductQuery { Id = Guid.NewGuid() };

        _productRepository.GetByIdAsync(query.Id).ReturnsNull();

        // Act
        var result = await _handler.AfterValidation(query);

        // Assert
        result.Should().BeNull();
        await _productRepository.Received(1).GetByIdAsync(query.Id);
        _mapper.Received(1).Map<ProductDto>(Arg.Any<Entities.Product>());
    }
}
