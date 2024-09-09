import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginationDto } from '../interfaces/common/PaginationDto';
import { GetBorrowedBookForUser } from '../interfaces/book/GetBorrowedBookForUser';
import { Observable } from 'rxjs';
import { Book } from '../interfaces/book/Book';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(public http : HttpClient) { }

  getBorrowHistoryForUser(pageSize:number,pageNumber:number,userId?:string): Observable<PaginationDto<GetBorrowedBookForUser>>
  {
    if (userId)
    {
      let params = new HttpParams();
      params = params.set('userId', userId);

      return this.http.get<PaginationDto<GetBorrowedBookForUser>>(environment.apiUrl+"api/users/borrow-history/"+pageNumber+"/"+pageSize, {params})
    }
    else
    {
      return this.http.get<PaginationDto<GetBorrowedBookForUser>>(environment.apiUrl+"api/users/borrow-history/"+pageNumber+"/"+pageSize)
    }
  }

  getCurrentlyBorrowedBooks(userId?:string):Observable<Book[]>
  {
    if (userId)
      {
        let params = new HttpParams();
        params = params.set('userId', userId);

        return this.http.get<Book[]>(environment.apiUrl+"api/Users/current-borrow/", {params})
      }
      else
      {
        return this.http.get<Book[]>(environment.apiUrl+"api/Users/current-borrow/")
      }

  }
}
