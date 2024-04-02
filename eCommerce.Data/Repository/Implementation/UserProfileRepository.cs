using AutoMapper;
using eCommerce.Data.Data;
using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using eCommerce.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;


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

        public async Task<UserAddressDTO> GetUserAddress(int? addressId)
            {
            var address = await context.UserAddresses.FindAsync(addressId);
            if (address is null)
            {
                throw new Exception("No Address Found");
            }
            var addressDto = mapper.Map<UserAddressDTO>(address);
            return addressDto;
        }

        public async Task<bool> SaveUserAddress(int? userId, AddUserAddressDTO userAddress)
        {
            if (userId is null)
                {
                throw new Exception("userId can not be null");
                }
            var newUserAddress = mapper.Map<UserAddress>(userAddress);
            newUserAddress.UserId = (int)userId;
            if (newUserAddress.IsDefault == true)
            {
                var existingDefaultAddress = await context.UserAddresses
                    .FirstOrDefaultAsync(a => a.IsDefault == true &&  a.UserId == userId);
                if (existingDefaultAddress is not null)
                    {
                    existingDefaultAddress.IsDefault = false;
                    }
            }
            await context.UserAddresses.AddAsync(newUserAddress);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAddress(int? addressId, UpdateUserAddressDTO userAddress)
            {
            if (addressId is null)
                {
                throw new Exception("addressId can not be null");
                }

            var address = await context.UserAddresses.FindAsync(addressId);
            if (address is null)
                {
                throw new Exception("No Address Found");
                }
            address.FullName = userAddress.FullName ?? address.FullName;
            address.MobileNumber = userAddress.MobileNumber ?? address.MobileNumber;
            address.HouseNumber = userAddress.HouseNumber ?? address.HouseNumber;
            address.Area = userAddress.Area ?? address.Area;
            address.Landmark = userAddress.Landmark ?? address.Landmark;
            address.City = userAddress.City ?? address.City;
            address.State = userAddress.State ?? address.State;
            address.Pincode = userAddress.Pincode ?? address.Pincode;
            address.IsDefault = userAddress.IsDefault ?? address.IsDefault;

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

        public async Task<bool> DeleteAddress(int? addressId) 
            {
            if (addressId is null)
            {
                throw new Exception("addressId is null");
            }
            var userAddress = await context.UserAddresses.FindAsync(addressId);
            if (userAddress is null) 
                {
                throw new Exception("No address found for specified Id");
                }
             context.UserAddresses.Remove(userAddress);
             await context.SaveChangesAsync();
             return true;
        }

        public async Task<bool> SetDefaultAddress(int? addressId)
        {
            if (addressId is null)
            {
                throw new Exception("addressId is null");
            }
            var userAddress = await context.UserAddresses.FindAsync(addressId);
            if (userAddress is null)
            {
                throw new Exception("No address found for specified Id");
            }
            var defaultAddress = await context.UserAddresses.FirstOrDefaultAsync(a => a.IsDefault == true);
            if (defaultAddress is null)
            {
                userAddress.IsDefault = true;
                await context.SaveChangesAsync();
                return true;
            }
            defaultAddress.IsDefault = false;
            userAddress.IsDefault = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetUserWishlistDTO>> GetUserAllWishlistProducts(int? userId)
        {
            if (userId is null)
            {
                throw new Exception("User Id cannot be null");
            }
            var wishlistItems = await context.UserWishlists
                .Where(w => w.UserId == userId)
                .Include(uw => uw.Product)
                .ThenInclude(p => p.Brand)
                .ToListAsync();

            var wishlistItemsDto = mapper.Map<List<GetUserWishlistDTO>>(wishlistItems);
            return wishlistItemsDto;
        }

        public async Task<bool> AddToWishlist(int? userId, AddUserWishlistDTO wishlistedItem)
        {
            try
            {
                if (userId is null)
                {
                    throw new Exception("User Id cannot be null");
                }

                if (await context.UserWishlists.FirstOrDefaultAsync(w => w.ProductId == wishlistedItem.ProductId && w.UserId == userId) != null)
                {
                    throw new Exception("Already in the wishlist");
                }

                var wishlistItem = mapper.Map<UserWishlist>(wishlistedItem);
                await context.AddAsync(wishlistItem);
                await context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteWishlistItem(int? wishlistId)
        {
            try
            {
                if (wishlistId is null)
                {
                    throw new Exception("Wishlist Id cannot be null");
                }
                var wishlistItem = await context.UserWishlists.FirstOrDefaultAsync(w => w.UserWishlistId == wishlistId);
                context.UserWishlists.Remove(wishlistItem);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Some Internal Server Error Occurred");
            }
        }
    }
    }
