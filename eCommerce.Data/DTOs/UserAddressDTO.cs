using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
    {
    public class UserAddressDTO
        {
        public string FullName { get; set; }

        public long MobileNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? Area { get; set; }
        public string? Landmark { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Pincode { get; set; }
        public bool IsDefault { get; set; } = false;
        }
    public class AddUserAddressDTO
        {
        [Required]
        public string FullName { get; set; }
        [Required]
        public long MobileNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? Area { get; set; }
        public string? Landmark { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Pincode { get; set; }
        public bool IsDefault { get; set; } = false;
        }
    public class UpdateUserAddressDTO
        {
        public string? FullName { get; set; }
        
        public long? MobileNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? Area { get; set; }
        public string? Landmark { get; set; }
       
        public string? City { get; set; }
       
        public string? State { get; set; }
       
        public int? Pincode { get; set; }
        public bool? IsDefault { get; set; } = false;
        }
    }
