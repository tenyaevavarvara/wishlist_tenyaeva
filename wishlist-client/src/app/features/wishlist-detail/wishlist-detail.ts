import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { WishlistService } from '../../services/wishlist';
import { WishService } from '../../services/wish';
import { AuthService } from '../../services/auth';
import { Wishlist } from '../../models/wishlist.model';
import { Wish, WishStatus, WishStatusLabels, WishStatusColors } from '../../models/wish.model';
import { WishFormComponent } from '../wish-form/wish-form';

@Component({
  selector: 'app-wishlist-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './wishlist-detail.html',
  styleUrls: ['./wishlist-detail.css']
})
export class WishlistDetailComponent implements OnInit {
  wishlist: Wishlist | null = null;
  wishes: Wish[] = [];
  loading = true;
  isOwner = false;
  
  WishStatus = WishStatus;
  WishStatusLabels = WishStatusLabels;
  WishStatusColors = WishStatusColors;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private wishlistService: WishlistService,
    private wishService: WishService,
    public authService: AuthService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadWishlist(id);
      this.loadWishes(id);
    }
  }

  loadWishlist(id: number): void {
    this.wishlistService.getById(id).subscribe({
      next: (data) => {
        this.wishlist = data;
        this.isOwner = this.authService.getUser()?.id === data.userId;
      },
      error: (err) => {
        console.error('Ошибка загрузки вишлиста:', err);
        this.snackBar.open('Вишлист не найден', 'Закрыть', { duration: 3000 });
        this.router.navigate(['/wishlists']);
      }
    });
  }

  loadWishes(wishlistId: number): void {
    this.loading = true;
    this.wishService.getByWishlist(wishlistId).subscribe({
      next: (data) => {
        this.wishes = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Ошибка загрузки желаний:', err);
        this.loading = false;
      }
    });
  }

  openAddWishDialog(): void {
    const dialogRef = this.dialog.open(WishFormComponent, {
      width: '600px',
      data: { wishlistId: this.wishlist?.id, mode: 'create' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.wishlist) {
        this.loadWishes(this.wishlist.id);
        this.snackBar.open('Желание добавлено', 'Закрыть', { duration: 3000 });
      }
    });
  }

  editWish(wish: Wish): void {
    const dialogRef = this.dialog.open(WishFormComponent, {
      width: '600px',
      data: { wish, mode: 'edit' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.wishlist) {
        this.loadWishes(this.wishlist.id);
        this.snackBar.open('Желание обновлено', 'Закрыть', { duration: 3000 });
      }
    });
  }

  deleteWish(id: number): void {
    if (confirm('Вы уверены, что хотите удалить это желание?')) {
      this.wishService.delete(id).subscribe({
        next: () => {
          if (this.wishlist) {
            this.loadWishes(this.wishlist.id);
          }
          this.snackBar.open('Желание удалено', 'Закрыть', { duration: 3000 });
        },
        error: (err) => {
          console.error('Ошибка удаления:', err);
          this.snackBar.open('Ошибка при удалении', 'Закрыть', { duration: 3000 });
        }
      });
    }
  }

  bookWish(id: number): void {
    this.wishService.book(id).subscribe({
      next: () => {
        if (this.wishlist) {
          this.loadWishes(this.wishlist.id);
        }
        this.snackBar.open('Желание забронировано!', 'Закрыть', { duration: 3000 });
      },
      error: (err) => {
        console.error('Ошибка бронирования:', err);
        this.snackBar.open(err.error?.message || 'Не удалось забронировать', 'Закрыть', { duration: 3000 });
      }
    });
  }

  unbookWish(id: number): void {
    this.wishService.unbook(id).subscribe({
      next: () => {
        if (this.wishlist) {
          this.loadWishes(this.wishlist.id);
        }
        this.snackBar.open('Бронирование отменено', 'Закрыть', { duration: 3000 });
      },
      error: (err) => {
        console.error('Ошибка отмены бронирования:', err);
        this.snackBar.open('Ошибка при отмене бронирования', 'Закрыть', { duration: 3000 });
      }
    });
  }

  updateStatus(wish: Wish, status: WishStatus): void {
    this.wishService.updateStatus(wish.id, status).subscribe({
      next: () => {
        if (this.wishlist) {
          this.loadWishes(this.wishlist.id);
        }
        this.snackBar.open(`Статус изменен на "${WishStatusLabels[status]}"`, 'Закрыть', { duration: 3000 });
      },
      error: (err) => {
        console.error('Ошибка изменения статуса:', err);
        this.snackBar.open('Ошибка при изменении статуса', 'Закрыть', { duration: 3000 });
      }
    });
  }

  // Методы для проверки прав
  canBook(wish: Wish): boolean {
    return !this.isOwner &&                           // не владелец
           this.authService.isAuthenticated() && 
           wish.status === WishStatus.Available;
  }

  canUnbook(wish: Wish): boolean {
    return !this.isOwner &&                           // не владелец
           this.authService.isAuthenticated() && 
           wish.status === WishStatus.Booked && 
           wish.bookedByUserId === this.authService.getUser()?.id;
  }

  canEdit(wish: Wish): boolean {
    return this.isOwner;                               // только владелец
  }

  canDelete(wish: Wish): boolean {
    return this.isOwner;                               // только владелец
  }

  canChangeStatus(wish: Wish): boolean {
    return this.isOwner;  // только владелец
  }

  // Методы для отображения информации (с учетом whoIsViewer)
  shouldShowStatus(): boolean {
    return !this.isOwner;  // Показываем статус ТОЛЬКО НЕ владельцу
  }

  shouldShowBookedBy(): boolean {
    return !this.isOwner &&  // Показываем кто забронировал ТОЛЬКО НЕ владельцу
           this.wishes.some(w => w.bookedByUserName);
  }
}