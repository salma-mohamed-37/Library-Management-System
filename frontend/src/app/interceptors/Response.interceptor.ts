import { HttpErrorResponse, HttpEvent, HttpEventType, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';
import { catchError, map, Observable, of, throwError} from 'rxjs';
import { APIResponse } from '../interfaces/common/apiresponse';
import { Injectable } from '@angular/core';
import { ToastContentService } from '../services/toast-content.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {

  constructor(private toastContentService: ToastContentService, private authService :AuthService, private router:Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
        map((event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            const httpResponse = event as HttpResponse<APIResponse<any>>;
            const response = httpResponse.body as APIResponse<any>;
            console.log(response.statusCode)
            console.log(response)

            if (response.statusCode === 200)
            {
              if (response.data == undefined)
              {
                this.toastContentService.showSuccess(response.message)
                return event;
              }
              else
              {
                return httpResponse.clone({ body: response.data });
              }
            }
            else
            {
              var messages = response.message.split("\n")
              for (var m in messages)
              {
                this.toastContentService.showError(m)
              }
              return event
            }
          }
          return event;
        }),
        catchError((error: HttpErrorResponse) =>
        {
          if(error.status == 401)
          {
            this.authService.logout()
            this.router.navigate(["pages/client/homepage"])
            this.toastContentService.showError("Unauthorized please login.")
            return of(error as any);
          }
          else if (error.status==400)
          {
            console.log(error)
            if(error.error.message)
            {
              this.toastContentService.showError(error.error.message)
            }
            if(error.error.errors)
            {
              for(var e in error.error.errors)
                {
                  this.toastContentService.showError(error.error.errors[e])
                }
            }
            return of(error as any);
          }
          else
          {
            this.toastContentService.showError(error.error.message)
            return of(error as any);
          }
        })
    );
  }
}
