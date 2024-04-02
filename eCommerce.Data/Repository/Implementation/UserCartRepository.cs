using AutoMapper;
using eCommerce.Data.Data;
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
    public class UserCartRepository : IUserCartRepository
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public UserCartRepository(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<bool> AddProductInCart(int? userId, AddUserCartDTO product)
        {
            try
            {
                if (userId is null)
                {
                    throw new Exception("User Id can not be null");
                }
                var alreadyProduct = await context.UserCarts.FirstOrDefaultAsync(u => u.ProductId == product.ProductId && u.UserId == userId);
                if (alreadyProduct != null) 
                    {
                    if (alreadyProduct.Quantity == 5)
                    {
                        throw new Exception("Can not add more than 5 ");
                    }
                    alreadyProduct.Quantity += 1;
                    await context.SaveChangesAsync();
                    return true;
                    }
                var cartProduct = mapper.Map<UserCart>(product);
                await context.UserCarts.AddAsync(cartProduct);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetUserCartCount(int? userId)
            {
            try
                {
                int count = 0;
                if (userId is null)
                    {
                    throw new Exception("User Id can not be null");
                    }
                var userCartList = await context.UserCarts.Where(u => u.UserId == userId).ToListAsync();
                foreach (var userCart in userCartList) 
                    {
                    count +=  userCart.Quantity;
                    }
                return count;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        public async Task<List<GetUserCartDTO>> GetUserCartProducts(int? userId)
        {
            try
            {
                if (userId is null)
                {
                    throw new Exception("User Id can not be null");
                }

                var cartList = await context.UserCarts
                    .Where(u => u.UserId == userId)
                    .Include(uc => uc.Product)
                    .ThenInclude(p => p.Brand)
                    .ToListAsync();

                var cartListDTO = mapper.Map<List<GetUserCartDTO>>(cartList);
                return cartListDTO;
            }
            catch 
            {
                throw new Exception("Some error occured");
            }
        }

        public async Task<bool> RemoveProductFromCart(int? userCartId)
        {
            try
            {
                if (userCartId is null)
                {
                    throw new Exception("UserCart Id can not be null");
                }

                var cartItem = await context.UserCarts.FindAsync(userCartId);
                if (cartItem is null)
                {
                    throw new Exception("No product found with specified id");
                }
                context.UserCarts.Remove(cartItem);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("Some error occured");
            }
        }

        public async Task<bool> UpdateProductQuantityInCart(int? userCartId, UpdateUserCartDTO updatedCart)
        {
            try
            {
                if (userCartId is null)
                {
                    throw new Exception("UserCart Id can not be null");
                }

                var cartItem = await context.UserCarts.FindAsync(userCartId);
                if (cartItem is null)
                {
                    throw new Exception("No product found with specified id");
                }
                if (updatedCart.Quantity == 0)
                {
                    return await RemoveProductFromCart(userCartId);
                }
                cartItem.Quantity = updatedCart.Quantity;
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw new Exception("Some error occured");
            }
        }

    }
}

