using Microsoft.EntityFrameworkCore;
using wishlist_tenyaeva.Data;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Services
{
    public class WishService : IWishService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWishlistService _wishlistService;

        public WishService(ApplicationDbContext context, IWishlistService wishlistService)
        {
            _context = context;
            _wishlistService = wishlistService;
        }

        public async Task<IEnumerable<WishResponseDto>> GetWishesByWishlistAsync(int wishlistId)
        {
            return await _context.Wishes
                .Include(w => w.Wishlist)
                    .ThenInclude(wl => wl.User)
                .Include(w => w.BookedByUser)
                .Where(w => w.WishlistId == wishlistId)
                .Select(w => new WishResponseDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                    Link = w.Link,
                    Price = w.Price,
                    Status = w.Status,
                    CreatedAt = w.CreatedAt,
                    WishlistId = w.WishlistId,
                    WishlistName = w.Wishlist.Name,
                    BookedByUserId = w.BookedByUserId,
                    BookedByUserName = w.BookedByUser != null ? w.BookedByUser.Username : null
                })
                .ToListAsync();
        }

        public async Task<WishResponseDto> GetWishByIdAsync(int id)
        {
            var wish = await _context.Wishes
                .Include(w => w.Wishlist)
                    .ThenInclude(wl => wl.User)
                .Include(w => w.BookedByUser)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wish == null)
                return null;

            return new WishResponseDto
            {
                Id = wish.Id,
                Title = wish.Title,
                Description = wish.Description,
                Link = wish.Link,
                Price = wish.Price,
                Status = wish.Status,
                CreatedAt = wish.CreatedAt,
                WishlistId = wish.WishlistId,
                WishlistName = wish.Wishlist.Name,
                OwnerName = wish.Wishlist.User?.Username,
                BookedByUserId = wish.BookedByUserId,
                BookedByUserName = wish.BookedByUser?.Username
            };
        }

        public async Task<WishResponseDto> CreateWishAsync(CreateWishDto dto, int userId)
        {
            // Проверяем, что пользователь - владелец списка
            if (!await _wishlistService.IsWishlistOwnerAsync(dto.WishlistId, userId))
                throw new UnauthorizedAccessException("Вы не можете добавлять желания в этот список");

            var wish = new Wish
            {
                Title = dto.Title,
                Description = dto.Description,
                Link = dto.Link,
                Price = dto.Price,
                WishlistId = dto.WishlistId,
                Status = WishStatus.Available,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishes.Add(wish);
            await _context.SaveChangesAsync();

            return await GetWishByIdAsync(wish.Id);
        }

        public async Task<WishResponseDto> UpdateWishAsync(UpdateWishDto dto, int userId)
        {
            var wish = await _context.Wishes
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(w => w.Id == dto.Id);

            if (wish == null)
                throw new KeyNotFoundException("Желание не найдено");

            // Проверяем, что пользователь - владелец списка
            if (wish.Wishlist.UserId != userId)
                throw new UnauthorizedAccessException("Вы не можете редактировать это желание");

            wish.Title = dto.Title;
            wish.Description = dto.Description;
            wish.Link = dto.Link;
            wish.Price = dto.Price;

            await _context.SaveChangesAsync();

            return await GetWishByIdAsync(wish.Id);
        }

        public async Task<bool> DeleteWishAsync(int id, int userId)
        {
            var wish = await _context.Wishes
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wish == null)
                return false;

            if (wish.Wishlist.UserId != userId)
                throw new UnauthorizedAccessException("Вы не можете удалить это желание");

            _context.Wishes.Remove(wish);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BookWishAsync(int wishId, int userId)
        {
            var wish = await _context.Wishes
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(w => w.Id == wishId);

            if (wish == null)
                throw new KeyNotFoundException("Желание не найдено");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Пользователь не найден");

            // Используем метод бизнес-логики из модели
            if (!wish.CanBook(user))
                throw new InvalidOperationException("Нельзя забронировать это желание");

            // Вызываем метод модели для бронирования
            wish.Book(user);

            // Добавляем запись в историю бронирований
            _context.BookedWishes.Add(new BookedWish
            {
                UserId = userId,
                WishId = wishId,
                BookedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnbookWishAsync(int wishId, int userId)
        {
            var wish = await _context.Wishes
                .FirstOrDefaultAsync(w => w.Id == wishId);

            if (wish == null)
                throw new KeyNotFoundException("Желание не найдено");

            // Проверяем, что именно этот пользователь забронировал
            if (wish.BookedByUserId != userId)
                throw new UnauthorizedAccessException("Вы не можете отменить чужое бронирование");

            wish.Status = WishStatus.Available;
            wish.BookedByUserId = null;
            wish.BookedByUser = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateWishStatusAsync(int wishId, WishStatus status, int userId)
        {
            var wish = await _context.Wishes
                .Include(w => w.Wishlist)
                .FirstOrDefaultAsync(w => w.Id == wishId);

            if (wish == null)
                throw new KeyNotFoundException("Желание не найдено");

            // Только владелец может менять статус (кроме бронирования)
            if (wish.Wishlist.UserId != userId)
                throw new UnauthorizedAccessException("Вы не можете изменить статус этого желания");

            wish.Status = status;

            // Если статус меняется на Available, сбрасываем бронирование
            if (status == WishStatus.Available)
            {
                wish.BookedByUserId = null;
                wish.BookedByUser = null;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // LINQ запрос - все доступные желания
        public async Task<IEnumerable<WishResponseDto>> GetAvailableWishesAsync()
        {
            return await _context.Wishes
                .Include(w => w.Wishlist)
                    .ThenInclude(wl => wl.User)
                .Where(w => w.Status == WishStatus.Available)
                .Select(w => new WishResponseDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                    Link = w.Link,
                    Price = w.Price,
                    Status = w.Status,
                    CreatedAt = w.CreatedAt,
                    WishlistId = w.WishlistId,
                    WishlistName = w.Wishlist.Name,
                    OwnerName = w.Wishlist.User.Username
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetPopularWishesAsync(int count = 10)
        {
            return await _context.BookedWishes
                .GroupBy(b => b.WishId)
                .Select(g => new
                {
                    WishId = g.Key,
                    WishTitle = g.First().Wish.Title,
                    BookingCount = g.Count(),
                    WishlistName = g.First().Wish.Wishlist.Name,
                    OwnerName = g.First().Wish.Wishlist.User.Username
                })
                .OrderByDescending(x => x.BookingCount)
                .Take(count)
                .ToListAsync();
        }
    }
}