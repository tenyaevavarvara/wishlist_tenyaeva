using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishesController : ControllerBase
    {
        private readonly IWishService _wishService;
        private readonly IWishlistService _wishlistService;

        public WishesController(IWishService wishService, IWishlistService wishlistService)
        {
            _wishService = wishService;
            _wishlistService = wishlistService;
        }

        // GET: api/wishes/wishlist/{wishlistId} - получить все желания в списке
        [HttpGet("wishlist/{wishlistId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWishesByWishlist(int wishlistId)
        {
            try
            {
                var wishes = await _wishService.GetWishesByWishlistAsync(wishlistId);
                return Ok(wishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/wishes/available - все доступные желания (LINQ запрос)
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableWishes()
        {
            try
            {
                var wishes = await _wishService.GetAvailableWishesAsync();
                return Ok(wishes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/wishes/{id} - получить конкретное желание
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWishById(int id)
        {
            try
            {
                var wish = await _wishService.GetWishByIdAsync(id);

                if (wish == null)
                    return NotFound(new { message = "Желание не найдено" });

                return Ok(wish);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // POST: api/wishes - создать новое желание (только авторизованные)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWish(CreateWishDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Проверяем, что список существует
                var wishlist = await _wishlistService.GetWishlistByIdAsync(dto.WishlistId);
                if (wishlist == null)
                    return BadRequest(new { message = "Указанный список желаний не существует" });

                var wish = await _wishService.CreateWishAsync(dto, userId);

                return CreatedAtAction(nameof(GetWishById), new { id = wish.Id }, wish);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // PUT: api/wishes/{id} - обновить желание (только владелец)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWish(int id, UpdateWishDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { message = "ID в маршруте не совпадает с ID в теле запроса" });

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var updatedWish = await _wishService.UpdateWishAsync(dto, userId);

                return Ok(updatedWish);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // PATCH: api/wishes/{id}/book - забронировать желание
        [HttpPatch("{id}/book")]
        [Authorize]
        public async Task<IActionResult> BookWish(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _wishService.BookWishAsync(id, userId);

                return Ok(new { message = "Желание успешно забронировано" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // PATCH: api/wishes/{id}/unbook - отменить бронирование
        [HttpPatch("{id}/unbook")]
        [Authorize]
        public async Task<IActionResult> UnbookWish(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _wishService.UnbookWishAsync(id, userId);

                return Ok(new { message = "Бронирование отменено" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // PATCH: api/wishes/{id}/status - изменить статус (только владелец)
        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateWishStatus(int id, [FromBody] WishStatus status)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _wishService.UpdateWishStatusAsync(id, status, userId);

                return Ok(new { message = $"Статус изменен на {status}" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // DELETE: api/wishes/{id} - удалить желание (только владелец)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWish(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _wishService.DeleteWishAsync(id, userId);

                if (!result)
                    return NotFound(new { message = "Желание не найдено" });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }
    }
}