import { Component, Input } from '@angular/core';
import { Book } from '../../../../interfaces/book/Book';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-book-card',
  templateUrl: './book-card.component.html',
  styleUrl: './book-card.component.scss'
})
export class BookCardComponent {
  @Input() book : Book|undefined =undefined;

  getFullImageUrl(path: string): string {
    return `${environment.apiUrl}${path}`;
  }
}
