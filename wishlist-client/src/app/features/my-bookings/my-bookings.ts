import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { BookingService, BookedWish } from '../../services/booking';
import { WishService } from '../../services/wish';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  templateUrl: './my-bookings.html',
  styleUrls: ['./my-bookings.css']
})
export class MyBookingsComponent implements OnInit {
  bookings: BookedWish[] = [];
  filteredBookings: BookedWish[] = [];
  loading = true;

  constructor(
    private bookingService: BookingService,
    private wishService: WishService,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading = true;
    this.bookingService.getMyBookings().subscribe({
      next: (data) => {
        this.bookings = data;
        this.filterActiveBookings();
        this.loading = false;
      },
      error: (err) => {
        console.error('Ошибка загрузки бронирований:', err);
        this.snackBar.open('Не удалось загрузить бронирования', 'Закрыть', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  // Фильтрация: оставляем только актуальные бронирования
  filterActiveBookings(): void {
    // Получаем все ID желаний из бронирований
    const wishIds = this.bookings.map(b => b.wishId);
    
    if (wishIds.length === 0) {
      this.filteredBookings = [];
      return;
    }
    
    // Для каждого желания проверяем его статус на бэкенде
    const checks = wishIds.map(wishId => 
      this.wishService.getById(wishId).toPromise().catch(() => null)
    );
    
    Promise.all(checks).then(results => {
      // Оставляем только те бронирования, у которых желание действительно забронировано
      this.filteredBookings = this.bookings.filter((booking, index) => {
        const wish = results[index];
        return wish && wish.status === 1; // 1 = Booked
      });
      
      // Если есть расхождения, обновляем локальные данные
      if (this.filteredBookings.length !== this.bookings.length) {
        console.log(`Отфильтровано ${this.bookings.length - this.filteredBookings.length} неактуальных бронирований`);
      }
    });
  }

  cancelBooking(wishId: number): void {
    if (confirm('Отменить бронирование? Желание снова станет доступным для других.')) {
      this.wishService.unbook(wishId).subscribe({
        next: () => {
          // Удаляем отмененное бронирование из локального списка
          this.bookings = this.bookings.filter(b => b.wishId !== wishId);
          this.filteredBookings = this.filteredBookings.filter(b => b.wishId !== wishId);
          
          this.snackBar.open('Бронирование отменено', 'Закрыть', { duration: 3000 });
          
          // Если список стал пустым, показываем сообщение
          if (this.filteredBookings.length === 0) {
          }
        },
        error: (err) => {
          console.error('Ошибка отмены:', err);
          this.snackBar.open('Не удалось отменить бронирование', 'Закрыть', { duration: 3000 });
        }
      });
    }
  }

  formatPrice(price?: number): string {
    if (price === undefined || price === null) return 'Цена не указана';
    return new Intl.NumberFormat('ru-RU', { style: 'currency', currency: 'RUB' }).format(price);
  }
}