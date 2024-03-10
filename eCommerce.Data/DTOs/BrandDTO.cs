using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class BrandDTO
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public List<ProductDTO>? productDTOs { get; set; }
    }

    public class AddBrandDTO
    {
        public int? BrandId { get; set; }
        public string BrandName { get; set; }
    }
    public class BrandSummaryDTO
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
    }
}
