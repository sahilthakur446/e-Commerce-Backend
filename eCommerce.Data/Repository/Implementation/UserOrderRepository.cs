using AutoMapper;
using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
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
        private readonly IConfiguration configuration;

        public UserOrderRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            }

        public async Task<List<OrderDetailsDTO>> GetUserOrderAsync(int? userId)
        {
            var userOrders = await context.UserOrders
                .Include(o => o.UserOrderItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Brand)
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

        public async Task<List<AdminOrderDetailsDTO>> GetUserOrderListWihtPaymentStatusAsync()
            {
            var userOrders = await context.UserOrders
                .Include(o => o.UserOrderItems)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Brand)
                .ToListAsync();

            var ordersDetailsList = new List<AdminOrderDetailsDTO>();

            foreach (var order in userOrders)
                {
                var orderDetailsDto = mapper.Map<AdminOrderDetailsDTO>(order);
                orderDetailsDto.PaymentStatus = GetPaymentStatus(orderDetailsDto.PaymentId);
                orderDetailsDto.OrderItems = order.UserOrderItems
                    .Select(item => mapper.Map<OrderItemDetailsDTO>(item))
                    .ToList();

                ordersDetailsList.Add(orderDetailsDto);
                }

            return ordersDetailsList;
            }

        private string GetPaymentStatus(string paymentId)
            {
            var paymentSettings = configuration.GetSection("PaymentSettings");
            string key = paymentSettings["SecretKey"];
            string secret = paymentSettings["Secret"];
            RazorpayClient client = new RazorpayClient(key, secret);
            var payment = client.Payment.Fetch(paymentId);
            var status = payment.Attributes["status"].ToString();
            return status;
            }

        public async Task<bool> ChangeOrderStatus(int orderId, string orderStatus)
            {
            try
                {
                var order = await context.UserOrders.FindAsync(orderId);
                if (order is null)
                {
                    throw new Exception("No order Found");
                }
                order.Status = orderStatus;
                await context.SaveChangesAsync();
                return true;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
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
                    await transaction.CommitAsync();
                    await context.SaveChangesAsync();
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
