import { Routes } from '@angular/router';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

// ============= ВРЕМЕННЫЕ КОМПОНЕНТЫ-ЗАГЛУШКИ (ОБЪЯВЛЯЕМ ПЕРВЫМИ) =============

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Все вишлисты</h1><p>Здесь будет отображаться список всех вишлистов</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class WishlistsPlaceholder {}

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Вход в систему</h1><p>Здесь будет форма входа</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class LoginPlaceholder {}

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Регистрация</h1><p>Здесь будет форма регистрации</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class RegisterPlaceholder {}

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Мои вишлисты</h1><p>Здесь будут мои списки желаний</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class MyWishlistsPlaceholder {}

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Мои бронирования</h1><p>Здесь будут желания, которые я забронировал</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class MyBookingsPlaceholder {}

@Component({
  standalone: true,
  imports: [CommonModule],
  template: '<div class="placeholder"><h1>Профиль пользователя</h1><p>Здесь будет информация о пользователе</p></div>',
  styles: ['.placeholder { padding: 20px; text-align: center; }']
})
export class ProfilePlaceholder {}

// ============= МАРШРУТЫ (ИСПОЛЬЗУЮТ КОМПОНЕНТЫ, ОБЪЯВЛЕННЫЕ ВЫШЕ) =============

export const routes: Routes = [
  { path: '', redirectTo: '/wishlists', pathMatch: 'full' },
  { path: 'wishlists', component: WishlistsPlaceholder, title: 'Все вишлисты' },
  { path: 'login', component: LoginPlaceholder, title: 'Вход' },
  { path: 'register', component: RegisterPlaceholder, title: 'Регистрация' },
  { path: 'my-wishlists', component: MyWishlistsPlaceholder, title: 'Мои списки' },
  { path: 'my-bookings', component: MyBookingsPlaceholder, title: 'Мои бронирования' },
  { path: 'profile', component: ProfilePlaceholder, title: 'Профиль' },
];