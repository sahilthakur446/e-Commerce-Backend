using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using eCommerce.Utilities.Enums;
using static eCommerce.Utilities.GenderConverter;
using AutoMapper;

namespace eCommerce.Data.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper mapper;

        public ProductRepository(ApplicationDbContext dbContext, IWebHostEnvironment hostingEnvironment, IMapper _mapper)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
            mapper = _mapper;
            }

        public async Task<List<ProductShowcaseDTO>> GetAllProductsAsync()
        {
            try
            {
                var products = await _dbContext.Products
                    .Include(product => product.Brand)
                    .Include(product => product.Category)
                    .ToListAsync();
                var productDtos = mapper.Map<List<ProductShowcaseDTO>>(products);
                
                return productDtos;
            }
            catch (Exception error)
            {
                throw new Exception("Some Error Occured");
            }
        }

        public async Task<ProductShowcaseDTO> GetProduct(int id)
        {
            var product = await _dbContext.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                var productDto = mapper.Map<ProductShowcaseDTO>(product);
                if (!(productDto is null))
                {
                    return productDto;
                }
            }
            return null;
        }

        public async Task<List<ProductShowcaseDTO>> GetProductsAbovePriceAsync(int minPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.Price >= minPrice)
                .ToListAsync();

            var productDTOsList = mapper.Map<List<ProductShowcaseDTO>>(productList);

            if (!productDTOsList.Any())
                {
                return null;
                }
            return productDTOsList;
            }

        public async Task<List<ProductShowcaseDTO>> GetProductsBelowPriceAsync(int maxPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => p.Price <= maxPrice)
                .ToListAsync();

            var productDTOsList = mapper.Map<List<ProductShowcaseDTO>>(productList);
            if (!productDTOsList.Any())
            {
                return null;
            }
            return productDTOsList;
        }

        public async Task<List<ProductShowcaseDTO>> GetProductsWithinPriceRangeAsync(int minPrice, int maxPrice)
            {
            var productList = await _dbContext.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(p => (p.Price >= minPrice) && (p.Price <= maxPrice) )
                .ToListAsync();

            var productDTOsList = mapper.Map<List<ProductShowcaseDTO>>(productList);

            if (!productDTOsList.Any())
                {
                throw new Exception("No Product Found");
                }
            return productDTOsList;
            }

        private ProductShowcaseDTO ProductToProductDTOMapper(Product product)
        {
            if (!(product is null))
            {
                var productDto = new ProductShowcaseDTO
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

        public async Task<bool> AddProductAsync(AddProductDTO productDto)
        {
            if (!(productDto is null))
            {
                var product = mapper.Map<Product>(productDto);
                product.ImagePath = await SaveImageAsync(productDto.CategoryId,productDto.Image);
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDTO productDTO)
        {
            string updateImagePath;
            string oldImagePath;
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


                  var x =  mapper.Map(productDTO,product);
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


    }
}
