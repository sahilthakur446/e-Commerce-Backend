using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;

        public UsersController(IAccountRepository _accountRepo)
        {
            accountRepo = _accountRepo;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO user) 
        {
            if (!await accountRepo.CheckIfEmailExistsAsync(user))
            {
                return NotFound(new
                {
                    Message = "Email not Found"
                });
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) )
            {
                return BadRequest(new
                {
                    Message = "Invalid Input"
                });
            }

            if (await accountRepo.Login(user))
            {
                return Ok(new {
                    message = "Login Success",
                    jwtToken = accountRepo.JWTToken });
            }
            return BadRequest(new
            {
                Message = "Wrong Password"
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO user)
        {
            if (await accountRepo.CheckIfEmailExistsAsync(user))
            {
                return BadRequest(new
                {
                    Message = "Already User Exist with Same email"
                });
            }
            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest(new
                { 
                    Message = "Password and Confirm Password are not same" 
                });
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) 
                || string.IsNullOrEmpty(user.ConfirmPassword) || string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
            {
                return BadRequest(new
                    { Message = "Invalid Input" });
            }

            if (await accountRepo.Register(user))
            {
                return Ok(new
                {
                    Message = "Signup Success",
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

        }

        [Authorize]
        [HttpGet("GetUserInfo/{userId}")]
        public async Task<IActionResult> GetUserInfo(int? userId)
        {
            if (userId is null)
            {
                return BadRequest(new
                {
                    Message = "User id can not be null"
                });
            }
            var user = await accountRepo.GetUserInfo((int)userId);
            if (user is null) 
            {
                return NotFound(new
                {
                    Message = "No user found"
                });
            }
            return Ok(user);
        }

        [Authorize]
        [HttpPut("UpdateUserInfo/{userId}")]
        public async Task<IActionResult> UpdateUserInfo(int? userId, [FromBody] UpdateUserDTO updatedUser)
        {
            try
            {
                if (userId is null)
            {
                return BadRequest(new
                {
                    Message = "User id can not be null"
                });
            }
            var user = await accountRepo.EditUserInformation((int)userId,updatedUser);
            if (!user)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,"Internal Server Error");
            }
            return Ok(new 
            {
                Message = "User updated Successfully"
            });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int? userId)
            {
            try
                {
                if (userId == null)
                    {
                    return BadRequest(new { Message = "User id cannot be null" });
                    }

                var result = await accountRepo.DeleteUser(userId.Value);
                if (result)
                    {
                    return Ok(new { Message = "Account Deleted Successfully" });
                    }
                else
                    {
                    return StatusCode((int) HttpStatusCode.InternalServerError, "Failed to Delete User");
                    }
                }
            catch (Exception ex)
                {
                return StatusCode((int) HttpStatusCode.InternalServerError, "Failed to Delete User");
                }
            }

        [Authorize]
        [HttpPut("ChangePassword/{userId}")]
        public async Task<IActionResult> ChangePassword(int? userId, [FromBody] ChangePasswordDTO password)
        {
            try
            {
                if (userId is null)
                {
                    return BadRequest(new
                    {
                        Message = "User id can not be null"
                    });
                }
                if (string.IsNullOrEmpty(password.OldPassword) || string.IsNullOrEmpty(password.NewPassword))
                {
                    return BadRequest(new
                    {
                        Message = "Please provide both old and new password"
                    });
                }
                var result = await accountRepo.ChangeUserPassword((int)userId, password);
                if (!result)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to change password");
                }
                return Ok(new
                {
                    Message = "Password updated successfully"
                });
            }
            catch (Exception ex) 
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
