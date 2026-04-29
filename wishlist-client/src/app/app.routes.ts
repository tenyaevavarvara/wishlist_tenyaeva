import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login';
import { RegisterComponent } from './features/register/register';
import { WishlistListComponent } from './features/wishlist-list/wishlist-list';
import { WishlistDetailComponent } from './features/wishlist-detail/wishlist-detail';
import { MyWishlistsComponent } from './features/my-wishlists/my-wishlists';
import { MyBookingsComponent } from './features/my-bookings/my-bookings';
import { ProfileComponent } from './features/profile/profile';
import { ReportsComponent } from './features/reports/reports';

export const routes: Routes = [
  { path: '', redirectTo: '/wishlists', pathMatch: 'full' },
  { path: 'wishlists', component: WishlistListComponent, title: 'Все вишлисты' },
  { path: 'wishlists/:id', component: WishlistDetailComponent, title: 'Вишлист' },
  { path: 'login', component: LoginComponent, title: 'Вход' },
  { path: 'register', component: RegisterComponent, title: 'Регистрация' },
  { path: 'my-wishlists', component: MyWishlistsComponent, title: 'Мои списки' },
  { path: 'my-bookings', component: MyBookingsComponent, title: 'Мои бронирования' },
  { path: 'profile', component: ProfileComponent, title: 'Профиль' },
  { path: 'reports', component: ReportsComponent, title: 'Отчеты' },
  { path: '**', redirectTo: '/wishlists' }
];