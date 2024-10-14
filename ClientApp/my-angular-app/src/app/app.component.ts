import { Component, OnInit } from '@angular/core';
import { BitcoinService } from './bitcoin.service'; // Adjust the path as necessary

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'] // Optional: your CSS file
})
export class AppComponent implements OnInit {
  bitcoinPrices: any; // Replace 'any' with a defined interface if necessary
  errorMessage: string | null = null;

  constructor(private bitcoinService: BitcoinService) {}

  ngOnInit(): void {
    this.fetchBitcoinPrices();
  }

  fetchBitcoinPrices(): void {
    this.bitcoinService.getBitcoinPrices().subscribe({
      next: (data) => {
        this.bitcoinPrices = data; // Store the fetched data
        this.errorMessage = null; // Clear any previous error messages
      },
      error: (error) => {
        this.errorMessage = error; // Set the error message
        this.bitcoinPrices = null; // Clear the data if there's an error
      }
    });
  }
}
