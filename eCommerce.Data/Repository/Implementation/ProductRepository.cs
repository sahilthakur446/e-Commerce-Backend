using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;


namespace eCommerce.Data.Repository.Implementation
    {
    public class ProductRepository : IProductRepository
        {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductRepository(ApplicationDbContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            context = _context;
            webHostEnvironment = _webHostEnvironment;
            }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
            {
            var productList = await context.Products.Include(p => p.Brand).ToListAsync();
            var productsDTOList = new List<ProductDTO>();

            foreach (var product in productList) 
                {
                var productDTO = new ProductDTO
                    {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Price = product.Price,
                    ImagePath = product.ImagePath,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.BrandName
                    };
                productsDTOList.Add(productDTO);
                }
            return productsDTOList;
            }

        public async Task<bool> AddProduct(ProductDTO product) 
            {
            var categoryEntity = await context.Categories.FindAsync(product.CategoryId);
            string CategoryName = categoryEntity.CategoryName;

            string imageFolderPath;
            string imagePath;

            if (product != null)
            {
                if (CategoryName.ToLower() == "tshirt" )
                    {
                    imagePath = Path.Combine("images", "tshirt");
                    imageFolderPath = Path.Combine(webHostEnvironment.WebRootPath, "images", "tshirts");

                    }
                else if (CategoryName.ToLower() == "jacket")
                    {
                    imagePath = Path.Combine("images", "jacket");
                    imageFolderPath = webHostEnvironment.WebRootPath + "images" + "jacket";

                    }
                else if (CategoryName.ToLower() == "dress")
                    {
                    imagePath = Path.Combine("images", "dress");
                    imageFolderPath = Path.Combine(webHostEnvironment.WebRootPath,"images","dress");
                    }
                else
                    {
                    return false;
                    }
                if (!Directory.Exists(imageFolderPath))
                {
                    Directory.CreateDirectory(imageFolderPath);
                }
                string uniqueName = Guid.NewGuid().ToString();
                string imageName = uniqueName + Path.GetExtension(product.Image.FileName);
                string imageFullPath = Path.Combine(imageFolderPath, imageName);
                imagePath = Path.Combine(imagePath, imageName);

                using (var filestream = new FileStream(imageFullPath, FileMode.Create))

                    { await product.Image.CopyToAsync(filestream); }

                var newProduct = new Product
                    {
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    TargetGender = product.TargetGender,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    ImagePath = imagePath,
                    CategoryId = product.CategoryId,
                    BrandId = product.BrandId,
                    DateAdded = DateTime.Now,
                    TimesBought = 1
                    };
                await context.AddAsync(newProduct);
                await context.SaveChangesAsync();

                return true;

            }
            return false;
        }

        public async Task<List<Brand>> GetProductwithSpecificBrand(string brandName)
            {
            int brandId;
            var productList = await context.Brands.Include(p => p.Products).Where(x => x.BrandName.ToLower() == brandName.ToLower()).ToListAsync();
            var brand = await context.Brands.FirstOrDefaultAsync(b => b.BrandName == brandName);
            if (brand != null)
                {
                brandId = brand.BrandId;
                /*await context.Products.Include(p => p.Brand).Where(p => p.BrandId == brandId).ToListAsync()*/
                return  (productList);  
                }
            return null;
            
            }

        }
    }
