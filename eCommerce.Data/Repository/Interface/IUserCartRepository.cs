using eCommerce.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.Repository.Interface
{
    public interface IUserCartRepository
    {
        Task<List<GetUserCartDTO>> GetUserCartProducts(int? userId);
        Task<bool> AddProductInCart(int? userId, AddUserCartDTO cartItem);
        Task<bool> UpdateProductQuantityInCart(int? userCartId, UpdateUserCartDTO updatedCart);
        Task<bool> RemoveProductFromCart(int? userCartId);
        Task<int> GetUserCartCount(int? userId);
    }
}
