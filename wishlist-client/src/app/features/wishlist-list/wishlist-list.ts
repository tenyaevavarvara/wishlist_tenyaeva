import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterLink } from '@angular/router';
import { WishlistService } from '../../services/wishlist';
import { AuthService } from '../../services/auth';
import { Wishlist } from '../../models/wishlist.model';
import { WishCardComponent } from '../../shared/wish-card/wish-card';

@Component({
  selector: 'app-wishlist-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatTableModule,
    MatSortModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    WishCardComponent
  ],
  templateUrl: './wishlist-list.html',
  styleUrls: ['./wishlist-list.css']
})
export class WishlistListComponent implements OnInit {
  wishlists: Wishlist[] = [];
  filteredWishlists: Wishlist[] = [];
  loading = true;
  searchTerm = '';
  viewMode: 'table' | 'cards' = 'table'; // 'table' или 'cards'
  
  // Для таблицы
  displayedColumns: string[] = ['name', 'ownerName', 'wishesCount', 'availableWishes', 'bookedWishes', 'actions'];
  sortField = 'name';
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(
    private wishlistService: WishlistService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadWishlists();
  }

  loadWishlists(): void {
    this.loading = true;
    this.wishlistService.getAll().subscribe({
      next: (data) => {
        this.wishlists = data;
        this.applyFilter();
        this.loading = false;
      },
      error: (err) => {
        console.error('Ошибка загрузки вишлистов:', err);
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    let filtered = [...this.wishlists];
    
    // Поиск
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase();
      filtered = filtered.filter(w => 
        w.name.toLowerCase().includes(term) || 
        w.ownerName.toLowerCase().includes(term)
      );
    }
    
    // Сортировка
    filtered.sort((a, b) => {
      let aVal = a[this.sortField as keyof Wishlist];
      let bVal = b[this.sortField as keyof Wishlist];
      
      if (typeof aVal === 'string' && typeof bVal === 'string') {
        return this.sortDirection === 'asc' 
          ? aVal.localeCompare(bVal) 
          : bVal.localeCompare(aVal);
      }
      
      if (typeof aVal === 'number' && typeof bVal === 'number') {
        return this.sortDirection === 'asc' ? aVal - bVal : bVal - aVal;
      }
      
      return 0;
    });
    
    this.filteredWishlists = filtered;
  }

  onSearchChange(): void {
    this.applyFilter();
  }

  onSortChange(sort: Sort): void {
    this.sortField = sort.active;
    this.sortDirection = sort.direction as 'asc' | 'desc';
    this.applyFilter();
  }

  toggleViewMode(): void {
    this.viewMode = this.viewMode === 'table' ? 'cards' : 'table';
  }

  deleteWishlist(id: number): void {
    if (confirm('Вы уверены, что хотите удалить этот список желаний?')) {
      this.wishlistService.delete(id).subscribe({
        next: () => {
          this.loadWishlists();
        },
        error: (err) => {
          console.error('Ошибка удаления:', err);
        }
      });
    }
  }
}