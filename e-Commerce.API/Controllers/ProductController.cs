using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_Commerce.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
        {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
            }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> Get() 
            {
            var products = await productRepository.GetAllProductsAsync();
            if (products.Any())
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpGet("GetProductswithSpecificBrand")]
        public async Task<IActionResult> GetProductswithSpecificBrand(string brandName)
            {
            var products = await productRepository.GetProductwithSpecificBrand(brandName);
            if (products.Any())
                {
                return Ok(products);
                }
            return NotFound();
            }
        }
    }
