using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    public class UserAddress
        {
        [Key]
        public int UserAddressId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string Area { get; set; }
        public string? Landmark { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Pincode { get; set; }
        public bool IsDefault { get; set; } = false;
        [ForeignKey("UserAddress")]
        public int UserId { get; set; }
        public User User { get; set; }
        }
    }
