using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
    {
    public interface IUserOrderRepository
        {
        Task<List<GetUserOrderProductsDTO>> GetUserOrderAsync(int? userId);
        Task<bool> AddUserOrderAsync(int userId, AddUserOrderDTO userOrderDetails);
        }
    }
