using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eCommerce.Utilities.Enums;

namespace eCommerce.Data.Models
{
    public class Product
        {
        public Product()
            {
            this.Reviews = new List<Review>();
            }
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [EnumDataType(typeof(GenderApplicability))]
        public GenderApplicability TargetGender { get; set; }

        [Required]
        public int Price { get; set; }
        public int StockQuantity { get; set; }

        public string ImagePath { get; set; }

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


