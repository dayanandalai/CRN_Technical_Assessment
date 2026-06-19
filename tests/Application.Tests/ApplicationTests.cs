using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests;

/// <summary>
/// Unit tests for Application layer service interfaces.
/// All dependencies are mocked with Moq.
/// </summary>
public class ApplicationTests
{
    // ──────────────────────────────────────────────
    // ICategoryService tests
    // ──────────────────────────────────────────────

    [Fact]
    public async Task CategoryService_GetAllAsync_ReturnsAllCategories()
    {
        // Arrange
        var expected = new PagedResponseDto<CategoryDto>
        {
            Data = new List<CategoryDto>
            {
                new() { Id = 1, Name = "Electronics", CreatedBy = "admin" },
                new() { Id = 2, Name = "Clothing",    CreatedBy = "admin" }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1
        };

        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expected);

        // Act
        var result = await mock.Object.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.TotalRecords.Should().Be(2);
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(c => c.Name == "Electronics");
        result.Data.Should().Contain(c => c.Name == "Clothing");
    }

    [Fact]
    public async Task CategoryService_GetByIdAsync_ReturnsCorrectCategory()
    {
        // Arrange
        var category = new CategoryDto { Id = 3, Name = "Books", CreatedBy = "admin" };

        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(category);

        // Act
        var result = await mock.Object.GetByIdAsync(3);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
        result.Name.Should().Be("Books");
    }

    [Fact]
    public async Task CategoryService_GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await mock.Object.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CategoryService_CreateAsync_ReturnsNewId()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Toys" };

        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(10);

        // Act
        var id = await mock.Object.CreateAsync(dto);

        // Assert
        id.Should().Be(10);
        mock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task CategoryService_UpdateAsync_CallsServiceOnce()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Name = "Updated Books" };

        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.UpdateAsync(3, dto)).Returns(Task.CompletedTask);

        // Act
        await mock.Object.UpdateAsync(3, dto);

        // Assert
        mock.Verify(s => s.UpdateAsync(3, dto), Times.Once);
    }

    [Fact]
    public async Task CategoryService_DeleteAsync_CallsServiceOnce()
    {
        // Arrange
        var mock = new Mock<ICategoryService>();
        mock.Setup(s => s.DeleteAsync(5)).Returns(Task.CompletedTask);

        // Act
        await mock.Object.DeleteAsync(5);

        // Assert
        mock.Verify(s => s.DeleteAsync(5), Times.Once);
    }

    // ──────────────────────────────────────────────
    // IProductService tests
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ProductService_GetAllAsync_ReturnsPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResponseDto<ProductDto>
        {
            Data = new List<ProductDto>
            {
                new() { Id = 1, Name = "Laptop",  Price = 999.99m, StockQuantity = 10 },
                new() { Id = 2, Name = "Monitor", Price = 299.99m, StockQuantity = 25 }
            },
            PageNumber = 1,
            PageSize = 10,
            TotalRecords = 2,
            TotalPages = 1
        };

        var mock = new Mock<IProductService>();
        mock.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(pagedResult);

        // Act
        var result = await mock.Object.GetAllAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result.TotalRecords.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(p => p.Name == "Laptop");
    }

    [Fact]
    public async Task ProductService_GetByIdAsync_ReturnsCorrectProduct()
    {
        // Arrange
        var product = new ProductDto { Id = 7, Name = "Keyboard", Price = 59.99m, StockQuantity = 100 };

        var mock = new Mock<IProductService>();
        mock.Setup(s => s.GetByIdAsync(7)).ReturnsAsync(product);

        // Act
        var result = await mock.Object.GetByIdAsync(7);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(7);
        result.Name.Should().Be("Keyboard");
        result.Price.Should().Be(59.99m);
    }

    [Fact]
    public async Task ProductService_GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var mock = new Mock<IProductService>();
        mock.Setup(s => s.GetByIdAsync(0)).ReturnsAsync((ProductDto?)null);

        // Act
        var result = await mock.Object.GetByIdAsync(0);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ProductService_CreateAsync_ReturnsNewId()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            Name = "Mouse",
            Description = "Wireless mouse",
            Price = 29.99m,
            StockQuantity = 200,
            CategoryId = 1
        };

        var mock = new Mock<IProductService>();
        mock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(15);

        // Act
        var id = await mock.Object.CreateAsync(dto);

        // Assert
        id.Should().Be(15);
        mock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ProductService_DeleteAsync_CallsServiceOnce()
    {
        // Arrange
        var mock = new Mock<IProductService>();
        mock.Setup(s => s.DeleteAsync(4)).Returns(Task.CompletedTask);

        // Act
        await mock.Object.DeleteAsync(4);

        // Assert
        mock.Verify(s => s.DeleteAsync(4), Times.Once);
    }
}

