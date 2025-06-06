﻿using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
            if (!(product is null))
            {
                return Ok(product);
            }
            return NotFound();
        }


        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("GetProductsWithPagination")]
        public async Task<IActionResult> GetProductsWithPagination(int currentPage, int pageSize)
        {
            var products = await productRepository.GetAllProductsUsingPaginationAsync(currentPage, pageSize);
            return Ok(products);
        }

        [HttpGet("GetProductsAbovePrice/{minPrice}")]
        public async Task<IActionResult> GetProductsAbovePriceRange(int? minPrice)
        {
            var products = await productRepository.GetProductsAbovePriceAsync(minPrice);
            if (products.Any())
            {
                return Ok(products);
            }
            return NotFound();
        }

        [HttpGet("GetProductsBelowPrice/{maxPrice}")]
        public async Task<IActionResult> GetProductsBelowPriceRange(int? maxPrice)
        {
            try
            {
                var products = await productRepository.GetProductsBelowPriceAsync(maxPrice);
                if (products.Any())
                {
                    return Ok(products);
                }
                return NotFound();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("GetProductsWithinPriceRange/{minPrice}/{maxPrice}")]
        public async Task<IActionResult> GetProductsWithinPriceRange(int? minPrice, int? maxPrice)
        {
            try
            {
                var products = await productRepository.GetProductsWithinPriceRangeAsync(minPrice, maxPrice);
                if (products.Any())
                {
                    return Ok(products);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetFilteredProducts ")]
        public async Task<IActionResult> GetFilteredProducts(int? minPrice, int? maxPrice, string? category, int? categoryId, int? brandId, string? gender, bool isNew)
        {
            try
            {
                var products = await productRepository.GetProductsWithFiltersAsync(minPrice, maxPrice, category, categoryId, brandId, gender, isNew);
                return Ok(products);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }

        [HttpGet("GetFilteredProductsWithPagination")]
        public async Task<IActionResult> GetFilteredProductsWithPagination(int currentPage, int pageSize, int? minPrice, int? maxPrice, string? category, int? categoryId, int? brandId, string? gender, bool isNew)
        {
            try
            {
                var products = await productRepository.GetProductsWithFiltersWithPaginationAsync(currentPage, pageSize, minPrice, maxPrice, category, categoryId, brandId, gender, isNew);
                return Ok(products);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO product)
        {
            if (product == null)
            {
                return BadRequest("Product is null");
            }
            return await productRepository.AddProductAsync(product) ? Ok(new
            {
                Message = "Product Added Successfuly"
            }) : StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            bool result = await productRepository.DeleteProductAsync(id);
            if (result)
            {
                return Ok(new { Message = "Product Deleted Succwssfully" });
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO product)
        {
            bool result = await productRepository.UpdateProductAsync(id, product);
            if (result)
            {
                return Ok(new { Message = "Product Updated Succwssfully" });
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
