using wishlist_tenyaeva.DTO;

namespace wishlist_tenyaeva.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> UserExistsAsync(string email);
    }
}