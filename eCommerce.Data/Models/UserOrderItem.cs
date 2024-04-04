using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    public class UserOrderItem
        {

        public int UserOrderItemId { get; set; }
        [ForeignKey("UserOrder")]
        public int UserOrderId { get; set; }
        public UserOrder UserOrder { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        }
    }
