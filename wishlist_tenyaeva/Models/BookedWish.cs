using System;
using System.Collections.Generic;

namespace wishlist_tenyaeva.Models
{
    public class BookedWish
    {
        public int Id { get; set; }

        // Дата бронирования
        public DateTime BookedAt { get; set; } = DateTime.Now;

        // ID пользователя, который забронировал
        public int UserId { get; set; }

        public User User { get; set; }

        // ID желания, которое забронировали
        public int WishId { get; set; }

        public Wish Wish { get; set; }

        // метод бизнес логики
        public string GetBookingInfo()
        {
            return $"Пользователь {User?.Username} забронировал {Wish?.Title} {BookedAt:dd.MM.yyyy}";
        }
    }
}