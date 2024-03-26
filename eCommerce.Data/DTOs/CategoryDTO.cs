using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? TargetGender { get; set; }
        public List<ProductShowcaseDTO>? productDTOs { get; set; }
    }

    public class AddCategoryDTO
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string TargetGender { get; set; }
    }

    public class UpdateCategoryDTO
        {
        public string? CategoryName { get; set; }
        public string? TargetGender { get; set; }
        }

    public class CategorySummaryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CategoryWithProductCount
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? TargetGender { get; set; }
        public int ProductCount { get; set; }
    }

}
