using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Implementation
    {
    public class UserOrderRepository : IUserOrderRepository
        {
        private readonly ApplicationDbContext context;

        public UserOrderRepository(ApplicationDbContext context)
            {
            this.context = context;
            }

        public async Task<List<GetUserOrderProductsDTO>> GetUserOrderAsync(int? userId)
            {
            var userOrders = await context.UserOrders
                                  .Include(o => o.UserOrderItems)
                                  .ThenInclude(p => p.Product)
                                  .Include(o => o.UserOrderItems) 
                                  .ThenInclude(p => p.Product.Brand) 
                                  .Include(o => o.UserOrderItems) 
                                  .ThenInclude(p => p.Product.Category)
                                  .Where(o => o.UserId == userId)
                                  .ToListAsync();

            var userOrderItems = new List<UserOrderItem>();
            foreach (var item in userOrders)
                {
                userOrderItems.AddRange(item.UserOrderItems);
                }

            var userOrderProducts = new List<GetUserOrderProductsDTO>();
            foreach (var item in userOrderItems)
                {
                var product = new GetUserOrderProductsDTO
                    {
                    UserOrderId = item.UserOrderId,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    Price = item.Product.Price,
                    ImagePath = item.Product.ImagePath,
                    BrandName = item.Product.Brand.BrandName,
                    UserId = (int) userId
                    };
                userOrderProducts.Add(product);
                }

            return userOrderProducts;
            }

        public async Task<bool> AddUserOrderAsync(int userId, AddUserOrderDTO userOrderDetails)
            {
            using (var transaction = context.Database.BeginTransaction())
                {
                try
                    {
                    var newUserOrder = new UserOrder
                        {
                        UserId = userId,
                        PaymentId = userOrderDetails.PaymentId,
                        TotalAmount = userOrderDetails.TotalAmount,
                        UserAddressId = userOrderDetails.UserAddressId,
                        OrderDate = DateTime.Now,
                        Status = "Pending"
                        };
                    await context.UserOrders.AddAsync(newUserOrder);
                    await context.SaveChangesAsync();
                    foreach (var item in userOrderDetails.UserCartItems)
                        {
                        var userOrderItem = new UserOrderItem
                            {
                            UserOrderId = newUserOrder.UserOrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price
                            };
                        await context.AddAsync(userOrderItem);
                        }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                    }
                catch (DbUpdateException ex)
                    {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                    }
                }
            }
        }
    }
