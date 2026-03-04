using wishlist_tenyaeva.DTO;

namespace wishlist_tenyaeva.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookedWishResponseDto>> GetUserBookedWishesAsync(int userId);
        Task<IEnumerable<object>> GetBookingStatisticsAsync(); // LINQ запрос с группировкой
        Task<bool> IsWishBookedByUserAsync(int wishId, int userId);
    }
}