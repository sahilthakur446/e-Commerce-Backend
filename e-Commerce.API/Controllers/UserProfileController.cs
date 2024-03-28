using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository userAddressRepo;

        public UserProfileController(IUserProfileRepository userAddressRepo)
        {
            this.userAddressRepo = userAddressRepo;
        }

        [HttpPost("SaveUserAddress/{userId}")]
        public async Task<IActionResult> SaveUserAddress([FromBody] AddUserAddressDTO userAddress, int? userId)
        {
            if (userId is null)
            {
                return BadRequest(new
                {
                    Message = "Invalid userId"
                });
            }
            bool result = await userAddressRepo.SaveUserAddress(userId, userAddress);
            if (result)
            {
                return Ok(new
                {
                    Message =  "Address saved successfully"
                }) ;
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }

        [HttpGet("GetDefaultUserAddress")]
        public async Task<IActionResult> GetDefaultUserAddress(int? userId)
        {
            if (userId is null)
            {
                return BadRequest(new
                {
                    Message = "Invalid userId"
                });
            }
            var address = await userAddressRepo.RetrieveDefaultUserAddress(userId);
            if (address is null)
            {
                return NotFound(new
                {
                    Message = "No default Address found"
                });
            }
            if (address is not null)
            {
                return Ok(address);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }

        [HttpGet("GetUserAllAddresses")]
        public async Task<IActionResult> GetUserAllAddresses(int? userId)
        {
            if (userId is null)
            {
                return BadRequest(new
                {
                    Message = "Invalid userId"
                });
            }
            var address = await userAddressRepo.GetUsersAllAddress(userId);
           
            return Ok(address);
        }
    }
}
