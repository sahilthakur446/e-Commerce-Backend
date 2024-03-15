using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using eCommerce.Utilities.Enums;
using static eCommerce.Utilities.GenderConverter;

namespace eCommerce.Data.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductRepository(ApplicationDbContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products.Include(product => product.Brand).ToListAsync();
                var productDtos = new List<ProductDTO>();

                foreach (var product in products)
                {
                    var productDto = ProductToProductDTOMapper(product);
                    if (!(productDto is null))
                    {
                        productDtos.Add(productDto);
                    }

                }
                return productDtos;
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public async Task<ProductDTO> GetProduct(int id)
        {
            var product = await _dbContext.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                var productDto = ProductToProductDTOMapper(product);
                if (!(productDto is null))
                {
                    return productDto;
                }
            }
            return null;
        }

        public async Task<List<ProductDTO>> GetProductsAbovePriceAsync(int minPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.Price >= minPrice)
                .ToListAsync();

            var productDTOsList = MapProductsToProductDTOs(productList);

            if (!productDTOsList.Any())
                {
                return null;
                }
            return productDTOsList;
            }

        public async Task<List<ProductDTO>> GetProductsBelowPriceAsync(int maxPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.Price <= maxPrice)
                .ToListAsync();

            var productDTOsList = MapProductsToProductDTOs(productList);

            if (!productDTOsList.Any())
            {
                return null;
            }
            return productDTOsList;
        }

        public async Task<List<ProductDTO>> GetProductsWithinPriceRangeAsync(int minPrice, int maxPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => (p.Price >= minPrice) && (p.Price <= maxPrice) )
                .ToListAsync();

            var productDTOsList = MapProductsToProductDTOs(productList);

            if (!productDTOsList.Any())
                {
                return null;
                }
            return productDTOsList;
            }

        private ProductDTO ProductToProductDTOMapper(Product product)
        {
            if (!(product is null))
            {
                var productDto = new ProductDTO
                    {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Price = product.Price,
                    TargetGender = GetTargetGenderString(product.TargetGender),
                    ImagePath = product.ImagePath,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.BrandName
                };
                return productDto;
            }
            return null;
        }
        
        private async Task<string> SaveImageAsync(int? categoryId, IFormFile imageFile)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            string categoryName = category.CategoryName;

            if (!string.IsNullOrEmpty(categoryName))
            {
                string imagesFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", categoryName.ToLower());
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string fullImagePath = Path.Combine(imagesFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return Path.Combine("images", categoryName.ToLower(), uniqueFileName);
            }

            return string.Empty;
        }

        private bool DeleteImage(string imagePath)
        {
            string imageFullPath = Path.Combine(_hostingEnvironment.WebRootPath, imagePath);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
                return true;
            }
            return false;
        }

        public async Task<bool> AddProductAsync(ProductDTO productDto)
        {
            if (!(productDto is null))
            {
                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    ProductDescription = productDto.ProductDescription,
                    TargetGender = GetTargetGender(productDto.TargetGender),
                    Price = (int)productDto.Price,
                    StockQuantity = (int)productDto.StockQuantity,
                    ImagePath = await SaveImageAsync(productDto.CategoryId, productDto.Image),
                    CategoryId =(int) productDto.CategoryId,
                    BrandId = (int)productDto.BrandId,
                    DateAdded = DateTime.Now,
                    TimesBought = 0
                };

                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDTO productDTO)
        {
            string updateImagePath;
            string oldImagePath = null;
            if (!(productDTO is null))
            {
                var product = await _dbContext.Products.FindAsync(id);
                if (!(product is null))
                {
                    if (!(productDTO.Image is null))
                    {
                        oldImagePath = product.ImagePath;
                        updateImagePath = await SaveImageAsync(productDTO.CategoryId, productDTO.Image);
                        product.ImagePath = updateImagePath;
                            if (!string.IsNullOrEmpty(oldImagePath))
                            {
                                if (!DeleteImage(oldImagePath))
                                {
                                throw new Exception("Unable to Delete Image");
                                }
                                return true;
                            }
                            
                    }


                    product.ProductName = productDTO.ProductName ?? product.ProductName;
                    product.ProductDescription = productDTO.ProductDescription ?? product.ProductDescription;
                    product.Price = productDTO.Price ?? product.Price;
                    product.StockQuantity = productDTO.StockQuantity ?? product.StockQuantity;
                    product.CategoryId = productDTO.CategoryId ?? product.CategoryId;
                    product.BrandId = productDTO.BrandId ?? product.BrandId;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;

        }


        public async Task<bool> DeleteProductAsync(int? id)
        {
            if (!(id is null))
            {
                var product = await _dbContext.Products.FindAsync(id);
                if (!(product is null))
                {
                    if (DeleteImage(product.ImagePath))
                    {
                        _dbContext.Products.Remove(product);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private List<ProductDTO> MapProductsToProductDTOs(List<Product> products)
            {
            var productDTOs = new List<ProductDTO>();

            if (!( products is null ))
                {
                foreach (var product in products)
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
