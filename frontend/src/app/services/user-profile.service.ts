import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginationDto } from '../interfaces/common/PaginationDto';
import { GetBorrowedBookForUser } from '../interfaces/book/GetBorrowedBookForUser';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(public http : HttpClient) { }

  getMyBorrowHistory(pageSize:number,pageNumber:number): Observable<PaginationDto<GetBorrowedBookForUser>>
  {
    return this.http.get<PaginationDto<GetBorrowedBookForUser>>(environment.apiUrl+"api/users/borrow-history/"+pageNumber+"/"+pageSize)
  }
}
