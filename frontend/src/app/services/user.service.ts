import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationDto } from '../interfaces/common/PaginationDto';
import { UserDto } from '../interfaces/User/UserDto';
import { environment } from '../../environments/environment';
import { Book } from '../interfaces/book/Book';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http : HttpClient) { }

  getUsers(pageSize:number,pageNumber:number, name:string):Observable<PaginationDto<UserDto>>
  {
    return this.http.post<PaginationDto<UserDto>>(environment.apiUrl+"api/users/search/"+pageNumber+"/"+pageSize,{name})
  }

  getCurrentlyBorrowedBooksByUserForLibrarian(userId:string):Observable<Book[]>
  {
    return this.http.get<Book[]>(environment.apiUrl+"api/Users/librarian/current-borrow/"+userId)
  }
}
