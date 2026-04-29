import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { AuthService } from '../../services/auth';
import { Wishlist } from '../../models/wishlist.model';

@Component({
  selector: 'app-wish-card',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule
  ],
  templateUrl: './wish-card.html',
  styleUrls: ['./wish-card.css']
})
export class WishCardComponent {
  @Input() wishlist!: Wishlist;
  @Output() delete = new EventEmitter<number>();

  constructor(public authService: AuthService) {}

  isOwner(): boolean {
    return this.authService.getUser()?.id === this.wishlist.userId;
  }

  onDelete(): void {
    this.delete.emit(this.wishlist.id);
  }
}