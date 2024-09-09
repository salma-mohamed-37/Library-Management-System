import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { Route, Router } from '@angular/router';

@Injectable()
export class RequestInterceptor implements HttpInterceptor
{
  constructor(private authService:AuthService, private router:Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    const token = this.authService.getCurrentUserToken()
    if(token)
    {
      if(this.authService.isTokenValid())
      {
        const authReq = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
        return next.handle(authReq);
      }
      else
      {
        this.authService.logout()
        return of(new HttpResponse({ status: 401, statusText: 'Unauthorized' }));
      }
    }
    else
    {
      return next.handle(req);
    }

  }
}
