import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { WishlistService } from '../../services/wishlist';
import { AuthService } from '../../services/auth';
import { Wishlist } from '../../models/wishlist.model';
import { WishlistFormComponent } from '../wishlist-form/wishlist-form';

@Component({
  selector: 'app-my-wishlists',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './my-wishlists.html',
  styleUrls: ['./my-wishlists.css']
})
export class MyWishlistsComponent implements OnInit {
  wishlists: Wishlist[] = [];
  loading = true;

  constructor(
    private wishlistService: WishlistService,
    private authService: AuthService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadMyWishlists();
  }

  loadMyWishlists(): void {
    this.loading = true;
    const userId = this.authService.getUser()?.id;
    
    if (!userId) {
      this.loading = false;
      return;
    }
    
    this.wishlistService.getUserWishlists(userId).subscribe({
      next: (data) => {
        this.wishlists = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Ошибка загрузки вишлистов:', err);
        this.snackBar.open('Не удалось загрузить ваши вишлисты', 'Закрыть', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(WishlistFormComponent, {
      width: '500px',
      data: { mode: 'create' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadMyWishlists();
        this.snackBar.open('Вишлист создан', 'Закрыть', { duration: 3000 });
      }
    });
  }

  openEditDialog(wishlist: Wishlist): void {
    const dialogRef = this.dialog.open(WishlistFormComponent, {
      width: '500px',
      data: { mode: 'edit', wishlist }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadMyWishlists();
        this.snackBar.open('Вишлист обновлен', 'Закрыть', { duration: 3000 });
      }
    });
  }

  deleteWishlist(id: number): void {
    if (confirm('Вы уверены, что хотите удалить этот вишлист? Все желания внутри будут также удалены.')) {
      this.wishlistService.delete(id).subscribe({
        next: () => {
          this.loadMyWishlists();
          this.snackBar.open('Вишлист удален', 'Закрыть', { duration: 3000 });
        },
        error: (err) => {
          console.error('Ошибка удаления:', err);
          this.snackBar.open('Не удалось удалить вишлист', 'Закрыть', { duration: 3000 });
        }
      });
    }
  }
}