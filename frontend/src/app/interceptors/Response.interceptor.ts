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
              this.toastContentService.showError(response.message)
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
          else
          {
            this.toastContentService.showError(error.error.message)
            return of(error as any);
          }
        })
    );
  }
}
