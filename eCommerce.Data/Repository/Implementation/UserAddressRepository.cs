using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Implementation
    {
     public class UserAddressRepository : IUserAddress
        {
        private readonly ApplicationDbContext context;

        public UserAddressRepository(ApplicationDbContext _context)
        {
            context = _context;
            }

        public async Task<bool> SaveUserAddress(int? userId, UserAddressDTO userAddress)
        {
            if (userId is null || userAddress is null)
            {
                return false;
            }
            var newUserAddress = MapUserAddressDTOtoUserAddressClass(userAddress);
            await context.UserAddresses.AddAsync(newUserAddress);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<UserAddressDTO> RetrieveDefaultUserAddress(int? userId)
            {
            if (userId is null)
            {
                throw new Exception("userId can not be null");
            }
                var userAddress = await context.UserAddresses.FirstOrDefaultAsync(u => u.UserId == userId && u.IsDefault == true);
                
            if (userAddress is null)
            {
                throw new Exception("userAddress is null");
            }
            var userAddressDto = MapUserAddressClasstoUserAddressDTO(userAddress);
            return userAddressDto;
        }
    

         public async Task<List<UserAddressDTO>> GetUsersAllAddress(int? userId)
            {
            if (userId is null)
            {
                throw new Exception("userId is null");
            }
            var userAddresses = await context.UserAddresses.Where(u => u.UserId == userId).ToListAsync();
            return MapUserAdressClassListtoUserAddressDtoList(userAddresses); 
            }
            
         private UserAddress MapUserAddressDTOtoUserAddressClass(UserAddressDTO userAddressDTO)
        {
            var newUserAddress = new UserAddress
            {
                FullName = userAddressDTO.FullName,
                MobileNumber = userAddressDTO.MobileNumber,
                HouseNumber = userAddressDTO.HouseNumber,
                Area = userAddressDTO.Area,
                Landmark = userAddressDTO.Landmark,
                City = userAddressDTO.City,
                State = userAddressDTO.State,
                Pincode = userAddressDTO.Pincode,
                IsDefault = userAddressDTO.IsDefault
            };
            return newUserAddress;
        }
        private UserAddressDTO MapUserAddressClasstoUserAddressDTO(UserAddress userAddress)
        {
            var UserAddress = new UserAddressDTO
            {
                FullName = userAddress.FullName,
                MobileNumber = userAddress.MobileNumber,
                HouseNumber = userAddress.HouseNumber,
                Area = userAddress.Area,
                Landmark = userAddress.Landmark,
                City = userAddress.City,
                State = userAddress.State,
                Pincode = userAddress.Pincode,
                IsDefault = userAddress.IsDefault
            };
            return UserAddress;
        }

        private List<UserAddressDTO> MapUserAdressClassListtoUserAddressDtoList(List<UserAddress> userAddresses)
        {
            var userAddressesDtoList = new List<UserAddressDTO>();
            foreach (var userAddress in userAddresses) 
            {
                var userAddressDto = MapUserAddressClasstoUserAddressDTO(userAddress);
                userAddressesDtoList.Add(userAddressDto);
            }
            return userAddressesDtoList;
        }
    }
    }
