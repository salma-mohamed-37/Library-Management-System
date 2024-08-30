import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Author } from '../interfaces/author/author';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http : HttpClient) { }

  getAuthorsNames():Observable<string[]>
  {
    return this.http.get<string[]>(environment.apiUrl+"api/authors/names")
  }

  getAuthors():Observable<Author[]>
  {
      return this.http.get<Author[]>(environment.apiUrl+"api/authors/all")
  }
}
