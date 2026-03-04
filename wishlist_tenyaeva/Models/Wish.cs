using System;

namespace wishlist_tenyaeva.Models
{
    public enum WishStatus // статус желания
    {
        Available,    // свободно - 0
        Booked,       // забронировано - 1
        Fulfilled     // исполнено - 2
    }

    public class Wish
    {
        public int Id { get; set; }
        public string Title { get; set; } // обязательно
        public string? Description { get; set; } // необязательно, ?-может быть null
        public string? Link { get; set; } // ссылка на товар (не обязательно)
        public decimal? Price { get; set; } // цена (необязательно)
        public WishStatus Status { get; set; } = WishStatus.Available; // статус желания
        public DateTime CreatedAt { get; set; } = DateTime.Now; // дата создания
        
        // Внешние ключи
        public int WishlistId { get; set; } // список, к которому относи ся желание
        public Wishlist Wishlist { get; set; } // в каком списке находится

        public int? BookedByUserId { get; set; } // кто забронировал, если забронировано
        public User? BookedByUser { get; set; } // пользователь

        // Методы бизнес-логики
        // может ли пользователь забронировать желание
        public bool CanBook(User user)
        {
            if (this.Wishlist.UserId == user.Id)
                return false; // нельзя забронировать свое желание
            if (this.Status != WishStatus.Available)
                return false; // нельзя забронировать уже забронированное или исполненное желание
            return true;
        }

        // бронирование желания
        public void Book(User user)
        {
            if (!CanBook(user))
                throw new InvalidOperationException("Нельзя забронировать это желание");

            this.Status = WishStatus.Booked;
            this.BookedByUserId = user.Id;
            this.BookedByUser = user;
        }

        // отметить, что жедание исполнено
        public void MarkAsFulfilled(User user)
        {
            if (this.Wishlist.UserId != user.Id) // только владелец желания
                throw new UnauthorizedAccessException("Только владелец может отметить желание как исполненное");

            this.Status = WishStatus.Fulfilled;
        }
    }
}