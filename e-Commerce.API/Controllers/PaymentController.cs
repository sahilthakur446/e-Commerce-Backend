using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace e_Commerce.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
        {
        private readonly IConfiguration configuration;

        public PaymentController(IConfiguration configuration)
        {
            this.configuration = configuration;
            }

        [HttpGet("Payment")]
        public IActionResult Get()
            {
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", 100); // this amount should be same as transaction amount
            input.Add("currency", "INR");

            var paymentSettings = configuration.GetSection("PaymentSettings");
            string key = paymentSettings["SecretKey"];
            string secret = paymentSettings["Secret"];

            try
                {
                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();

                // Assuming you want to return the order ID to the client
                return Ok(new { orderId = orderId });
                }
            catch (Exception ex)
                {
                // Handle the exception (log it, return error response, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order");
                }
            }

        }
    }
