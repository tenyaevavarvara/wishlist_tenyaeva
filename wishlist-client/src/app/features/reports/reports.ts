import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { AuthService } from '../../services/auth';
import { WishlistService } from '../../services/wishlist';
import { BookingService } from '../../services/booking';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTableModule
  ],
  templateUrl: './reports.html',
  styleUrls: ['./reports.css']
})
export class ReportsComponent implements OnInit {
  loading = true;
  wishlistStats: any = null;
  bookingStats: any[] = [];
  
  // Изменено: убран столбец totalBookings, оставлен только uniqueWishes
  displayedColumns: string[] = ['userId', 'userName', 'uniqueWishes', 'lastBookingDate'];

  constructor(
    private authService: AuthService,
    private wishlistService: WishlistService,
    private bookingService: BookingService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAdmin()) {
      this.snackBar.open('Доступ запрещен. Только для администраторов.', 'Закрыть', { duration: 3000 });
      return;
    }
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    
    this.wishlistService.getWishlistStats().subscribe({
      next: (data) => {
        this.wishlistStats = data;
      },
      error: (err) => {
        console.error('Ошибка загрузки статистики вишлистов:', err);
      }
    });
    
    this.bookingService.getBookingStatistics().subscribe({
      next: (data) => {
        this.bookingStats = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Ошибка загрузки статистики бронирований:', err);
        this.loading = false;
      }
    });
  }
}