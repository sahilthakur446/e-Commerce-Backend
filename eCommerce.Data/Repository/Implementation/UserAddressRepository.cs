using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Implementation
    {
     class UserAddressRepository : IUserAddress
        {
        private readonly ApplicationDbContext context;

        public UserAddressRepository(ApplicationDbContext _context)
        {
            context = _context;
            }

        public Task<bool> AddUserAddress(int? userId, UserAddressDTO userAddress)
            {
            throw new NotImplementedException();
            }

        //public Task<bool> AddUserAddress(int? userId, UserAddressDTO userAddress)
        //    {
        //    if (userId is null || userAddress is null)
        //    {
        //        return false;
        //    }
        //    var newUserAddress = new UserAddress
        //        {
        //        FullName = userAddress.FullName,
        //        MobileNumber = userAddress.MobileNumber,
        //        HouseNumber = userAddress.HouseNumber,
        //        Area = userAddress.Area,
        //        Landmark = userAddress.Landmark,
        //        City = userAddress.City,
        //        State = userAddress.State,
        //        Pincode = userAddress.Pincode
        //        };
        //    return true;
        //}

        public Task<UserAddressDTO> GetUserDefaultAddress(int userID)
            {
            throw new NotImplementedException();
            }

         public Task<List<UserAddressDTO>> GetUsersAllAddress(int userID)
            {
            throw new NotImplementedException();
            }
        }
    }
