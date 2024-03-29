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

        [HttpGet("GetAddress/{addressId}")]
        public async Task<IActionResult> GetAddress(int? addressId)
            {
            if (addressId is null)
                {
                return BadRequest(new
                    {
                    Message = "Invalid addressId"
                    });
                }
            var address = await userAddressRepo.GetUserAddress(addressId);
            if (address is null)
                {
                return NotFound(new
                    {
                    Message = "No Address found"
                    });
                }
            if (address is not null)
                {
                return Ok(address);
                }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        [HttpGet("GetDefaultUserAddress/{userId}")]
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

        [HttpGet("GetUserAllAddresses/{userId}")]
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
                    Message = "Address saved successfully"
                    });
                }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        [HttpPut("UpdateUserAddress/{addressId}")]
        public async Task<IActionResult> UpdateUserAddress([FromBody] UpdateUserAddressDTO userAddress, int? addressId)
            {
            if (addressId is null)
                {
                return BadRequest(new
                    {
                    Message = "Invalid addressId"
                    });
                }
            bool result = await userAddressRepo.UpdateUserAddress(addressId, userAddress);
            if (result)
                {
                return Ok(new
                    {
                    Message = "Address updated successfully"
                    });
                }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        [HttpDelete("DeleteUserAddress/{addressId}")]
        public async Task<IActionResult> DeleteUserAddress(int? addressId)
            {
            try
                {
                bool result = await userAddressRepo.DeleteAddress(addressId);
                if (result)
                    {
                    return Ok(new
                        {
                        Message = "Address deleted successfully"
                        });
                    }
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
                }
            catch (Exception ex) 
                {
                return Ok(new
                    {
                    Message = ex.Message
                    });;
                }
    
            }


        }
    }
