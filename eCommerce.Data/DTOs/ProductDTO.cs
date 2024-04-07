using eCommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static eCommerce.Data.Models.Product;

namespace eCommerce.Data.DTOs
    {
    public class AddProductDTO
        {
        public string ProductName { get; set; }
        
        public string ProductDescription { get; set; }
        public string TargetGender { get; set; }

        public int Price { get; set; }
        public int StockQuantity { get; set; }
        [SwaggerParameter(Required = false)]
        public IFormFile Image { get; set; }
        
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        }

    public class UpdateProductDTO
        {
        public string? ProductName { get; set; }

        public string? ProductDescription { get; set; }
        public string? TargetGender { get; set; }

        public int? Price { get; set; }
        public int? StockQuantity { get; set; }
        [SwaggerParameter(Required = false)]
        public IFormFile? Image { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        }

    public class ProductInfoDTO
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }
        public string TargetGender { get; set; }

        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        }

    public class ProductInfoDTOWithPagination
    {
        public List<ProductInfoDTO> ProductList { get; set; }
        public int TotalPages { get; set; }
    }
}