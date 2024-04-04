using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    public class UserOrder
        {

        public UserOrder()
            {
            UserOrderItems = new List<UserOrderItem>();
            }

        public int UserOrderId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public List<UserOrderItem> UserOrderItems { get; set; }
        public string PaymentId { get; set; }
        public int TotalAmount { get; set; }

        [ForeignKey("UserAddress")]
        public int UserAddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        }
    }
