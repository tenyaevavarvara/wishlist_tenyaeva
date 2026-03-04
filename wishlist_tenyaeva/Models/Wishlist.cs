using System;
using System.Collections.Generic;

namespace wishlist_tenyaeva.Models
{
    public class Wishlist
    {
        // ID списка желаний
        public int Id { get; set; }

        // Название списка (например, "День рождения 2026")
        public string Name { get; set; }

        // Описание/повод для списка (необязательное)
        public string? Description { get; set; }

        // Дата создания
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Внешний ключ - ссылка на пользователя-владельца
        public int UserId { get; set; }

        // Навигационное свойство - сам пользователь
        public User User { get; set; }

        // Список желаний в этом Wishlist
        public ICollection<Wish> Wishes { get; set; }

        // МЕТОД БИЗНЕС-ЛОГИКИ (обязательное требование ТЗ)
        // Проверка, может ли пользователь добавить желание в этот список
        public bool CanAddWish(User currentUser)
        {
            // Только владелец может добавлять желания
            return currentUser.Id == this.UserId;
        }

        // Еще один метод бизнес-логики
        public int GetAvailableWishesCount()
        {
            if (Wishes == null) return 0;
            return Wishes.Count(w => w.Status == WishStatus.Available);
        }
    }
}