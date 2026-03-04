using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wishlist_tenyaeva.DTO;
using wishlist_tenyaeva.Interfaces;

namespace wishlist_tenyaeva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistsController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // GET: api/wishlists - получить все списки желаний (доступно всем)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllWishlists()
        {
            try
            {
                var wishlists = await _wishlistService.GetAllWishlistsAsync();
                return Ok(wishlists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/wishlists/user/{userId} - получить списки конкретного пользователя
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserWishlists(int userId)
        {
            try
            {
                var wishlists = await _wishlistService.GetUserWishlistsAsync(userId);
                return Ok(wishlists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // GET: api/wishlists/{id} - получить конкретный список по ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetWishlistById(int id)
        {
            try
            {
                var wishlist = await _wishlistService.GetWishlistByIdAsync(id);

                if (wishlist == null)
                    return NotFound(new { message = "Список желаний не найден" });

                return Ok(wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // POST: api/wishlists - создать новый список (только авторизованные)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWishlist(CreateWishlistDto dto)
        {
            try
            {
                // Получаем ID текущего пользователя из токена
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var wishlist = await _wishlistService.CreateWishlistAsync(dto, userId);

                return CreatedAtAction(nameof(GetWishlistById), new { id = wishlist.Id }, wishlist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // PUT: api/wishlists/{id} - обновить список (только владелец)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWishlist(int id, UpdateWishlistDto dto)
        {
            try
            {
                // Проверяем, что ID в маршруте совпадает с ID в теле запроса
                if (id != dto.Id)
                    return BadRequest(new { message = "ID в маршруте не совпадает с ID в теле запроса" });

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var updatedWishlist = await _wishlistService.UpdateWishlistAsync(dto, userId);

                return Ok(updatedWishlist);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                //return Forbid(ex.Message);
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Внутренняя ошибка сервера: {ex.Message}" });
            }
        }

        // DELETE: api/wishlists/{id} - удалить список (только владелец)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _wishlistService.DeleteWishlistAsync(id, userId);

                if (!result)
                    return NotFound(new { message = "Список желаний не найден" });

                return NoContent(); // 204 - успешное удаление
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