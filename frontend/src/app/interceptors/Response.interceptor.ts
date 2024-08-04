import { HttpErrorResponse, HttpEvent, HttpEventType, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest, HttpResponse } from '@angular/common/http';
import { catchError, map, Observable, of, throwError} from 'rxjs';
import { APIResponse } from '../interfaces/common/apiresponse';
import { Injectable } from '@angular/core';
import { ToastContentService } from '../services/toast-content.service';

@Injectable
({
  providedIn: 'root'
})
export class ResponseInterceptor implements HttpInterceptor {

  constructor(private toastContentService: ToastContentService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
        map((event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
            const httpResponse = event as HttpResponse<APIResponse<any>>;
            const response = httpResponse.body as APIResponse<any>;
            console.log(response.statusCode)

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
            this.toastContentService.showError(error.error.message)
            return of(error as any);
        })
    );
  }
}
