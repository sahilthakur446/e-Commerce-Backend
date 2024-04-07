using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace e_Commerce.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrderController : ControllerBase
        {
        private readonly IUserOrderRepository userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            this.userOrderRepo = userOrderRepo;
            }

        [Authorize]
        [HttpGet("GetUserOrders/{userId}")]
        public async Task<IActionResult> Get(int? userId)
            {
            var userOrders = await userOrderRepo.GetUserOrderAsync(userId);
            return Ok(userOrders);
            }

        [Authorize]
        [HttpGet("GetUserOrderListWihtPaymentStatusAsync")]
        public async Task<IActionResult> GetUserOrderListWihtPaymentStatusAsync()
            {
            var userOrdersList = await userOrderRepo.GetUserOrderListWihtPaymentStatusAsync();
            return Ok(userOrdersList);
            }

        [Authorize]
        [HttpPost("AddUserOrder/{userId}")]
        public async Task<IActionResult> AddUserOrder(int userId, [FromBody] AddUserOrderDTO userOrderDetails)
            {
            try
                {
                bool result = await this.userOrderRepo.AddUserOrderAsync(userId, userOrderDetails);
                if (result)
                    {
                    return Ok( new {Message = "Order Created Successfully" });
                    }
                return BadRequest();
                }
            catch (Exception ex) 
                {
                return StatusCode((int)HttpStatusCode.InternalServerError,ex.Message);
                }
            }

        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeOrderStatus/{orderId}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, string orderStatus)
            {
            try
                {
                bool result = await this.userOrderRepo.ChangeOrderStatus(orderId, orderStatus);
                if (result)
                    {
                    return Ok(new { Message = "Order Status Changed Successfully" });
                    }
                return BadRequest();
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        }
    }
