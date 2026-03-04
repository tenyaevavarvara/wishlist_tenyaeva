using System;
using System.Collections.Generic;

namespace wishlist_tenyaeva.Models
{
    public class User
    {
        // ID пользователя (первичный ключ)
        public int Id { get; set; }

        // Имя пользователя для входа
        public string Username { get; set; }

        // Email пользователя
        public string Email { get; set; }

        // Хеш пароля (никогда не храним пароли в открытом виде!)
        public string PasswordHash { get; set; }

        // Роль пользователя (User или Admin)
        public string Role { get; set; } = "User"; // по умолчанию обычный пользователь

        // Дата регистрации
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Связь с другими таблицами
        // Один пользователь может иметь много списков желаний
        public ICollection<Wishlist> Wishlists { get; set; }

        // Желания, которые пользователь забронировал у других
        public ICollection<BookedWish> BookedWishes { get; set; }
    }
}