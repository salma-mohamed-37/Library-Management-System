import { Component } from '@angular/core';
import { Book } from '../../../../../interfaces/book/Book';
import { BookService } from '../../../../../services/book.service';
import { environment } from '../../../../../../environments/environment';
import { PaginationDto } from '../../../../../interfaces/common/PaginationDto';
import { BorrowService } from '../../../../../services/borrow.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-borrow',
  templateUrl: './borrow.component.html',
  styleUrl: './borrow.component.scss'
})
export class BorrowComponent {
  data:PaginationDto<Book>=
  {
    pageSize:3,
    pageNumber:1,
    count:0,
    data:[]
  }
  source!:Book[]
  target:Book[]=[]
  name:string=""
  first:number=1
  rows:number=3

  loading:boolean=true

  constructor(private bookService:BookService, private borrowService: BorrowService, private router:Router){}

  ngOnInit()
  {
    this.getBooks()
  }

  getFullImage(image:string)
  {
    return environment.apiUrl+image
  }

  getBooks()
  {
    this.loading=true
    const pageNumber = Math.floor(this.first / this.rows) + 1;
    this.bookService.getAvailableBooksForLibrarian(pageNumber,this.rows,this.name).subscribe({
      next:(res)=>
      {
        console.log(this.target)
        this.data=res
        this.data.data= res.data.filter(item =>!this.target.some(t => t.id === item.id));
      }
    })
    this.loading=false
  }

  search(name:string)
  {
    this.name=name
    this.getBooks()
  }

  onPageChange(event:any)
  {
    this.first=event.first
    console.log(this.first)
    this.getBooks()
  }

  borrow()
  {
    this.loading=true
    if(this.target.length>0)
    {
      var ids = this.target.map(b=>b.id)
      var uniqueIds = [...new Set(ids)];
      this.borrowService.booksIds=uniqueIds
      this.target=[]

      this.borrowService.borrow().subscribe({
        next:(res)=>
        {
          console.log(res)
        }
      })
      this.borrowService.clear()
    }
    this.loading=false
  }


}
