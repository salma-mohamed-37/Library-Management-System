import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(public http : HttpClient) { }

  getCategoriesNames():Observable<string[]>
  {
    return this.http.get<string[]>(environment.apiUrl+"api/categories/names")
  }


}
