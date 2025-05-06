using Microsoft.AspNetCore.Mvc;
using BookStore.Models.DTO.User;
using Business.Interface;
using Repository.DTO;
using Repository.Helper.CustomExceptions;
using RepositoryLayer.DTO;
using NLog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public UserController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequestDto request)
        {
            try
            {
                var response = await _userBL.RegisterUserAsync(request);
                if (response.success)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while registering user");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "Internal server error" });
            }
        }

        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string email)
        {
            try
            {
                var response = await _userBL.DeleteUserAsync(email);
                if (response.success)
                    return Ok(response);
                return NotFound(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred while deleting user with email: {email}");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "Internal server error" });
            }
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var response = await _userBL.GetAllUsersAsync();
                if (response.success)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while retrieving all users");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLoginAsync([FromBody] LoginDto request)
        {
            try
            {
                var result = await _userBL.UserLoginAsync(request);
                if (result.success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (InvalidCredentialsException ex)
            {
                logger.Warn(ex, "Invalid login attempt for email: {0}", request.email);
                return Unauthorized(new ResponseDto<string> { success = false, message = "Invalid email or password" });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                logger.Error(ex, "Unexpected error during login");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "An error occurred during login" });
            }
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                logger.Info($"ForgetPassword endpoint called with email: {email}");
                var response = await _userBL.ForgetPasswordAsync(email);
                if (response.success)
                    return Ok(response);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred while processing forget password for email: {email}");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "Internal server error" });
            }
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized(new ResponseDto<string> { success = false, message = "Invalid token or email not found in token." });

                var result = await _userBL.ResetPasswordAsync(dto, email);
                return Ok(result);
            }
            catch (PasswordMismatchException ex)
            {
                return BadRequest(new ResponseDto<string> { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in ResetPassword controller");
                return StatusCode(500, new ResponseDto<string> { success = false, message = "Internal Server Error" });
            }
        }

    }
}

