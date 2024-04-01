using eCommerce.Data.DTOs;
using eCommerce.Data.Models;


namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<ProductInfoDTO>> GetAllProductsAsync();
        Task<ProductInfoDTO> GetProduct(int id);
        Task<List<ProductInfoDTO>> GetProductsAbovePriceAsync(int? minPrice);
        Task<List<ProductInfoDTO>> GetProductsBelowPriceAsync(int? maxPrice);
        Task<List<ProductInfoDTO>> GetProductsWithinPriceRangeAsync(int? minPrice, int? maxPrice);
        Task<List<ProductInfoDTO>> GetProductsWithFiltersAsync(int? minPrice, int? maxPrice, string? category, int? categoryId, int? brandId, string? gender, bool isNew);
        Task<bool> AddProductAsync(AddProductDTO product);
        Task<bool> DeleteProductAsync(int? id);
        Task<bool> UpdateProductAsync(int id, UpdateProductDTO productDTO);
        }
    }
