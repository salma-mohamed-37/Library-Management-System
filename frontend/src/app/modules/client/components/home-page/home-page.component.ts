import { Component, numberAttribute } from '@angular/core';
import { Book } from '../../../../interfaces/book/Book';
import { BookService } from '../../../../services/book.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  constructor(public bookService:BookService){}
  books : Book[] =[]
  loading:boolean=true
  ngOnInit()
  {
    this.bookService.getBooksForUser(4,1).subscribe({
      next:(r)=>
      {
        this.books = r.data
        this.loading=false
      },
      error:(err) =>
      {
       
      }
    });

 }

}
