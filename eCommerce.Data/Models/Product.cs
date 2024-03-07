using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Models
    {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace eCommerce.Data.Models
        {
        public class Product
            {
            public Product()
                {
                this.Reviews = new List<Review>();
                }
            public enum Gender
                {
                Male,
                Female,
                Unisex
                }

            [Key]
            public int ProductId { get; set; }

            [Required]
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            [EnumDataType(typeof(Gender))]
            public Gender TargetGender { get; set; }

            [Required]
            public int Price { get; set; }
            public int StockQuantity { get; set; }

            public string ProductImageUrl { get; set; }

            public int CategoryId { get; set; }
            public Category Category { get; set; }

            public int BrandId { get; set; }
            public Brand Brand { get; set; }

            [NotMapped]

            public bool IsNew { get; set; }

            public DateTime DateAdded { get; set; } = DateTime.Now;
            public int TimesBought { get; set; }

            public virtual ICollection<Review> Reviews { get; set; }
            }
        }

    }
