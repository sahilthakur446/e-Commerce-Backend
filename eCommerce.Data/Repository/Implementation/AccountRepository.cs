using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace eCommerce.Data.Repository.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;
        public string JWTToken { get; set; }
        public AccountRepository(ApplicationDbContext _context, IConfiguration _config)
        {
            context = _context;
            config = _config;
        }

        public async Task<bool> Login(LoginDTO user)
        {
            if (user != null)
            {
                if (await CheckIfEmailExistsAsync(user))
                {
                    var loggigInUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                    if (loggigInUser != null && VerifyPassword(user.Password, loggigInUser.Password))
                    {
                        return await GenerateJwtToken(loggigInUser) ? true : false;
                    }

                }
            }
            return false;
            
        }

        public async Task<bool> Register(RegisterDTO user)
        {
            var ifUserTableEmpty = !await context.Users.AnyAsync();
            var ifUserRolesTableEmpty = !await context.UserRoles.AnyAsync();
            bool doesPasswordMatch = user.Password == user.ConfirmPassword;
            if (ifUserRolesTableEmpty)
            {
                var adminRole = new UserRole
                {
                    RoleName = "Admin"
                };
                var normalUserRole = new UserRole
                {
                    RoleName = "NormalUser"
                };
                await context.UserRoles.AddAsync(adminRole);
                await context.UserRoles.AddAsync(normalUserRole);
                await context.SaveChangesAsync();
            }
            if (!await CheckIfEmailExistsAsync(user) && doesPasswordMatch)
            {
                int userRoleId;
                if (ifUserTableEmpty)
                {
                    userRoleId =  context.UserRoles.FirstOrDefault(role => role.RoleName == "Admin").RoleId;
                }
                else
                {
                    userRoleId = context.UserRoles.FirstOrDefault(role => role.RoleName == "NormalUser").RoleId;
                }
                using var hmac = new HMACSHA512();
                var newUser = new User
                {
                    FirstName = user.FirstName.ToLower(),
                    LastName = user.LastName.ToLower(),
                    Email = user.Email.ToLower(),
                    Password = HashingPassword(user.Password),
                    UserRoleId = userRoleId
                };
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfEmailExistsAsync(LoginDTO user)
        {
            if (user != null)
            {
                bool ifEmailExist = await context.Users.AnyAsync(u => u.Email == user.Email);
                if (ifEmailExist)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> CheckIfEmailExistsAsync(RegisterDTO user)
        {
            if (user != null)
            {
                bool ifEmailExist = await context.Users.AnyAsync(u => u.Email == user.Email);
                if (ifEmailExist)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<string> UserRoleFinder(int id)
        {
            var role = await context.UserRoles.FindAsync(id);
            if (role != null)
            {
                string roleName = role.RoleName;
                return roleName;
            }
            else
            {
                return "Not Found";
            }

        }
        private async Task<bool> GenerateJwtToken(User user) 
        {
            string userRole = await UserRoleFinder(user.UserRoleId);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>
            {
                new Claim("name", user.FirstName + " " + user.LastName),
                new Claim("email", user.Email),
                new Claim("userRole", userRole)
            };
            var tokenOptions = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            JWTToken = tokenString;
            return true;
        }
        private string HashingPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
