import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Book } from '../interfaces/book/Book';
import { PaginationDto } from '../interfaces/common/PaginationDto';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  constructor(public http : HttpClient) { }

  getBooksForUser(pageSize: number, pageNumber:number): Observable<PaginationDto<Book>>
  {
    return this.http.get<PaginationDto<Book>>(environment.apiUrl+"api/books/"+pageNumber+"/"+pageSize)
  }

}
