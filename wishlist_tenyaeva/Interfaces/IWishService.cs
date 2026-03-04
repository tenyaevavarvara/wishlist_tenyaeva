using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Interfaces
{
    public interface IWishService
    {
        Task<IEnumerable<WishResponseDto>> GetWishesByWishlistAsync(int wishlistId);
        Task<WishResponseDto> GetWishByIdAsync(int id);
        Task<WishResponseDto> CreateWishAsync(CreateWishDto dto, int userId);
        Task<WishResponseDto> UpdateWishAsync(UpdateWishDto dto, int userId);
        Task<bool> DeleteWishAsync(int id, int userId);
        Task<bool> BookWishAsync(int wishId, int userId);
        Task<bool> UnbookWishAsync(int wishId, int userId);
        Task<bool> UpdateWishStatusAsync(int wishId, WishStatus status, int userId);
        Task<IEnumerable<WishResponseDto>> GetAvailableWishesAsync(); // LINQ запрос
        Task<IEnumerable<object>> GetPopularWishesAsync(int count = 10);
    }
}