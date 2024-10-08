import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { Book } from '../interfaces/book/Book';
import { PaginationDto } from '../interfaces/common/PaginationDto';
import { FilteringRequest } from '../interfaces/common/FilteringRequest';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(public http : HttpClient) { }

  getBooksForUser(pageSize: number, pageNumber:number): Observable<PaginationDto<Book>>
  {
    return this.http.get<PaginationDto<Book>>(environment.apiUrl+"api/books/"+pageNumber+"/"+pageSize)
  }

  getBookbyId(id:number): Observable<Book>
  {
    return this.http.get<Book>(environment.apiUrl+"api/books/"+id)
  }

  getFilteredBooks(request: FilteringRequest) :  Observable<PaginationDto<Book>>
  {
    return this.http.post<PaginationDto<Book>>(environment.apiUrl+"api/books/filtered", request)
  }

  getBooksForLibrarian(pageNumber:number, pageSize:number, name:string)
  {
      return this.http.post<PaginationDto<Book>>(environment.apiUrl+"api/books/librarian/search/"+pageNumber+"/"+pageSize, {name})
  }

  getAvailableBooksForLibrarian(pageNumber:number, pageSize:number, name:string)
  {
      return this.http.post<PaginationDto<Book>>(environment.apiUrl+"api/books/librarian/available/search/"+pageNumber+"/"+pageSize, {name})
  }

  getFilteredBooksForLibrarian(request:FilteringRequest)
  {
    return this.http.post<PaginationDto<Book>>(environment.apiUrl+"api/books/librarian/filtered", request)
  }

  addBook(book:FormData)
  {
      return this.http.post(environment.apiUrl+"api/books", book)
  }
}
