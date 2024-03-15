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

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
            {
            var product = await productRepository.GetProduct(id);
            if (!( product is null ))
                {
                return Ok(product);
                }
            return NotFound();
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

        [HttpGet("GetProductsAbovePrice/{minPrice}")]

        public async Task<IActionResult> GetProductsAbovePriceRange(int minPrice)
            {
            var products = await productRepository.GetProductsAbovePriceAsync(minPrice);
            if (products.Any())
                {
                return Ok(products);
                }
            return NotFound();
            }
        [HttpGet("GetProductsBelowPrice/{maxPrice}")]

        public async Task<IActionResult> GetProductsBelowPriceRange(int maxPrice)
            {
            var products = await productRepository.GetProductsBelowPriceAsync(maxPrice);
            if (products.Any())
                {
                return Ok(products);
                }
            return NotFound();
            }


        [HttpGet("GetProductsWithinPriceRange/{minPrice}/{maxPrice}")]

        public async Task<IActionResult> GetProductsWithinPriceRange(int minPrice, int maxPrice)
            {
            var products = await productRepository.GetProductsWithinPriceRangeAsync(minPrice,maxPrice);
            if (products.Any())
                {
                return Ok(products);
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
        [Route("DeleteProduct/{id}")]
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
        [Route("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] ProductDTO product)
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
