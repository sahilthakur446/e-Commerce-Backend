using eCommerce.Data.Models.eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    public class Brand
        {
        public Brand()
        {
            this.Products = new List<Product>();
        }
        [Key]
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        }
    }
