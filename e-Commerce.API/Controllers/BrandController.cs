﻿using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace e_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet]
        [Route("GetBrand/{brandId}")]
        public async Task<IActionResult> GetBrand(int? brandId)
        {
            var brand = await _brandRepository.GetBrand(brandId);
            if (brand is not null)
            {
                return Ok(brand);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetBrandWithProducts/{brandId}")]
        public async Task<IActionResult> GetBrandWithProducts(int? brandId)
        {
            var brand = await _brandRepository.GetBrandWithProducts(brandId);
            if (brand is not null)
            {
                return Ok(brand);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetBrandList")]
        public async Task<IActionResult> GetBrandList()
        {
            var brandList = await _brandRepository.GetBrandSummaryListAsync();
            if (brandList is not null)
            {
                return Ok(brandList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("GetBrandDetailsList")]
        public async Task<IActionResult> GetBrandDetailsList()
        {
            var brandList = await _brandRepository.GetBrandDetailsListAsync();
            if (brandList is not null)
            {
                return Ok(brandList);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("AddBrand")]
        public async Task<IActionResult> AddBrand([FromForm] AddBrandDTO brandDTO)
        {
            if (brandDTO is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _brandRepository.AddBrandAsync(brandDTO);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut]
        [Route("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand([FromForm] AddBrandDTO brandDTO)
        {
            if (brandDTO is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _brandRepository.UpdateBrandAsync(brandDTO);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete]
        [Route("DeleteBrand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int? brandId)
        {
            if (brandId == 0 || brandId is null)
            {
                return BadRequest("Invalid Input");
            }
            bool result = await _brandRepository.DeleteBrandAsync(brandId);
            if (result)
            {
                return Ok();
            }
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
