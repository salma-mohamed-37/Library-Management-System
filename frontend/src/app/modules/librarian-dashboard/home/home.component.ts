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
      path:"pages/dashboard/users"
    },
    {
      label:"Author",
      path:"pages/dashboard/users"
    },
    {
      label:"Genre",
      path:"pages/dashboard/users"
    },
    {
      label:"Users",
      path:"pages/dashboard/users/all"
    }
  ]

  navigate(url:string)
  {
    this.router.navigate([url])
  }

}
