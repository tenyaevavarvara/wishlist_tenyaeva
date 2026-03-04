using System.ComponentModel.DataAnnotations;

namespace wishlist_tenyaeva.DTO
{
    // Для создания нового списка желаний
    public class CreateWishlistDto
    {
        [Required(ErrorMessage = "Название списка обязательно")]
        [MaxLength(100, ErrorMessage = "Максимальная длина 100 символов")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Максимальная длина 500 символов")]
        public string? Description { get; set; }
    }

    // Для обновления существующего списка
    public class UpdateWishlistDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }

    // Для ответа с данными списка
    public class WishlistResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string OwnerName { get; set; }
        public int WishesCount { get; set; }
        public int AvailableWishes { get; set; }
        public int BookedWishes { get; set; }
        public int FulfilledWishes { get; set; }
    }
}