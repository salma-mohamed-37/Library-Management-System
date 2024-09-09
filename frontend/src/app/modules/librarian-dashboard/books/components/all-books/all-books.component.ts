import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PaginationDto } from '../../../../../interfaces/common/PaginationDto';
import { Book } from '../../../../../interfaces/book/Book';
import { environment } from '../../../../../../environments/environment';
import { BookService } from '../../../../../services/book.service';
import { FilteringRequest } from '../../../../../interfaces/common/FilteringRequest';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CategoryService } from '../../../../../services/category.service';

@Component({
  selector: 'app-all-books',
  templateUrl: './all-books.component.html',
  styleUrl: './all-books.component.scss'
})
export class AllBooksComponent {

  first:number =1
  rows:number=5

  filters!: FormGroup;

  filterRequest: FilteringRequest = {
    pageNumber:1,
    pageSize:5
  }

  response:PaginationDto<Book>={
    pageNumber:0,
    pageSize:0,
    count:0,
    data:[]
  }

  loading:boolean=false

  constructor(private router:Router, private booksService:BookService, private fb:FormBuilder, private categoryService:CategoryService){}

  ngOnInit(): void {
    this.filters = this.fb.group({
      bookName: [''],
      isDeleted:[''],
    });
  }

  getBooks()
  {
      this.booksService.getFilteredBooksForLibrarian(this.filterRequest).subscribe({
        next:(res)=>
        {
          this.response=res
        }
        
      })
  }

  navigate(url:string)
  {
    this.router.navigate([url])
  }

  onPageChange(event:any)
  {
    this.first=event.first
    this.rows=event.rows
    this.filterRequest.pageSize=event.rows
    this.filterRequest.pageNumber= Math.floor(event.first / event.rows)+1
    this.getBooks()
  }

  search()
  {
    this.filterRequest.IsDeleted=this.filters.value["isDeleted"]
    this.filterRequest.name=this.filters.value["bookName"]
    this.getBooks()
  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

  

}
