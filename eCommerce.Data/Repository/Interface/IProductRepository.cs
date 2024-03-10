using eCommerce.Data.DTOs;
using eCommerce.Data.Models;


namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProduct(int id);
        Task<bool> AddProductAsync(ProductDTO product);
        Task<bool> DeleteProductAsync(int? id);
        Task<bool> UpdateProductAsync(int id, ProductDTO productDTO);
        }
    }
