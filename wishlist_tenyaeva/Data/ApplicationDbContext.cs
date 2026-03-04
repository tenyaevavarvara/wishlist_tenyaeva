using Microsoft.EntityFrameworkCore;
using wishlist_tenyaeva.Models;

namespace wishlist_tenyaeva.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Конструктор, который принимает параметры конфигурации
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet для каждой модели - таблицы в базе данных
        public DbSet<User> Users { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Wish> Wishes { get; set; }
        public DbSet<BookedWish> BookedWishes { get; set; }

        // Метод для настройки связей между таблицами
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // настройка связей и ограничений

            // 1. Связь User -> Wishlist (один ко многим)
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)                 // У одного списка один владелец
                .WithMany(u => u.Wishlists)          // У одного пользователя много списков
                .HasForeignKey(w => w.UserId)        // Внешний ключ - UserId в таблице Wishlists
                .OnDelete(DeleteBehavior.Cascade);   // При удалении пользователя удаляются все его списки

            // 2. Связь Wishlist -> Wish (один ко многим)
            modelBuilder.Entity<Wish>()
                .HasOne(w => w.Wishlist)              // У одного желания один список
                .WithMany(wl => wl.Wishes)            // У одного списка много желаний
                .HasForeignKey(w => w.WishlistId)     // Внешний ключ - WishlistId в таблице Wishes
                .OnDelete(DeleteBehavior.Cascade);    // При удалении списка удаляются все его желания

            // 4. Связь BookedWish -> User (кто забронировал)
            modelBuilder.Entity<BookedWish>()
                .HasOne(b => b.User)               // У записи о бронировании один пользователь
                .WithMany(u => u.BookedWishes)     // User.BookedWishes это ICollection<BookedWish>  // У пользователя много записей о бронированиях
                .HasForeignKey(b => b.UserId)      // Внешний ключ - UserId в таблице BookedWishes
                .OnDelete(DeleteBehavior.Restrict);// Запрет каскадного удаления

            // 5. Связь BookedWish -> Wish (что забронировали)
            modelBuilder.Entity<BookedWish>()
                .HasOne(b => b.Wish)                     // У записи о бронировании одно желание
                .WithMany()                              // У желания может быть много записей о бронировании (история)                
                .HasForeignKey(b => b.WishId)            // Внешний ключ - WishId в таблице BookedWishes
                .OnDelete(DeleteBehavior.Restrict);      // Запрет каскадного удаления
            
            // настройка индексов (для ускорения поиска)
            // Индекс для быстрого поиска пользователей по email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); // Email должен быть уникальным

            // Индекс для быстрого поиска пользователей по имени
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(); // Имя пользователя тоже уникально

            // Индекс для поиска желаний по статусу
            modelBuilder.Entity<Wish>()
                .HasIndex(w => w.Status);

            // Составной индекс для поиска желаний в конкретном списке
            modelBuilder.Entity<Wish>()
                .HasIndex(w => new { w.WishlistId, w.Status });

            // Индекс для поиска бронирований пользователя
            modelBuilder.Entity<BookedWish>()
                .HasIndex(b => b.UserId);

            // настройка ограничений для свойств
            // Настройка свойств для User
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Username)
                    .IsRequired()                    // Обязательное поле
                    .HasMaxLength(50);                // Максимальная длина 50 символов

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                    .IsRequired();
            });

            // Настройка свойств для Wishlist
            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.Property(w => w.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(w => w.Description)
                    .HasMaxLength(500);               // Описание необязательное, макс 500 символов
            });

            // Настройка свойств для Wish
            modelBuilder.Entity<Wish>(entity =>
            {
                entity.Property(w => w.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(w => w.Description)
                    .HasMaxLength(1000);

                entity.Property(w => w.Link)
                    .HasMaxLength(500);

                entity.Property(w => w.Price)
                    .HasPrecision(18, 2);             // 18 знаков всего, 2 после запятой (для цены)
            });

            // начальные данные
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEJ0...", 
                    Role = "Admin",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}