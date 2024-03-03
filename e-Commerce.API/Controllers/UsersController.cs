using eCommerce.Data.Data;
using eCommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UsersController(ApplicationDbContext _context)
        {
            context = _context;
        }
        [HttpPost("Login")]
        public IActionResult Login() 
        {
            return Ok();
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDTO user)
        {
            var ifUserAlreadyExist = context.Users.FirstOrDefault(u => u.Email == user.Email);
            var ifUserTableEmpty = !context.Users.Any();
            var ifUserRolesTableEmpty = !context.UserRoles.Any();
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
                context.UserRoles.Add(adminRole);
                context.UserRoles.Add(normalUserRole);
                context.SaveChanges();
            }
            if (ifUserAlreadyExist == null && doesPasswordMatch)
            {
                int userRoleId;
                if (ifUserTableEmpty)
                {
                    userRoleId = context.UserRoles.FirstOrDefault(role => role.RoleName == "Admin").RoleId;
                }
                else
                {
                    userRoleId = context.UserRoles.FirstOrDefault(role => role.RoleName == "NormalUser").RoleId;
                }
                var newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    UserRoleId = userRoleId
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
