import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from '../../../../interfaces/book/Book';
import { BookService } from '../../../../services/book.service';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrl: './book-details.component.scss'
})
export class BookDetailsComponent {
  constructor(private route: ActivatedRoute,public bookService: BookService) { }

  @Input() selectedBook?:Book
  @Input() visible! :boolean
  @Output() dialogClosed = new EventEmitter<boolean>()

  ngOnInit()
  {

  }

  getFullImageUrl(path: string): string {
    return `${environment.apiUrl}${path}`;
  }

  hideDialog() {
    this.dialogClosed.emit();
  }


}
