using Microsoft.EntityFrameworkCore;
using wishlist_tenyaeva.Data;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistResponseDto>> GetAllWishlistsAsync()
        {
            return await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.Wishes)
                .Select(w => new WishlistResponseDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CreatedAt = w.CreatedAt,
                    UserId = w.UserId,
                    OwnerName = w.User.Username,
                    WishesCount = w.Wishes.Count,
                    AvailableWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Available),
                    BookedWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Booked),
                    FulfilledWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Fulfilled)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<WishlistResponseDto>> GetUserWishlistsAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.Wishes)
                .Where(w => w.UserId == userId)
                .Select(w => new WishlistResponseDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    CreatedAt = w.CreatedAt,
                    UserId = w.UserId,
                    OwnerName = w.User.Username,
                    WishesCount = w.Wishes.Count,
                    AvailableWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Available),
                    BookedWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Booked),
                    FulfilledWishes = w.Wishes.Count(wi => wi.Status == WishStatus.Fulfilled)
                })
                .ToListAsync();
        }

        public async Task<WishlistResponseDto> GetWishlistByIdAsync(int id)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.User)
                .Include(w => w.Wishes)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlist == null)
                return null;

            return new WishlistResponseDto
            {
                Id = wishlist.Id,
                Name = wishlist.Name,
                Description = wishlist.Description,
                CreatedAt = wishlist.CreatedAt,
                UserId = wishlist.UserId,
                OwnerName = wishlist.User.Username,
                WishesCount = wishlist.Wishes.Count,
                AvailableWishes = wishlist.Wishes.Count(w => w.Status == WishStatus.Available),
                BookedWishes = wishlist.Wishes.Count(w => w.Status == WishStatus.Booked),
                FulfilledWishes = wishlist.Wishes.Count(w => w.Status == WishStatus.Fulfilled)
            };
        }

        public async Task<WishlistResponseDto> CreateWishlistAsync(CreateWishlistDto dto, int userId)
        {
            var wishlist = new Wishlist
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return await GetWishlistByIdAsync(wishlist.Id);
        }

        public async Task<WishlistResponseDto> UpdateWishlistAsync(UpdateWishlistDto dto, int userId)
        {
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == dto.Id);

            if (wishlist == null)
                throw new KeyNotFoundException("Список желаний не найден");

            if (wishlist.UserId != userId)
                throw new UnauthorizedAccessException("Вы не можете редактировать этот список");

            wishlist.Name = dto.Name;
            wishlist.Description = dto.Description;

            await _context.SaveChangesAsync();

            return await GetWishlistByIdAsync(wishlist.Id);
        }

        public async Task<bool> DeleteWishlistAsync(int id, int userId)
        {
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wishlist == null)
                return false;

            if (wishlist.UserId != userId)
                throw new UnauthorizedAccessException("Вы не можете удалить этот список");

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsWishlistOwnerAsync(int wishlistId, int userId)
        {
            var wishlist = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.Id == wishlistId);

            return wishlist != null && wishlist.UserId == userId;
        }
    }
}