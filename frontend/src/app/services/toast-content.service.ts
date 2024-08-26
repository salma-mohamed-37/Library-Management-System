import { Injectable } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ToastContentService {

  constructor(private messageService: MessageService, private confirmationService: ConfirmationService) { }

  showError(message:string)
  {
    this.messageService.add({ severity: 'error', summary: 'Error', detail: message });

  }

  showSuccess(message:string)
  {
    this.messageService.add({ severity: 'success', summary: 'Success', detail: message });

  }

  showConfirm():Observable<boolean>
  {
    return new Observable(observer => {
  
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete?',
        header: 'Confirmation',
        icon: 'pi pi-exclamation-triangle',
        acceptIcon: "none",
        rejectIcon: "none",
        rejectButtonStyleClass: "p-button-text",
        accept: () => {
          observer.next(true);
          observer.complete(); 
        },
        reject: () => {
          observer.next(false);
          observer.complete(); 
        }
      });
    });
  }
}
