import { Component } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  constructor(private router:Router){}

  menuItems: any[]=[
    {
      label:"Books",
      path:"pages/dashboard/books",
      icon:"pi pi-book"
    },
    {
      label:"Authors",
      path:"pages/dashboard/users",
      icon:"pi pi-pen-to-square"
    },
    {
      label:"Genres",
      path:"pages/dashboard/users",
      icon:"pi pi-objects-column"
    },
    {
      label:"Users",
      path:"pages/dashboard/users/all",
      icon:"pi pi-user"
    }
  ]

  navigate(url:string)
  {
    this.router.navigate([url])
  }

}
