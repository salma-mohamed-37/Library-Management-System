import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BorrowService {

  constructor(private http:HttpClient) { }
  userId:string=""
  booksIds:number[]=[]

  borrow()
  {
    return this.http.post(environment.apiUrl+"api/Borrows/borrow",{UserId:this.userId, BooksIds:this.booksIds})
  }

  return()
  {
    return this.http.put(environment.apiUrl+"api/Borrows/return",{UserId:this.userId, BooksIds:this.booksIds})
  }

  clear()
  {
    this.booksIds=[]
    this.userId=''
  }
}
