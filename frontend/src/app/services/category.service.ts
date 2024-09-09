import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Category } from '../interfaces/category/catgory';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(public http : HttpClient) { }

  getCategoriesNames():Observable<string[]>
  {
    return this.http.get<string[]>(environment.apiUrl+"api/categories/names")
  }

  getCategories():Observable<Category[]>
  {
      return this.http.get<Category[]>(environment.apiUrl+"api/categories/all")
  }


}
