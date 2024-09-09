import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { ResponseInterceptor } from './interceptors/Response.interceptor';
import { ConfirmationService, MessageService } from 'primeng/api';
import { RequestInterceptor } from './interceptors/Request.interceptor';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
     provideRouter(routes),
     provideAnimations(),
     provideHttpClient( withFetch(), withInterceptorsFromDi(),),
     {
      provide: HTTP_INTERCEPTORS,
      useClass: RequestInterceptor,
      multi: true
    },
     {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseInterceptor,
      multi: true
    },
    MessageService,
    ConfirmDialogModule,
    ConfirmationService
  ]
};
