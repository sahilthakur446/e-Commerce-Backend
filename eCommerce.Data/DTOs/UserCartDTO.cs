using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class GetUserCartDTO
    {
        public int UserCartId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImagePath { get; set; }
        public string BrandName { get; set; }
        public int UserId { get; set; }
    }

    public class AddUserCartDTO
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

    }

    public class UpdateUserCartDTO
    {
        public int Quantity { get; set; }
    }
}
