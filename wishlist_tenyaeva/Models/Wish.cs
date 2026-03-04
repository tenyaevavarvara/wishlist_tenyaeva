using System;

namespace wishlist_tenyaeva.Models
{
    public enum WishStatus
    {
        Available,    // свободно
        Booked,       // забронировано
        Fulfilled     // исполнено
    }

    public class Wish
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public decimal? Price { get; set; }
        public WishStatus Status { get; set; } = WishStatus.Available;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }

        // 👇 ДОБАВЬТЕ ЭТИ ДВЕ СТРОКИ
        public int? BookedByUserId { get; set; }
        public User? BookedByUser { get; set; }

        // Методы бизнес-логики
        public bool CanBook(User user)
        {
            if (this.Wishlist.UserId == user.Id)
                return false;
            if (this.Status != WishStatus.Available)
                return false;
            return true;
        }

        public void Book(User user)
        {
            if (!CanBook(user))
                throw new InvalidOperationException("Нельзя забронировать это желание");

            this.Status = WishStatus.Booked;
            this.BookedByUserId = user.Id;
            this.BookedByUser = user;
        }

        public void MarkAsFulfilled(User user)
        {
            if (this.Wishlist.UserId != user.Id)
                throw new UnauthorizedAccessException("Только владелец может отметить желание как исполненное");

            this.Status = WishStatus.Fulfilled;
        }
    }
}