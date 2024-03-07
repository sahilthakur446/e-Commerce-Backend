using eCommerce.Data.Models.eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
    {
    public interface IProductRepository
        {
        Task<List<Product>> GetAllProductsAsync();
        Task<List<Product>> GetProductwithSpecificBrand(string brandName);
        }
    }
