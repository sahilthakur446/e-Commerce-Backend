using eCommerce.Data.DTOs;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet]
        [Route("GetCategoriesWithProductCounts")]
        public async Task<IActionResult> GetCategoriesWithProductCounts() 
        
        {
            var categoryList = await _categoryRepository.GetCategoriesWithProductCountsAsync();
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

        [HttpGet]
        [Route("GetCategoryProductList/{categoryId}")]
        public async Task<IActionResult> GetCategoryProductList(int categoryId)
            {
            var productList = await _categoryRepository.GetProductsForCategoryAsync(categoryId);
            if (productList is not null)
                {
                return Ok(productList);
                }
            return NotFound();
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _categoryRepository.CreateCategoryAsync(categoryDTO);
            if (result)
            {
                return StatusCode((int)HttpStatusCode.OK);
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO is null)
                {
                    return BadRequest("Invalid Input");
                }
                bool result = await _categoryRepository.UpdateCategoryAsync(id, categoryDTO);
                if (result)
                {
                    return Ok();
                }
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Messsage = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
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
