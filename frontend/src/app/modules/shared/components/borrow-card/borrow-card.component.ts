import { Component, Input } from '@angular/core';
import { GetBorrowedBookForUser } from '../../../../interfaces/book/GetBorrowedBookForUser';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-borrow-card',
  templateUrl: './borrow-card.component.html',
  styleUrl: './borrow-card.component.scss'
})
export class BorrowCardComponent {

  @Input() book!:GetBorrowedBookForUser
  getFullImageUrl(path: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

  isReturned(date:string|Date|undefined)
  {
    const d = "9999-01-01T00:00:00"
    if (date! !== d)
    {
      return true
    }
    else
    {
      return false
    }
  }
}
