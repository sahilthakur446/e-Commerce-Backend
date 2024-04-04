using AutoMapper;
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
        private readonly IMapper mapper;

        public UserOrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<OrderDetailsDTO>> GetUserOrderAsync(int? userId)
        {
            var userOrders = await context.UserOrders
                .Include(o => o.UserOrderItems)
                .ThenInclude(p => p.Product)
                .Where(o => o.UserId == userId && (o.Status == "Pending" || o.Status == "Shipped"))
                .ToListAsync();

            var ordersDetailsList = new List<OrderDetailsDTO>();

            foreach (var order in userOrders)
            {
                var orderDetailsDto = mapper.Map<OrderDetailsDTO>(order);
                orderDetailsDto.OrderItems = order.UserOrderItems
                    .Select(item => mapper.Map<OrderItemDetailsDTO>(item))
                    .ToList();

                ordersDetailsList.Add(orderDetailsDto);
            }

            return ordersDetailsList;
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
