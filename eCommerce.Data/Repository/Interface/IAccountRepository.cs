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
    }
}
