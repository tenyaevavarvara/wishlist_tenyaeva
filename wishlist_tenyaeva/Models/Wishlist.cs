using System;
using System.Collections.Generic;

namespace wishlist_tenyaeva.Models
{
    public class Wishlist
    {
        // ID списка желаний
        public int Id { get; set; }

        // Название списка
        public string Name { get; set; }

        // Описание (необязательное)
        public string? Description { get; set; }

        // Дата создания
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Внешний ключ - ссылка на пользователя-владельца
        public int UserId { get; set; }

        // Владелец списка
        public User User { get; set; }

        // Список желаний в этом Wishlist
        public ICollection<Wish> Wishes { get; set; }

        // метод бизнес логики
        // Проверка, может ли пользователь добавить желание в этот список
        public bool CanAddWish(User currentUser)
        {
            // Только владелец может добавлять желания
            return currentUser.Id == this.UserId;
        }

        // метод бизнес-логики
        public int GetAvailableWishesCount()
        {
            if (Wishes == null) return 0;
            return Wishes.Count(w => w.Status == WishStatus.Available);
        }
    }
}