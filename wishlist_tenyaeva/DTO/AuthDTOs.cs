using System.ComponentModel.DataAnnotations;

namespace wishlist_tenyaeva.DTO
{
    // Для регистрации нового пользователя
    public class RegisterDto
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3 символа")]
        [MaxLength(50, ErrorMessage = "Максимальная длина 50 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен быть минимум 6 символов")]
        public string Password { get; set; }
    }

    // Для входа в систему
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    // Ответ после успешного входа
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}