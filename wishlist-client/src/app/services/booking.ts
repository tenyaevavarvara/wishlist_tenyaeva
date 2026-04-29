import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface BookedWish {
  id: number;
  bookedAt: Date;
  wishId: number;
  wishTitle: string;
  wishPrice?: number;
  wishLink?: string;
  wishDescription?: string;
  ownerId: number;
  ownerName: string;
  wishlistId: number;
  wishlistName: string;
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'https://localhost:7178/api/bookings';

  constructor(private http: HttpClient) {}

  getMyBookings(): Observable<BookedWish[]> {
    return this.http.get<BookedWish[]>(`${this.apiUrl}/my`);
  }

  checkIfBookedByMe(wishId: number): Observable<{ wishId: number; isBookedByMe: boolean }> {
    return this.http.get<{ wishId: number; isBookedByMe: boolean }>(`${this.apiUrl}/check/${wishId}`);
  }

  getBookingStatistics(): Observable<any> {
    return this.http.get<any>(`https://localhost:7178/api/reports/booking-statistics`);
  }
}