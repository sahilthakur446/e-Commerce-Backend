﻿using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login([FromBody] LoginDTO user) 
        {
            if (!await accountRepo.CheckIfEmailExistsAsync(user))
            {
                return BadRequest("No email found");
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) )
            {
                return BadRequest("Invalid Input");
            }

            if (await accountRepo.Login(user))
            {
                return Ok(new {JWTtoken = accountRepo.JWTToken });
            }
            return BadRequest("Wrong Password");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            if (await accountRepo.CheckIfEmailExistsAsync(user))
            {
                return BadRequest("Already User Exist with Same email");
            }
            if (user.Password != user.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password are not same");
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) 
                || string.IsNullOrEmpty(user.ConfirmPassword) || string.IsNullOrEmpty(user.FirstName)
                || string.IsNullOrEmpty(user.LastName))
            {
                return BadRequest("Invalid Input");
            }

            if (await accountRepo.Register(user))
            {
                return CreatedAtAction("Register",user);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

        }
    }
}
