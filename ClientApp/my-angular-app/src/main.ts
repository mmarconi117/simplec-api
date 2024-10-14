import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component'; // Adjust the path as necessary

// Bootstrap the application with the standalone AppComponent and the provided appConfig
bootstrapApplication(AppComponent, { providers: appConfig.providers }) // Use providers from appConfig
  .catch((err) => console.error(err));
