using System.ComponentModel.DataAnnotations;
using eCommerce.Utilities.Enums;

namespace eCommerce.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.products = new List<Product>();
        }
        
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        [EnumDataType(typeof(GenderApplicability))]
        public GenderApplicability CategoryTargetGender { get; set; }
        public List<Product> products { get; set; }
    }
}
