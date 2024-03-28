using AutoMapper;
using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace eCommerce.Data.Repository.Implementation
    {
     public class UserProfileRepository : IUserProfileRepository
        {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserProfileRepository(ApplicationDbContext _context, IMapper mapper)
        {
            context = _context;
            this.mapper = mapper;
            }

        public async Task<bool> SaveUserAddress(int? userId, AddUserAddressDTO userAddress)
        {
            if (userId is null || userAddress is null)
            {
                return false;
            }
            var newUserAddress = mapper.Map<UserAddress>(userAddress);
            newUserAddress.UserId = (int)userId;
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
            var userAddressDto = mapper.Map<UserAddressDTO>(userAddress);
            return userAddressDto;
        }
    

         public async Task<List<UserAddressDTO>> GetUsersAllAddress(int? userId)
            {
            if (userId is null)
            {
                throw new Exception("userId is null");
            }
            var userAddresses = await context.UserAddresses.Where(u => u.UserId == userId).ToListAsync();
            var userAddressList = mapper.Map<List<UserAddressDTO>>(userAddresses);
            return userAddressList;
            }
            
    }
    }
