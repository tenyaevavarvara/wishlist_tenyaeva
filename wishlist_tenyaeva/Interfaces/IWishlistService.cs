using wishlist_tenyaeva.DTO;

namespace wishlist_tenyaeva.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistResponseDto>> GetAllWishlistsAsync();
        Task<IEnumerable<WishlistResponseDto>> GetUserWishlistsAsync(int userId);
        Task<WishlistResponseDto> GetWishlistByIdAsync(int id);
        Task<WishlistResponseDto> CreateWishlistAsync(CreateWishlistDto dto, int userId);
        Task<WishlistResponseDto> UpdateWishlistAsync(UpdateWishlistDto dto, int userId);
        Task<bool> DeleteWishlistAsync(int id, int userId);
        Task<bool> IsWishlistOwnerAsync(int wishlistId, int userId);
    }
}