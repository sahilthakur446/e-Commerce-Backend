﻿using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Implementation
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BrandDTO> GetBrand(int? BrandId) 
        {
            var brand = await _dbContext.Brands.FindAsync(BrandId);
            if (brand is null)
            {
                return null;
            }
            var brandDTO = new BrandDTO 
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName
            };
            return brandDTO;
        }

        public async Task<BrandDTO> GetBrandWithProducts(int? BrandId)
        {
            var brand = await _dbContext.Brands.Include(brand => brand.Products).FirstOrDefaultAsync(brand => brand.BrandId == BrandId);
            if (brand is null)
            {
                return null;
            }
            var brandDTO = new BrandDTO
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                productDTOs = MapProductsToProductDTOs(brand.Products)
            };
            return brandDTO;
        }

        public async Task<List<BrandSummaryDTO>> GetBrandSummaryListAsync()
        {
            var Brands = await _dbContext.Brands.ToListAsync();
            if (Brands != null)
            {
                var brandSummaries = Brands
                    .Select(c => new BrandSummaryDTO { BrandId = c.BrandId, BrandName = c.BrandName })
                    .ToList();
                if (!brandSummaries.Any())
                {
                    throw new Exception("No Brands found.");
                }
                return brandSummaries;
            }
            return null;
        }

        public async Task<List<BrandDTO>> GetBrandDetailsListAsync()
        {
            var Brands = await _dbContext.Brands.Include(c => c.Products).ToListAsync();
            List<BrandDTO> brandDetails = new List<BrandDTO>();
            if (Brands.Any())
            {
                foreach (var brand in Brands)
                {
                    var brandDetail = new BrandDTO
                    {
                        BrandId = brand.BrandId,
                        BrandName = brand.BrandName,
                        productDTOs = MapProductsToProductDTOs(brand.Products)
                    };
                    brandDetails.Add(brandDetail);
                }
                return brandDetails;
            }
            return null;
        }

        public async Task<bool> AddBrandAsync(AddBrandDTO BrandDTO)
        {
            if (BrandDTO is null)
            {
                return false;
            }
            var brand = new Brand
            {
                BrandName = BrandDTO.BrandName,
            };
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateBrandAsync(AddBrandDTO BrandDTO)
        {
            var brand = await _dbContext.Brands.FindAsync(BrandDTO.BrandId);
            if (brand is null)
            {
                return false;
            }
            _dbContext.Entry(brand).CurrentValues.SetValues(BrandDTO);
            var changes = await _dbContext.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteBrandAsync(int? BrandId)
        {
            var brand = await _dbContext.Brands.FindAsync(BrandId);
            if (brand is null)
            {
                return false;
            }
            _dbContext.Brands.Remove(brand);
            var changes = await _dbContext.SaveChangesAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }
        private List<ProductDTO> MapProductsToProductDTOs(List<Product> Products)
        {
            var productDTOs = new List<ProductDTO>();

            if (!(Products is null))
            {
                foreach (var product in Products)
                {
                    var productDto = new ProductDTO
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        Price = product.Price,
                        ImagePath = product.ImagePath,
                        StockQuantity = product.StockQuantity,
                        CategoryId = product.CategoryId,
                        BrandId = product.BrandId
                    };
                    productDTOs.Add(productDto);
                }
                return productDTOs;
            }
            return null;
        }

    }
}
