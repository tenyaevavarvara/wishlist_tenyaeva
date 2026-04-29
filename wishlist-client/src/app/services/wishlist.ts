import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Wishlist, CreateWishlistDto, UpdateWishlistDto } from '../models/wishlist.model';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private apiUrl = 'https://localhost:7178/api/wishlists';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Wishlist[]> {
    return this.http.get<Wishlist[]>(this.apiUrl);
  }

  getUserWishlists(userId: number): Observable<Wishlist[]> {
    return this.http.get<Wishlist[]>(`${this.apiUrl}/user/${userId}`);
  }

  getById(id: number): Observable<Wishlist> {
    return this.http.get<Wishlist>(`${this.apiUrl}/${id}`);
  }

  create(data: CreateWishlistDto): Observable<Wishlist> {
    return this.http.post<Wishlist>(this.apiUrl, data);
  }

  update(data: UpdateWishlistDto): Observable<Wishlist> {
    return this.http.put<Wishlist>(`${this.apiUrl}/${data.id}`, data);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getWishlistStats(): Observable<any> {
    return this.http.get<any>(`https://localhost:7178/api/reports/wishlist-stats`);
  }
}