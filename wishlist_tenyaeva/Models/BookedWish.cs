using System;
using System.Collections.Generic;  // Для ICollection

namespace wishlist_tenyaeva.Models
{
    public class BookedWish
    {
        public int Id { get; set; }

        // Дата бронирования
        public DateTime BookedAt { get; set; } = DateTime.Now;

        // ID пользователя, который забронировал
        public int UserId { get; set; }

        // Навигационное свойство - пользователь
        public User User { get; set; }

        // ID желания, которое забронировали
        public int WishId { get; set; }

        // Навигационное свойство - желание
        public Wish Wish { get; set; }

        // МЕТОД БИЗНЕС-ЛОГИКИ
        public string GetBookingInfo()
        {
            return $"Пользователь {User?.Username} забронировал {Wish?.Title} {BookedAt:dd.MM.yyyy}";
        }
    }
}