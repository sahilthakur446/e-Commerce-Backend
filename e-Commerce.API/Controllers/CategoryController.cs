﻿using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            var categoryList = await _categoryRepository.GetCategorySummaryListAsync();
            if (categoryList is not null)
            {
                return Ok(categoryList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetCategoryDetailsList")]
        public async Task<IActionResult> GetCategoryDetailsList() 
        {
            var categoryList = await _categoryRepository.GetCategoryDetailsListAsync();
            if (categoryList is not null)
            {
                return Ok(categoryList);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _categoryRepository.AddCategoryAsync(categoryDTO);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromForm] AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _categoryRepository.UpdateCategoryAsync(categoryDTO);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Route("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int? categoryId)
        {
            if (categoryId == 0 || categoryId is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _categoryRepository.DeleteCategoryAsync(categoryId);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
