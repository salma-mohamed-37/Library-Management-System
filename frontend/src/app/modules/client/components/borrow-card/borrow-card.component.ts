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
}
