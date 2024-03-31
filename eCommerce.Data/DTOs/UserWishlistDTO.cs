using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class GetUserWishlistDTO
    {
        public int UserWishlistId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImagePath { get; set; }
        public string BrandName { get; set; }
        public int UserId { get; set; }
    }

    public class AddUserWishlistDTO
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

    }
}

