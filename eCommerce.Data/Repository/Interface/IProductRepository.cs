using eCommerce.Data.DTOs;
using eCommerce.Data.Models;


namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProduct(int id);
        Task<List<ProductDTO>> GetProductsAbovePriceAsync(int minPrice);
        Task<List<ProductDTO>> GetProductsBelowPriceAsync(int maxPrice);
        Task<List<ProductDTO>> GetProductsWithinPriceRangeAsync(int minPrice, int maxPrice);
        Task<bool> AddProductAsync(ProductDTO product);
        Task<bool> DeleteProductAsync(int? id);
        Task<bool> UpdateProductAsync(int id, ProductDTO productDTO);
        }
    }
