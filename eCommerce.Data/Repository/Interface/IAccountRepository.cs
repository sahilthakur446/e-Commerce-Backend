using eCommerce.Data.DTOs;


namespace eCommerce.Data.Repository.Interface
{
    public interface IAccountRepository
    {
        string JWTToken { get; set; }
        Task<bool> Register(RegisterDTO user);
        Task<bool> Login(LoginDTO user);
        Task<bool> CheckIfEmailExistsAsync(LoginDTO user);
        Task<bool> CheckIfEmailExistsAsync(RegisterDTO user);
        Task<UpdateUserDTO> GetUserInfo(int userId);
        Task<bool> EditUserInformation(int userId, UpdateUserDTO updatedUser);
        Task<bool> ChangeUserPassword(int UserId, ChangePasswordDTO password);
    }
}
