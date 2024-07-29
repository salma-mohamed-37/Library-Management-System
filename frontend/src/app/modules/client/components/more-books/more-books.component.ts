import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookService } from '../../../../services/book.service';
import { Book } from '../../../../interfaces/book/Book';
import { PaginationDto } from '../../../../interfaces/common/PaginationDto';

@Component({
  selector: 'app-more-books',
  templateUrl: './more-books.component.html',
  styleUrl: './more-books.component.scss'
})
export class MoreBooksComponent {
  constructor(private route: ActivatedRoute, public booksService: BookService ) { }
  loading : boolean =true
  pageNumber:number=0;
  pageSize: number =6
  books! : PaginationDto<Book>

  ngOnInit(): void {
    this.getPage()

  }
  getPage()
  {
    this.pageNumber++
    this.loading=true
    this.booksService.getBooksForUser(this.pageSize, this.pageNumber).subscribe({
      next : (r)=>
      {
        if (this.books == undefined)
        {
          this.books=r
        }
        else
        {
          this.books.data.push(...r.data)
          this.books.count=r.count
        }
      },
      error : (err)=>
      console.log(err)
    })

    this.loading=false
  }

  isThereareMore()
  {
    if(this.pageNumber* this.pageSize < this.books.count)
    {
      return true
    }
    else
    {
      return false
    }
  }

}
