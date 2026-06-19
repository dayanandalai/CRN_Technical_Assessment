using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Entities;
using CRN_Technical_Assessment.Infrastructure.Data;
using CRN_Technical_Assessment.Infrastructure.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Infrastructure.Tests;

/// <summary>
/// Unit tests for Infrastructure layer – GenericRepository using EF Core In-Memory database.
/// Each test gets a fresh, isolated database to avoid state leakage.
/// </summary>
public class InfrastructureTests
{
    // ──────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────

    /// <summary>Creates a new in-memory DbContext with a unique name per test.</summary>
    private static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new ApplicationDbContext(options);
    }

    /// <summary>Builds a GenericRepository with a mocked ICurrentUserService.</summary>
    private static GenericRepository<Categories> CreateCategoryRepo(ApplicationDbContext ctx)
    {
        var userService = new Mock<ICurrentUserService>();
        userService.Setup(u => u.UserName).Returns("test-user");
        return new GenericRepository<Categories>(ctx, userService.Object);
    }

    private static GenericRepository<Products> CreateProductRepo(ApplicationDbContext ctx)
    {
        var userService = new Mock<ICurrentUserService>();
        userService.Setup(u => u.UserName).Returns("test-user");
        return new GenericRepository<Products>(ctx, userService.Object);
    }

    // ──────────────────────────────────────────────
    // Categories – GetAllAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_Categories_ReturnsOnlyNonDeleted()
    {
        // Arrange
        using var ctx = CreateContext("cat-getall");
        ctx.Categories.AddRange(
            new Categories { Id = 1, Name = "Active",  IsDeleted = false },
            new Categories { Id = 2, Name = "Deleted", IsDeleted = true  }
        );
        await ctx.SaveChangesAsync();
        var repo = CreateCategoryRepo(ctx);

        // Act
        var result = await repo.GetAllAsync();

        // Assert
        result.Should().ContainSingle();
        result.Should().Contain(c => c.Name == "Active");
        result.Should().NotContain(c => c.IsDeleted);
    }

    // ──────────────────────────────────────────────
    // Categories – GetByIdAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_Categories_ReturnsEntity_WhenExists()
    {
        // Arrange
        using var ctx = CreateContext("cat-getbyid-found");
        ctx.Categories.Add(new Categories { Id = 10, Name = "Electronics", IsDeleted = false });
        await ctx.SaveChangesAsync();
        var repo = CreateCategoryRepo(ctx);

        // Act
        var result = await repo.GetByIdAsync(10);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetByIdAsync_Categories_ReturnsNull_WhenDeleted()
    {
        // Arrange
        using var ctx = CreateContext("cat-getbyid-deleted");
        ctx.Categories.Add(new Categories { Id = 11, Name = "Removed", IsDeleted = true });
        await ctx.SaveChangesAsync();
        var repo = CreateCategoryRepo(ctx);

        // Act
        var result = await repo.GetByIdAsync(11);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Categories_ReturnsNull_WhenIdMissing()
    {
        // Arrange
        using var ctx = CreateContext("cat-getbyid-missing");
        var repo = CreateCategoryRepo(ctx);

        // Act
        var result = await repo.GetByIdAsync(9999);

        // Assert
        result.Should().BeNull();
    }

    // ──────────────────────────────────────────────
    // Categories – AddAsync / SaveChanges
    // ──────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_Categories_PersistsEntity()
    {
        // Arrange
        using var ctx = CreateContext("cat-add");
        var repo = CreateCategoryRepo(ctx);
        var category = new Categories { Id = 20, Name = "New Category", IsDeleted = false };

        // Act
        await repo.AddAsync(category);
        await ctx.SaveChangesAsync();

        // Assert
        var stored = await ctx.Categories.FindAsync(20);
        stored.Should().NotBeNull();
        stored!.Name.Should().Be("New Category");
    }

    // ──────────────────────────────────────────────
    // Categories – Delete (soft-delete)
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Delete_Categories_SoftDeletesEntity()
    {
        // Arrange
        using var ctx = CreateContext("cat-delete");
        var category = new Categories { Id = 30, Name = "To Be Deleted", IsDeleted = false };
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();

        var repo = CreateCategoryRepo(ctx);

        // Act
        repo.Delete(category);
        await ctx.SaveChangesAsync();

        // Assert – must reload to bypass AsNoTracking cache
        var stored = await ctx.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == 30);
        stored.Should().NotBeNull();
        stored!.IsDeleted.Should().BeTrue();
        stored.DeletedBy.Should().Be("test-user");
    }

    // ──────────────────────────────────────────────
    // Categories – ExistsAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenEntityExists()
    {
        // Arrange
        using var ctx = CreateContext("cat-exists-true");
        ctx.Categories.Add(new Categories { Id = 40, Name = "Existing", IsDeleted = false });
        await ctx.SaveChangesAsync();
        var repo = CreateCategoryRepo(ctx);

        // Act
        var exists = await repo.ExistsAsync(40);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenEntityDeleted()
    {
        // Arrange
        using var ctx = CreateContext("cat-exists-deleted");
        ctx.Categories.Add(new Categories { Id = 41, Name = "Deleted", IsDeleted = true });
        await ctx.SaveChangesAsync();
        var repo = CreateCategoryRepo(ctx);

        // Act
        var exists = await repo.ExistsAsync(41);

        // Assert
        exists.Should().BeFalse();
    }

    // ──────────────────────────────────────────────
    // Products – GetPagedAsync
    // ──────────────────────────────────────────────

    [Fact]
    public async Task GetPagedAsync_Products_ReturnsPaginatedResults()
    {
        // Arrange
        using var ctx = CreateContext("prod-paged");

        // Need a Category first (FK constraint)
        ctx.Categories.Add(new Categories { Id = 1, Name = "Cat", IsDeleted = false });
        ctx.Products.AddRange(Enumerable.Range(1, 15).Select(i => new Products
        {
            Id = i,
            Name = $"Product {i}",
            IsDeleted = false,
            CategoryId = 1,
            Price = i * 10m
        }));
        await ctx.SaveChangesAsync();
        var repo = CreateProductRepo(ctx);

        // Act – page 2, size 5 → items 6-10
        var page2 = await repo.GetPagedAsync(pageNumber: 2, pageSize: 5);

        // Assert
        page2.Should().HaveCount(5);
        page2.Select(p => p.Id).Should().BeEquivalentTo(new[] { 6, 7, 8, 9, 10 });
    }
}

