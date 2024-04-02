using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartController : ControllerBase
    {
        private readonly IUserCartRepository userCartRepository;

        public UserCartController(IUserCartRepository userCartRepository)
        {
            this.userCartRepository = userCartRepository;
        }

        [HttpGet("GetUserCartProducts/{userId}")]
        public async Task<IActionResult> Get(int? userId) 
        {
            try
            {
                var cartItems = await userCartRepository.GetUserCartProducts(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserCartCount/{userId}")]
        public async Task<IActionResult> GetUserCartCount(int? userId)
            {
            try
                {
                var cartCount = await userCartRepository.GetUserCartCount(userId);
                return Ok(cartCount);
                }
            catch (Exception ex)
                {
                return BadRequest(ex.Message);
                }
            }

        [HttpPost("AddProductInCart/{userId}")]
        public async Task<IActionResult> AddProductInCart(int? userId,AddUserCartDTO cartItem)
        {
            try
            {
                var result = await userCartRepository.AddProductInCart(userId,cartItem);
                if (result)
                {
                    return Ok(new { Message = "Product added to cart" });
                }
                return BadRequest(new { Message = "Failed to Add product in cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateProductInCart/{userCartId}")]
        public async Task<IActionResult> UpdateProductInCart(int? userCartId, UpdateUserCartDTO quantity)
        {
            try
            {
                var result = await userCartRepository.UpdateProductQuantityInCart(userCartId, quantity);
                if (result)
                {
                    return Ok(new { Message = "Quantity updated" });
                }
                return BadRequest(new { Message = "Failed to change quantiy" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveProductInCart/{userCartId}")]
        public async Task<IActionResult> RemoveProductInCart(int? userCartId)
        {
            try
            {
                var result = await userCartRepository.RemoveProductFromCart(userCartId);
                if (result)
                {
                    return Ok(new { Message = "Product removed successfuly" });
                }
                return BadRequest(new { Message = "Failed to remove product" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
