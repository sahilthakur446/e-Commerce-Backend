using eCommerce.Data.DTOs;
using eCommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
    {
    public interface IUserProfileRepository
        {
        Task<List<UserAddressDTO>> GetUsersAllAddress(int? userID);
        Task<UserAddressDTO> RetrieveDefaultUserAddress(int? userID);
        Task<bool> SaveUserAddress(int? userId, AddUserAddressDTO userAddress);
        }
    }
