import { Component, effect, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MenuModule } from 'primeng/menu';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MenuModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})

export class HeaderComponent {
  constructor( public router:Router, private authService : AuthService)
  {
    effect(() => {
      this.isLoggedIn=this.authService.isLoggedin();
      this.constructItems()
    });
  }
  items: any[] = [];
  isLoggedIn : boolean =false

  login()
  {
    this.router.navigate(["pages/auth/login"]);
  }

  constructItems()
  {
    this.items=[]
    if(this.authService.isLoggedin())
    {
      if(this.authService.getCurrentUserRole() == "USER")
      {
        this.items.push({
          label:"Borrow History",
            command: () => {
                this.navigate("pages/client/borrow-history");

            }},
            {
              label:"Currently Borrowed",
                command: () => {
                    this.navigate("pages/client/homepage");

                }}
          );
      }
      else if (this.authService.getCurrentUserRole() == "lIBRARIAN")
      {
        this.items.push({
          label:"Dashboard",
            command: () => {
                this.navigate("pages/dashboard");

            }},
          );
      }
      else if(this.authService.getCurrentUserRole() == "ADMIN")
      {
        this.items.push({
          label:"Admin Dashboard",
            command: () => {
                this.navigate("pages/client/homepage");

            }},
          );

      }

      this.items.push(
        {
          label:"Profile",
            command: () => {
                this.navigate("pages/client/homepage");

            }},
        {
        label:"log out",
          command: () => {
              this.logout();

          }});
    }
  }

  logout()
  {
    this.authService.logout();
    this.router.navigate(["pages/client/homepage"]);
    this.constructItems()
  }

  navigate(url:string)
  {
    this.router.navigate([url]);
  }
}
