using eCommerce.Data.Data;
using eCommerce.Data.Models.eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Implementation
    {
    public class ProductRepository : IProductRepository
        {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext _context)
        {
            context = _context;
            }
        public async Task<List<Product>> GetAllProductsAsync()
            {
            return ( await context.Products.ToListAsync() );
            }

        public async Task<List<Product>> GetProductwithSpecificBrand(string brandName)
            {
            int brandId;
            var brand = await context.Brands.FirstOrDefaultAsync(b => b.BrandName == brandName);
            if (brand != null)
                {
                brandId = brand.BrandId;
                return ( await context.Products.Include(p => p.Brand).Where(p => p.BrandId == brandId).ToListAsync() );
                }
            return null;
            
            }

   
        }
    }
