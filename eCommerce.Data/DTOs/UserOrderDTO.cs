using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
    {
    public class OrderDetailsDTO
    {
        public int UserOrderId { get; set; }
        public string PaymentId { get; set; }
        public int TotalAmount { get; set; }

        public int UserAddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDetailsDTO> OrderItems { get; set; }
    }

    public class OrderItemDetailsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }
    }


    public class AddUserOrderDTO
        {
        public int UserId { get; set; }
        public List<GetUserCartDTO> UserCartItems { get; set; }
        public string PaymentId { get; set; }
        public int TotalAmount { get; set; }
        public int UserAddressId { get; set; }

        }

    }
