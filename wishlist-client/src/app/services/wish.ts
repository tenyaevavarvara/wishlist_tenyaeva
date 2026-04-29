import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Wish, CreateWishDto, WishStatus } from '../models/wish.model';

@Injectable({
  providedIn: 'root'
})
export class WishService {
  private apiUrl = 'https://localhost:7178/api/wishes';

  constructor(private http: HttpClient) {}

  getByWishlist(wishlistId: number): Observable<Wish[]> {
    return this.http.get<Wish[]>(`${this.apiUrl}/wishlist/${wishlistId}`);
  }

  getAvailable(): Observable<Wish[]> {
    return this.http.get<Wish[]>(`${this.apiUrl}/available`);
  }

  getById(id: number): Observable<Wish> {
    return this.http.get<Wish>(`${this.apiUrl}/${id}`);
  }

  create(data: CreateWishDto): Observable<Wish> {
    return this.http.post<Wish>(this.apiUrl, data);
  }

  update(id: number, data: Partial<Wish>): Observable<Wish> {
    return this.http.put<Wish>(`${this.apiUrl}/${id}`, { id, ...data });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  book(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/book`, {});
  }

  unbook(id: number): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/unbook`, {});
  }

  updateStatus(id: number, status: WishStatus): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/status`, status);
  }
}