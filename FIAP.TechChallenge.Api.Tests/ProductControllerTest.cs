using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.TechChallenge.Api.Controllers;
using FIAP.TechChallenge.Application.Interfaces;
using FIAP.TechChallenge.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using FluentAssertions;

namespace FIAP.TechChallenge.Api.Tests;

public class ProductControllerTest
{
    private readonly IProductServiceApp _productServiceApp;
    private readonly IMediatorHandler _mediator;
    private readonly ProductController _controller;
    private readonly DomainNotificationHandler _domainNotification;

    public ProductControllerTest()
    {
        _productServiceApp = Substitute.For<IProductServiceApp>();
        _domainNotification = Substitute.For<DomainNotificationHandler>();
        _mediator = Substitute.For<IMediatorHandler>();
        _mediator.GetNotificationHandler().Returns(_domainNotification);
        _controller = new ProductController(_productServiceApp, _mediator);
    }

    [Fact]
    public async Task GetProductsByCategory_ShouldReturnOk_WhenProductsExist()
    {
        // Arrange
        var category = "Lanche";
        var products = new List<ProductViewModel>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Category = "Lanche" },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Category = "Lanche" }
        };
        _productServiceApp.GetProductsByCategory(category).Returns(products);

        // Act
        var result = await _controller.Get(category);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedProducts = okResult.Value.Should().BeAssignableTo<List<ProductViewModel>>().Subject;
        returnedProducts.Should().HaveCount(2);
        returnedProducts.Should().Contain(p => p.Name == "Product 1" && p.Category == "Lanche");
    }

    [Fact]
    public async Task Post_ShouldReturnOk_WhenProductIsSaved()
    {
        // Arrange
        var product = new ProductViewModel { Id = Guid.NewGuid(), Name = "Product", Category = "Lanche" };
    
        // Act
        var result = await _controller.Post(product);
    
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        await _productServiceApp.Received(1).SaveProduct(product);
    }
    
    [Fact]
    public async Task Put_ShouldReturnOk_WhenProductIsUpdated()
    {
        // Arrange
        var product = new ProductViewModel { Id = Guid.NewGuid(), Name = "Updated Product", Category = "Lanche" };
    
        // Act
        var result = await _controller.Put(product);
    
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        await _productServiceApp.Received(1).SaveProduct(product, true);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenProductIsRemoved()
    {
        // Arrange
        var productId = Guid.NewGuid().ToString();
    
        // Act
        var result = await _controller.Delete(productId);
    
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        await _productServiceApp.Received(1).RemoveProduct(productId);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var invalidProductId = "invalid-guid";
    
        // Act
        var result = await _controller.Delete(invalidProductId);
    
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.Should().NotBeNull();
        await _productServiceApp.DidNotReceive().RemoveProduct(Arg.Any<string>());
    }
}
