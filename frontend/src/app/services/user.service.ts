import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PaginationDto } from '../interfaces/common/PaginationDto';
import { UserDto } from '../interfaces/User/UserDto';
import { environment } from '../../environments/environment';
import { Book } from '../interfaces/book/Book';
import { ChangePasswordDto } from '../interfaces/User/ChangePasswordDto';
import { SearchforUser} from '../interfaces/User/searchByname';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http : HttpClient) { }

  getUsers(pageSize:number,pageNumber:number, request:SearchforUser):Observable<PaginationDto<UserDto>>
  {
    return this.http.post<PaginationDto<UserDto>>(environment.apiUrl+"api/users/search/"+pageNumber+"/"+pageSize,request)
  }
  addUser(user:FormData)
  {
    return this.http.post(environment.apiUrl+"api/account/register", user)
  }
  //add, update , delete

  getUserbyID(userId? :string)
  {
    if (userId)
    {
      let params = new HttpParams();
      params = params.set('userId', userId);

      return this.http.get<UserDto>(environment.apiUrl+"api/account/info", {params})
    }
    else
    {
      return this.http.get<UserDto>(environment.apiUrl+"api/account/info")
    }
  }

  changePassword(dto:ChangePasswordDto)
  {
    return this.http.put(environment.apiUrl+"api/account/password",dto)
  }

  delete(userId?:string)
  {
    if (userId)
      {
        let params = new HttpParams();
        params = params.set('userId', userId);

        return this.http.delete(environment.apiUrl+"api/account", {params})
      }
      else
      {
        return this.http.delete(environment.apiUrl+"api/account")
      }
  }

  updateUser( user:FormData, userId?:string)
  {
    if (userId)
      {
        let params = new HttpParams();
        params = params.set('userId', userId);
  
        return this.http.put(environment.apiUrl+"api/account",user, {params})
      }
      else
      {
        return this.http.put(environment.apiUrl+"api/account",user)
      }
  }
}
