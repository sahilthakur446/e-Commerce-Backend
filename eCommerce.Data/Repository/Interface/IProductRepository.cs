using eCommerce.Data.DTOs;
using eCommerce.Data.Models;


namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<ProductShowcaseDTO>> GetAllProductsAsync();
        Task<ProductShowcaseDTO> GetProduct(int id);
        Task<List<ProductShowcaseDTO>> GetProductsAbovePriceAsync(int minPrice);
        Task<List<ProductShowcaseDTO>> GetProductsBelowPriceAsync(int maxPrice);
        Task<List<ProductShowcaseDTO>> GetProductsWithinPriceRangeAsync(int minPrice, int maxPrice);
        Task<bool> AddProductAsync(AddProductDTO product);
        Task<bool> DeleteProductAsync(int? id);
        Task<bool> UpdateProductAsync(int id, UpdateProductDTO productDTO);
        }
    }
