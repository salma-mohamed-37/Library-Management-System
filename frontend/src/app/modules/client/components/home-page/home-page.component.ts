import { Component, numberAttribute } from '@angular/core';
import { Book } from '../../../../interfaces/book/Book';
import { BookService } from '../../../../services/book.service';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {
  selectedBook? : Book
  visible : boolean =false
  constructor(public bookService:BookService){}
  books : Book[] =[]
  ngOnInit()
  {
    this.bookService.getBooksForUser(4,1).subscribe({
      next:(r)=>
      {
        this.books = r.data

      },
      error:(err) =>
      {
        console.log(err)
      }
    });
 }

 getFullDetails(selectedBook:Book)
 {
    this.selectedBook = selectedBook
    this.display()
 }

 display()
 {
  this.visible=true
 }

 hide()
 {
  this.visible = false
 }

}
