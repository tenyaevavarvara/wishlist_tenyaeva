using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wishlist_tenyaeva.Interfaces;

namespace wishlist_tenyaeva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Только администраторы
    public class ReportsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IWishlistService _wishlistService;
        private readonly IWishService _wishService;

        public ReportsController(
            IBookingService bookingService,
            IWishlistService wishlistService,
            IWishService wishService)
        {
            _bookingService = bookingService;
            _wishlistService = wishlistService;
            _wishService = wishService;
        }

        // GET: api/reports/booking-statistics - статистика бронирований
        [HttpGet("booking-statistics")]
        public async Task<IActionResult> GetBookingStatistics()
        {
            try
            {
                var statistics = await _bookingService.GetBookingStatisticsAsync();
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/reports/popular-wishes - самые популярные желания
        [HttpGet("popular-wishes")]
        public async Task<IActionResult> GetPopularWishes()
        {
            try
            {
                // Здесь можно добавить метод в IWishService
                // Пока возвращаем тестовые данные
                return Ok(new { message = "Метод в разработке" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/reports/user-activity - активность пользователей
        [HttpGet("user-activity")]
        public async Task<IActionResult> GetUserActivity()
        {
            try
            {
                // Статистика по пользователям
                return Ok(new { message = "Метод в разработке" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/reports/wishlist-stats - общая статистика по вишлистам
        [HttpGet("wishlist-stats")]
        public async Task<IActionResult> GetWishlistStats()
        {
            try
            {
                var allWishlists = await _wishlistService.GetAllWishlistsAsync();

                var stats = new
                {
                    TotalWishlists = allWishlists.Count(),
                    TotalWishes = allWishlists.Sum(w => w.WishesCount),
                    TotalAvailable = allWishlists.Sum(w => w.AvailableWishes),
                    TotalBooked = allWishlists.Sum(w => w.BookedWishes),
                    TotalFulfilled = allWishlists.Sum(w => w.FulfilledWishes),
                    AverageWishesPerList = allWishlists.Average(w => w.WishesCount)
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }
    }
}