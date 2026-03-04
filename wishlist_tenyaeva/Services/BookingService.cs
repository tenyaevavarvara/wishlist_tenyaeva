using Microsoft.EntityFrameworkCore;
using wishlist_tenyaeva.Data;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // LINQ запрос с объединением данных
        public async Task<IEnumerable<BookedWishResponseDto>> GetUserBookedWishesAsync(int userId)
        {
            return await _context.BookedWishes
                .Include(b => b.Wish)
                    .ThenInclude(w => w.Wishlist)
                        .ThenInclude(wl => wl.User)
                .Where(b => b.UserId == userId)
                .Select(b => new BookedWishResponseDto
                {
                    Id = b.Id,
                    BookedAt = b.BookedAt,
                    WishId = b.WishId,
                    WishTitle = b.Wish.Title,
                    WishPrice = b.Wish.Price,
                    OwnerId = b.Wish.Wishlist.UserId,
                    OwnerName = b.Wish.Wishlist.User.Username,
                    WishlistName = b.Wish.Wishlist.Name
                })
                .ToListAsync();
        }

        // LINQ запрос с группировкой (для статистики)
        public async Task<IEnumerable<object>> GetBookingStatisticsAsync()
        {
            var stats = await _context.BookedWishes
                .GroupBy(b => b.Wish.Wishlist.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    UserName = g.First().Wish.Wishlist.User.Username,
                    TotalBookings = g.Count(),
                    UniqueWishes = g.Select(b => b.WishId).Distinct().Count(),
                    LastBookingDate = g.Max(b => b.BookedAt),
                    Wishes = g.Select(b => new
                    {
                        b.Wish.Title,
                        b.BookedAt,
                        OwnerName = b.Wish.Wishlist.User.Username
                    }).ToList()
                })
                .ToListAsync();

            return stats;
        }

        public async Task<bool> IsWishBookedByUserAsync(int wishId, int userId)
        {
            return await _context.BookedWishes
                .AnyAsync(b => b.WishId == wishId && b.UserId == userId);
        }
    }
}