using System.ComponentModel.DataAnnotations;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.DTO
{
    // Для создания нового желания
    public class CreateWishDto
    {
        [Required(ErrorMessage = "Название желания обязательно")]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Некорректная ссылка")]
        [MaxLength(500)]
        public string? Link { get; set; }

        [Range(0, 9999999.99, ErrorMessage = "Некорректная цена")]
        public decimal? Price { get; set; }

        [Required]
        public int WishlistId { get; set; }
    }

    // Для обновления желания
    public class UpdateWishDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Url]
        [MaxLength(500)]
        public string? Link { get; set; }

        [Range(0, 9999999.99)]
        public decimal? Price { get; set; }
    }

    // Для изменения статуса
    public class UpdateWishStatusDto
    {
        [Required]
        public int WishId { get; set; }

        [Required]
        public WishStatus Status { get; set; }
    }

    // Для ответа с данными желания
    public class WishResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public decimal? Price { get; set; }
        public WishStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WishlistId { get; set; }
        public string WishlistName { get; set; }
        public int? BookedByUserId { get; set; }
        public string? OwnerName { get; set; }
        public string? BookedByUserName { get; set; }
    }

    // Для бронирования желания
    public class BookWishDto
    {
        [Required]
        public int WishId { get; set; }
    }
}