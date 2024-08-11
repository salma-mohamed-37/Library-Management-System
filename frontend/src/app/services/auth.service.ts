import { Injectable, signal } from '@angular/core';
import { LoggedInUser, LoginDto } from '../interfaces/User/LoginDto';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  //log in logout is logged in get currently logged in user is valid token

  public authStatus = signal<boolean>(false);

  constructor(private http: HttpClient) {
    this.initializeAuthStatus();
  }

  private initializeAuthStatus()
  {
    this.authStatus.set(this.isTokenValid());
  }

  logIn(userData:LoginDto): Observable<boolean>
  {
    return this.http.post<LoggedInUser>(`${environment.apiUrl}api/Account/login`, userData)
      .pipe(
        map(res => {
          localStorage.setItem("Id", res.id);
          localStorage.setItem("Role", res.role);
          localStorage.setItem("Token", res.token);
          localStorage.setItem("Expiry Date", new Date(res.expiryDate).toISOString());
          localStorage.setItem("Image", res.imagePath);
          this.authStatus.set(true)
          return true;
        }),
        catchError(err => {
          return of(false);
        })
      );
  }

  logout()
  {
    localStorage.setItem("Id", "");
    localStorage.setItem("Role","");
    localStorage.setItem("Token", "");
    localStorage.setItem("Expiry Date", "");
    localStorage.setItem("Image", "");
    this.authStatus.set(false)
  }

  isLoggedin():boolean
  {
    return this.authStatus();
  }

  getCurrentUserId()
  {
    return localStorage.getItem("Id")
  }

  getCurrentUserRole()
  {
    return localStorage.getItem("Role")
  }

  getCurrentUserToken()
  {
    return localStorage.getItem("Token")
  }

  getUserImage()
  {
    return localStorage.getItem("Image")
  }

  isTokenValid():boolean
  {
    if(new Date(localStorage.getItem("Expiry Date")!) > new Date())
    {
        return true
    }
    else
    {
      return false
    }
  }
}
