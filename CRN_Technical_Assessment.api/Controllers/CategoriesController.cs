using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CRN_Technical_Assessment.api.Controllers
{
    /// <summary>
    /// Categories API endpoints
    /// Manages category-related operations
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService,
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories with pagination
        /// GET /api/categories
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paginated list of categories</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<PagedResponseDto<CategoryDto>>>> GetAllCategories(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _categoryService.GetAllAsync(pageNumber, pageSize);
                return Ok(new ApiResponseDto<PagedResponseDto<CategoryDto>>
                {
                    Success = true,
                    Message = "Categories retrieved successfully",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving categories.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get specific category by ID
        /// GET /api/categories/{id}
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<CategoryDto>>> GetCategoryById(int id)
        {
            try
            {
                var result = await _categoryService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponseDto<CategoryDto>
                    {
                        Success = false,
                        Message = "Category not found.",
                        StatusCode = 404
                    });

                return Ok(new ApiResponseDto<CategoryDto>
                {
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving category with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving the category.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Create new category
        /// POST /api/categories
        /// </summary>
        /// <param name="createCategoryDto">Category data to create</param>
        /// <returns>Created category ID</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto<int>>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid category data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                var result = await _categoryService.CreateAsync(createCategoryDto);
                return CreatedAtAction(nameof(GetCategoryById), new { id = result }, new ApiResponseDto<int>
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = result,
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while creating the category.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Update existing category
        /// PUT /api/categories/{id}
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="updateCategoryDto">Category data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> UpdateCategory(
            int id,
            [FromBody] CreateCategoryDto updateCategoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid category data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                await _categoryService.UpdateAsync(id, new UpdateCategoryDto { Name = updateCategoryDto.Name });
                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "Category updated successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating the category.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Delete category
        /// DELETE /api/categories/{id}
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "Category deleted successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deleting the category.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get all products in a category with pagination
        /// GET /api/categories/{id}/products
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paginated list of products in the category</returns>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<ApiResponseDto<PagedResponseDto<ProductDto>>>> GetCategoryProducts(
            int id,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ApiResponseDto<PagedResponseDto<ProductDto>>
                    {
                        Success = false,
                        Message = "Category not found.",
                        StatusCode = 404
                    });

                // Note: Since CategoryService doesn't have a method to get products by category,
                // this endpoint structure is defined for future implementation when the service is updated
                return Ok(new ApiResponseDto<PagedResponseDto<ProductDto>>
                {
                    Success = true,
                    Message = "Products in category retrieved successfully",
                    Data = new PagedResponseDto<ProductDto>
                    {
                        Data = Enumerable.Empty<ProductDto>(),
                        TotalRecords = 0,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = 0
                    },
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products for category with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto<PagedResponseDto<ProductDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving products in the category.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
