using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<Brand>> GetProductwithSpecificBrand(string brandName);
        Task<bool> AddProduct(ProductDTO product);
        }
    }
