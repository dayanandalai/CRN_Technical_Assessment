using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN_Technical_Assessment.api.Controllers
{
    /// <summary>
    /// Authentication API endpoints
    /// Manages user authentication and token operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register new user
        /// POST /api/auth/register
        /// </summary>
        /// <param name="registerUserDto">User registration data</param>
        /// <returns>Auth tokens and user information</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid registration data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                var result = await _authService.RegisterAsync(registerUserDto);
                if (!result.Success)
                    return BadRequest(new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = result.Message,
                        StatusCode = 400
                    });

                return CreatedAtAction(null, new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = result,
                    StatusCode = 201
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred during registration.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Login user
        /// POST /api/auth/login
        /// </summary>
        /// <param name="loginRequestDto">Login credentials</param>
        /// <returns>Auth tokens and user information</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid login data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                var result = await _authService.LoginAsync(loginRequestDto);
                if (!result.Success)
                    return Unauthorized(new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = result.Message,
                        StatusCode = 401
                    });

                return Ok(new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred during login.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Refresh JWT token
        /// POST /api/auth/refresh-token
        /// </summary>
        /// <param name="refreshTokenRequestDto">Refresh token</param>
        /// <returns>New auth tokens</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponseDto<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid refresh token data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                var result = await _authService.RefreshTokenAsync(refreshTokenRequestDto);
                if (!result.Success)
                    return Unauthorized(new ApiResponseDto<AuthResponseDto>
                    {
                        Success = false,
                        Message = result.Message,
                        StatusCode = 401
                    });

                return Ok(new ApiResponseDto<AuthResponseDto>
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Data = result,
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred during token refresh.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Logout user
        /// POST /api/auth/logout
        /// </summary>
        /// <returns>Logout result</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> Logout()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _authService.RevokeRefreshTokenAsync(refreshToken);
                }

                // Clear the refresh token cookie
                Response.Cookies.Delete("refreshToken");

                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "Logout successful",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred during logout.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Revoke refresh token
        /// POST /api/auth/revoke-token
        /// </summary>
        /// <param name="revokeTokenRequestDto">Token to revoke</param>
        /// <returns>Revoke result</returns>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<ActionResult<ApiResponseDto>> RevokeToken([FromBody] RefreshTokenRequestDto revokeTokenRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponseDto
                    {
                        Success = false,
                        Message = "Invalid token data",
                        StatusCode = 400,
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });

                await _authService.RevokeRefreshTokenAsync(revokeTokenRequestDto.RefreshToken);

                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = "Token revoked successfully",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token");
                return StatusCode(500, new ApiResponseDto
                {
                    Success = false,
                    Message = "An error occurred while revoking the token.",
                    StatusCode = 500,
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
