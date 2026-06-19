using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN_Technical_Assessment.api.Controllers
{
    /// <summary>
    /// Users API endpoints
    /// Manages user-related operations
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// GET /api/users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<UserDto>>>> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                return Ok(new ApiResponseDto<IEnumerable<UserDto>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving users.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get specific user by ID
        /// GET /api/users/{id}
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<UserDto>>> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
                if (result == null)
                    return NotFound(new ApiResponseDto<UserDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        StatusCode = 404
                    });

                return Ok(new ApiResponseDto<UserDto>
                {
                    Success = true,
                    Message = "User retrieved successfully",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Create new user
        /// POST /api/users
        /// </summary>
        /// <param name="createUserDto">User data to create</param>
        /// <returns>Created user ID</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<int>>> CreateUser([FromBody] RegisterUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid user data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                var result = await _userService.CreateAsync(createUserDto);
                return CreatedAtAction(nameof(GetUserById), new { id = result }, new ApiResponseDto<int>
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = result,
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while creating the user.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Update existing user
        /// PUT /api/users/{id}
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="updateUserDto">User data to update</param>
        /// <returns>Update result</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> UpdateUser(
            int id,
            [FromBody] RegisterUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid user data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                await _userService.UpdateProfileAsync(id, updateUserDto);
                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "User updated successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating the user.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Delete user
        /// DELETE /api/users/{id}
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Delete result</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "User deleted successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with id: {Id}", id);
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while deleting the user.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
