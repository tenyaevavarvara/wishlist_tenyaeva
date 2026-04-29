import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipsModule } from '@angular/material/chips';
import { AuthService } from '../../services/auth';
import { WishlistService } from '../../services/wishlist';
import { BookingService, BookedWish } from '../../services/booking';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatChipsModule
  ],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class ProfileComponent implements OnInit {
  user: any = null;
  myWishlistsCount = 0;
  myBookingsCount = 0;
  loading = true;

  constructor(
    public authService: AuthService,
    private wishlistService: WishlistService,
    private bookingService: BookingService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.user = this.authService.getUser();
    
    if (!this.user) {
      this.router.navigate(['/login']);
      return;
    }
    
    this.loadStats();
  }

  loadStats(): void {
    this.loading = true;
    
    // Загружаем количество вишлистов пользователя
    this.wishlistService.getUserWishlists(this.user.id).subscribe({
      next: (data) => {
        this.myWishlistsCount = data.length;
      },
      error: (err) => {
        console.error('Ошибка загрузки вишлистов:', err);
      }
    });
    
    // Загружаем количество бронирований пользователя
    this.bookingService.getMyBookings().subscribe({
      next: (data) => {
        this.myBookingsCount = data.length;
      },
      error: (err) => {
        console.error('Ошибка загрузки бронирований:', err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
    this.snackBar.open('Вы вышли из системы', 'Закрыть', { duration: 3000 });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('ru-RU');
  }
}