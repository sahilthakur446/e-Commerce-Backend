using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace e_Commerce.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : Controller
        {
        private readonly IUserProfileRepository userProfileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepo)
            {
            this.userProfileRepo = userProfileRepo;
            }

        [Authorize]
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
            var address = await userProfileRepo.GetUserAddress(addressId);
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

        [Authorize]
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
            var address = await userProfileRepo.RetrieveDefaultUserAddress(userId);
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

        [Authorize]
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
            var address = await userProfileRepo.GetUsersAllAddress(userId);

            return Ok(address);
            }

        [Authorize]
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
            bool result = await userProfileRepo.SaveUserAddress(userId, userAddress);
            if (result)
                {
                return Ok(new
                    {
                    Message = "Address saved successfully"
                    });
                }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        [Authorize]
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
            bool result = await userProfileRepo.UpdateUserAddress(addressId, userAddress);
            if (result)
                {
                return Ok(new
                    {
                    Message = "Address updated successfully"
                    });
                }
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        [Authorize]
        [HttpDelete("DeleteUserAddress/{addressId}")]
        public async Task<IActionResult> DeleteUserAddress(int? addressId)
            {
            try
                {
                bool result = await userProfileRepo.DeleteAddress(addressId);
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

        [Authorize]
        [HttpPut("SetDefaultAddress/{addressId}")]
        public async Task<IActionResult> SetDefaultAddress(int? addressId)
        {
            try
            {
                bool result = await userProfileRepo.SetDefaultAddress(addressId);
                if (result)
                {
                    return Ok(new { Message = "Address Set to default successfully" });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetUserAllWishlistProducts/{userId}")]
        public async Task<IActionResult> GetUserAllWishlistProducts(int? userId)
        {
            try
            {
                var wishlistedItems = await userProfileRepo.GetUserAllWishlistProducts(userId);
                return Ok(wishlistedItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("AddToWishlist/{userId}")]
        public async Task<IActionResult> AddToWishlist([FromBody] AddUserWishlistDTO wishListItem, int? userId)
        {
            try
            {
                bool result = await userProfileRepo.AddToWishlist(userId, wishListItem);
                if (result)
                {
                    return Ok(new
                    {
                        Message = "Added to wishlist"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("RemoveWishlistItem/{wishlistId}")]
        public async Task<IActionResult> RemoveWishlistItem(int? wishlistId)
        {
            try
            {
                bool result = await userProfileRepo.DeleteWishlistItem(wishlistId);
                if (result)
                {
                    return Ok(new
                    {
                        Message = "Removed Successfully"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Message = ex.Message
                }); ;
            }

        }
    }
}
