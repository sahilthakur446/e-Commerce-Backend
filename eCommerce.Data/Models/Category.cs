using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    public class Category
        {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string CategoryGender { get; set; }
        }
    }
