using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static eCommerce.Data.Models.Category;

namespace eCommerce.Data.Models
    {
    public class Category
        {
        public enum TargetGender
            {
            Male,
            Female,
            Unisex
            }
        [Key]
        public int CategoryId { get; set; }
        
        public string CategoryName { get; set; }
        [EnumDataType(typeof(TargetGender))]
        public TargetGender CategoryTargetGender { get; set; }
        }
    }
