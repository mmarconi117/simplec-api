import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BitcoinService {
  private apiUrl = 'http://localhost:5007/api/api/bitcoin-prices'; // Updated with the correct API URL

  constructor(private http: HttpClient) {}

  getBitcoinPrices(): Observable<any> {
    return this.http.get<any>(this.apiUrl).pipe(
      catchError(this.handleError) // Handle errors
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(errorMessage);
  }
}
