using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wishlist_tenyaeva.Interfaces;

namespace wishlist_tenyaeva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // только авторизованные
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/bookings/my - мои забронированные желания
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookedWishes()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var bookedWishes = await _bookingService.GetUserBookedWishesAsync(userId);

                return Ok(bookedWishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/bookings/check/{wishId} - проверить, забронировано ли желание мной
        [HttpGet("check/{wishId}")]
        public async Task<IActionResult> CheckIfBookedByMe(int wishId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var isBooked = await _bookingService.IsWishBookedByUserAsync(wishId, userId);

                return Ok(new
                {
                    wishId = wishId,
                    isBookedByMe = isBooked
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }
    }
}