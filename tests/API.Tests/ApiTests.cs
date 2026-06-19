using CRN_Technical_Assessment.api.Controllers;
using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace API.Tests;

/// <summary>
/// Unit tests for API controllers (ProductsController, CategoriesController).
/// Uses Moq to mock service dependencies so no database is needed.
/// </summary>
public class ApiTests
{
    // ──────────────────────────────────────────────
    // ProductsController tests
    // ──────────────────────────────────────────────

    private static ProductsController BuildProductsController(IProductService svc)
    {
        var logger = Mock.Of<ILogger<ProductsController>>();
        return new ProductsController(svc, logger);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOk_WithPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResponseDto<ProductDto>
        {
            Data = new List<ProductDto>
            {
                new() { Id = 1, Name = "Widget A", Price = 9.99m, StockQuantity = 50 },
                new() { Id = 2, Name = "Widget B", Price = 19.99m, StockQuantity = 20 }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1
        };

        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(pagedResult);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var actionResult = await controller.GetAllProducts(1, 10);

        // Assert
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponseDto<PagedResponseDto<ProductDto>>;
        response.Should().NotBeNull();
        response!.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.TotalRecords.Should().Be(2);
        response.Data.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetProductById_ReturnsOk_WhenProductExists()
    {
        // Arrange
        var product = new ProductDto { Id = 5, Name = "Gadget X", Price = 49.99m };

        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(product);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var actionResult = await controller.GetProductById(5);

        // Assert
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponseDto<ProductDto>;
        response.Should().NotBeNull();
        response!.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.Id.Should().Be(5);
        response.Data.Name.Should().Be("Gadget X");
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductMissing()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((ProductDto?)null);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var actionResult = await controller.GetProductById(99);

        // Assert
        actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
        var response = (actionResult.Result as NotFoundObjectResult)!.Value as ApiResponseDto<ProductDto>;
        response!.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedAt_WithNewId()
    {
        // Arrange
        var dto = new CreateProductDto { Name = "New Product", Price = 5.00m, CategoryId = 1, StockQuantity = 10 };

        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(42);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var actionResult = await controller.CreateProduct(dto);

        // Assert
        actionResult.Result.Should().BeOfType<CreatedAtActionResult>();
        var created = actionResult.Result as CreatedAtActionResult;
        created!.RouteValues!["id"].Should().Be(42);
        var response = created.Value as ApiResponseDto<int>;
        response!.Success.Should().BeTrue();
        response.Data.Should().Be(42);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOk_OnSuccess()
    {
        // Arrange
        var dto = new UpdateProductDto { Name = "Updated", Price = 15.00m };

        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.UpdateAsync(1, dto)).Returns(Task.CompletedTask);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var result = await controller.UpdateProduct(1, dto);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var response = okResult!.Value as ApiResponseDto;
        response!.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOk_OnSuccess()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        mockService.Setup(s => s.DeleteAsync(3)).Returns(Task.CompletedTask);

        var controller = BuildProductsController(mockService.Object);

        // Act
        var result = await controller.DeleteProduct(3);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var response = okResult!.Value as ApiResponseDto;
        response!.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
    }

    // ──────────────────────────────────────────────
    // CategoriesController tests
    // ──────────────────────────────────────────────

    private static CategoriesController BuildCategoriesController(ICategoryService svc)
    {
        var logger = Mock.Of<ILogger<CategoriesController>>();
        return new CategoriesController(svc, logger);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsOk_WithPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResponseDto<CategoryDto>
        {
            Data = new List<CategoryDto>
            {
                new() { Id = 1, Name = "Electronics" },
                new() { Id = 2, Name = "Clothing" }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1
        };

        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(pagedResult);

        var controller = BuildCategoriesController(mockService.Object);

        // Act
        var actionResult = await controller.GetAllCategories(1, 10);

        // Assert
        var ok = actionResult.Result as OkObjectResult;
        ok.Should().NotBeNull();
        var response = ok!.Value as ApiResponseDto<PagedResponseDto<CategoryDto>>;
        response.Should().NotBeNull();
        response!.Success.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data!.TotalRecords.Should().Be(2);
        response.Data.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsNotFound_WhenMissing()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((CategoryDto?)null);

        var controller = BuildCategoriesController(mockService.Object);

        // Act
        var actionResult = await controller.GetCategoryById(999);

        // Assert
        actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
        var response = (actionResult.Result as NotFoundObjectResult)!.Value as ApiResponseDto<CategoryDto>;
        response!.Success.Should().BeFalse();
        response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task CreateCategory_ReturnsCreatedAt_WithNewId()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Books" };

        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(7);

        var controller = BuildCategoriesController(mockService.Object);

        // Act
        var actionResult = await controller.CreateCategory(dto);

        // Assert
        actionResult.Result.Should().BeOfType<CreatedAtActionResult>();
        var created = actionResult.Result as CreatedAtActionResult;
        created!.RouteValues!["id"].Should().Be(7);
        var response = created.Value as ApiResponseDto<int>;
        response!.Success.Should().BeTrue();
        response.Data.Should().Be(7);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsOk_OnSuccess()
    {
        // Arrange
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.DeleteAsync(2)).Returns(Task.CompletedTask);

        var controller = BuildCategoriesController(mockService.Object);

        // Act
        var result = await controller.DeleteCategory(2);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var response = okResult!.Value as ApiResponseDto;
        response!.Success.Should().BeTrue();
        response.StatusCode.Should().Be(200);
    }
}

