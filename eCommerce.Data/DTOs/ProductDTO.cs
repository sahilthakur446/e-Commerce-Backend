using eCommerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static eCommerce.Data.Models.Product;

namespace eCommerce.Data.DTOs
    {
    public class ProductDTO
        {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public GenderApplicability TargetGender { get; set; }

        [Required]
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile Image { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        }
    }