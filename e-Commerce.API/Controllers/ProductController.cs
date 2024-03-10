using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace e_Commerce.API.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
        {
        private readonly IProductRepository productRepository;
        private readonly ApplicationDbContext context;

        public ProductController(IProductRepository _productRepository, ApplicationDbContext _context)
        {
            productRepository = _productRepository;
            context = _context;
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

        [HttpGet("GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await productRepository.GetProduct(id);
            if (!(product is null))
            {
                return Ok(product);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDTO product)
        {
            if (product == null)
            {
                return BadRequest("Product is null");
            }
            return await productRepository.AddProductAsync(product)? Ok(): StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
           bool result =  await productRepository.DeleteProductAsync(id);
            if (result) 
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO product)
        {
            bool result = await productRepository.UpdateProductAsync(id, product);
            if (result)
            {
                return Ok(product);
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
    }
