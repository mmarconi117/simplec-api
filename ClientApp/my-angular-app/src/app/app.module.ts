import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http'; // Import provideHttpClient

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    // HttpClientModule is no longer needed here
  ],
  providers: [
    provideHttpClient() // Add provideHttpClient here
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
