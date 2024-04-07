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

        [HttpGet("initialize")]
        public IActionResult Get(string amount)
            {
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", amount);
            input.Add("currency", "INR");
            input.Add("payment_capture", true);

            var paymentSettings = configuration.GetSection("PaymentSettings");
            string key = paymentSettings["SecretKey"];
            string secret = paymentSettings["Secret"];

            try
                {
                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();

                return Ok(new { orderId = orderId });
                }
            catch (Exception ex)
                {
                // Handle the exception (log it, return error response, etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order");
                }
            }

        [HttpGet("confirm")]
        public IActionResult ConfirmPayment(string paymentId)
            {
            var paymentSettings = configuration.GetSection("PaymentSettings");
            string key = paymentSettings["SecretKey"];
            string secret = paymentSettings["Secret"];
            RazorpayClient client = new RazorpayClient(key, secret);
            var payment = client.Payment.Fetch(paymentId);
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            var status = paymentCaptured.Attributes["status"].ToString();
            return Ok(new { paymentstatus = status });
            }

        [HttpGet("GetPaymentStatus")]
        public IActionResult GetPaymentStatus(string paymentId)
            {
            var paymentSettings = configuration.GetSection("PaymentSettings");
            string key = paymentSettings["SecretKey"];
            string secret = paymentSettings["Secret"];
            RazorpayClient client = new RazorpayClient(key, secret);
            var payment = client.Payment.Fetch(paymentId);
            var status = payment.Attributes["status"].ToString();
            return Ok(new { paymentstatus = status });
            }
        }
    }
