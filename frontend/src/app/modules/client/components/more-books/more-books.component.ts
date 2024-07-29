import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookService } from '../../../../services/book.service';
import { Book } from '../../../../interfaces/book/Book';
import { PaginationDto } from '../../../../interfaces/common/PaginationDto';
import { CategoryService } from '../../../../services/category.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilteringRequest } from '../../../../interfaces/common/FilteringRequest';

@Component({
  selector: 'app-more-books',
  templateUrl: './more-books.component.html',
  styleUrl: './more-books.component.scss'
})
export class MoreBooksComponent {
  constructor(private fb: FormBuilder, private route: ActivatedRoute, public booksService: BookService, public catgoryService:CategoryService) { }
  sidebarVisible: boolean=false
  loading : boolean =false

  filterRequest: FilteringRequest = {
    pageNumber:0,
    pageSize:6
  }

  books: PaginationDto<Book> =
  {pageNumber:this.filterRequest.pageNumber,
    pageSize:this.filterRequest.pageSize,
     data: [],
      count: 0 };

  filters!: FormGroup;
  categories: { label: string, value: string }[] = [{ label: "Select genre", value: "" }];
  sortFields = [
    { label: 'Sort Field', value: '' },
    { label: 'Name', value: 'name' },
    { label: 'Publish Date', value: 'publish date' },
    { label: 'Author', value: 'author' },
    { label: 'Genre', value: 'genre' }
  ];
  sortDirections = [
    { label: 'Sort Direction', value: '' },
    { label: 'Ascending', value: 'asc' },
    { label: 'Descending', value: 'desc' }
  ];

  ngOnInit(): void {
    this.filters = this.fb.group({
      bookName: [''],
      author: [''],
      genre: [''],
      publishDateFrom: [null],
      publishDateTo: [null],
      sortField: [''],
      sortDirection: ['']
    });
    this.getCategories()
    this.getPage()
  }
  getPage()
  {
    this.filterRequest.pageNumber++
    this.loading=true
    this.booksService.getFilteredBooks(this.filterRequest).subscribe({
      next:(b)=>
      {
        if(this.books.data.length == 0)
        {
          this.books=b
        }
        else
        {
          this.books.data.push(...b.data)
          this.books.count=b.count
        }
        this.loading=false
        this.sidebarVisible = false


      },
      error:(err)=>
        console.log(err)
    })
  }



  onSubmit()
  {
    const formValues = this.filters.value;

      this.filterRequest.name = formValues.bookName || undefined
      this.filterRequest.author= formValues.author || undefined
      this.filterRequest.category = formValues.genre || undefined
      this.filterRequest.fromDate= formValues.publishDateFrom || undefined
      this.filterRequest.toDate= formValues.publishDateTo || undefined
      this.filterRequest.sortField= formValues.sortField || undefined
      this.filterRequest.sortDirection= formValues.sortDirection || undefined
      this.filterRequest.pageNumber=0

      this.books.data =[]
      this.books.count=0
      this.books
      this.getPage()
      console.log("done")

  }

  getCategories()
  {
    this.catgoryService.getCategoriesNames().subscribe({
      next:(r)=>
        this.categories.push(...r.map(category => ({ label: category, value: category }))),
      error :(err)=>
        console.log(err)
    })
  }

  isThereareMore()
  {
    if(this.filterRequest.pageNumber* this.filterRequest.pageSize < this.books.count)
    {
      return true
    }
    else
    {
      return false
    }
  }

}
