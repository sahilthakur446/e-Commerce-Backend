using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static eCommerce.Utilities.GenderConverter;
using eCommerce.Utilities.Enums;
using eCommerce.Utilities;


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
                    RoleName = "User"
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
                    userRoleId = context.UserRoles.FirstOrDefault(role => role.RoleName == "Admin").RoleId;
                }
                else
                {
                    userRoleId = context.UserRoles.FirstOrDefault(role => role.RoleName == "User").RoleId;
                }
                using var hmac = new HMACSHA512();
                var newUser = new User
                {
                    FirstName = user.FirstName.ToLower(),
                    LastName = user.LastName.ToLower(),
                    Gender = GetUserGender(user.Gender),
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
        private async Task<bool> CheckIfEmailExistsAsync(string email)
        {
           
                bool ifEmailExist = await context.Users.AnyAsync(u => u.Email == email);
                if (ifEmailExist)
                {
                    return true;
               }
    
            return false;
        }
        public async Task<UpdateUserDTO> GetUserInfo(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            var userDto = new UpdateUserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Gender = GenderConverter.GetUserGenderString(user.Gender),
            };
            return userDto;
        }

        public async Task<bool> EditUserInformation(int userId, UpdateUserDTO updatedUser)
        {
            var user = await context.Users.FindAsync(userId);
            if (user is null)
            {
                throw new Exception("No user found for this id");
            }
            if (string.IsNullOrWhiteSpace(updatedUser.FirstName) &&
                string.IsNullOrWhiteSpace(updatedUser.LastName) &&
                string.IsNullOrWhiteSpace(updatedUser.Email) &&
                string.IsNullOrWhiteSpace(updatedUser.Gender))
            {
                throw new Exception("Invalid Input");
            }
            if (await CheckIfEmailExistsAsync(updatedUser.Email))
            {
                throw new Exception("Email already exists");
            }
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Email = updatedUser.Email ?? user.Email;
            if (!string.IsNullOrEmpty(updatedUser.Gender))
            {
                user.Gender = GenderConverter.GetUserGender(updatedUser.Gender);
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(int? userId)
            {
            var user = await context.Users.FindAsync(userId);
            if (user is null)
                {
                throw new Exception("No user found for this id");
                }
            try
                {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
                }
            catch
                {
                throw new Exception("Failed to delete your account");
                }
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

        public async Task<bool> ChangeUserPassword(int userID, ChangePasswordDTO password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userID);
            if (user is not null)
            {
                if (!VerifyPassword(password.OldPassword, user.Password))
                {
                    throw new Exception("Old password is not correct");
                }
                if (VerifyPassword(password.NewPassword,user.Password))
                {
                    throw new Exception("Old and new password can't be same");
                }
                user.Password = HashingPassword(password.NewPassword);
                await context.SaveChangesAsync();
                return true;
            }
            throw new Exception("User not found");
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
            string userId = user.UserId.ToString();
            var userClaims = new List<Claim>
            {
                new Claim("userid",userId),
                new Claim("name", $"{user.FirstName} {user.LastName}"),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, userRole)
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
