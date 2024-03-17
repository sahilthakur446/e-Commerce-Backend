using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
{
    public interface IBrandRepository
    {
        Task<BrandDTO> GetBrandByIdAsync(int? BrandId);
        Task<BrandDTO> GetBrandWithProductsAsync(int? BrandId);
        Task<List<BrandWithProductCount>> GetBrandsWithProductCountsAsync();
        Task<List<BrandSummaryDTO>> GetBrandSummaryListAsync();
        Task<List<BrandDTO>> GetAllBrandsWithProductsAsync();
        Task<bool> AddBrandAsync(CreateUpdateBrandDto BrandDTO);
        Task<bool> UpdateBrandAsync(int id, CreateUpdateBrandDto BrandDTO);
        Task<bool> DeleteBrandAsync(int? BrandId);
    }
}
