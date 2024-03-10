using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? TargetGender { get; set; }
        public List<ProductDTO>? productDTOs { get; set; }
    }

    public class AddCategoryDTO
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TargetGender { get; set; }
    }
    public class CategorySummaryDTO
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
