import { Component, effect, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MenuModule } from 'primeng/menu';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';


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
      this.profile=this.authService.getUserImage()!;
      this.constructItems()
    });
  }
  profile?:string =""
  items: any[] = [];
  isLoggedIn : boolean =false

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

  login()
  {
    this.router.navigate(["pages/auth/login"]);
  }

  constructItems()
  {
    this.items=[]
    this.items.push({
      icon: 'pi pi-home',
      label:"Home",
        command: () => {
            this.navigate("pages/client/home");

    }})

    if(this.authService.isLoggedin())
    {
      if(this.authService.getCurrentUserRole() == "USER")
      {
        this.items.push({
          icon: "pi pi-history",
          label:"Borrow History",
            command: () => {
                this.navigate("pages/client/borrow-history");

            }},
        {
          icon:"pi pi-bookmark",
          label:"Books in Use",
            command: () => {
                this.navigate("pages/client/current");

            }}
        );
      }
      else if (this.authService.getCurrentUserRole() == "lIBRARIAN")
      {
        this.items.push({
          icon:"pi pi-th-large",
          label:"Dashboard",
            command: () => {
                this.navigate("pages/dashboard");

            }},
          );
      }
      else if(this.authService.getCurrentUserRole() == "ADMIN")
      {
        this.items.push({
          icon:"pi pi-th-large",
          label:"Admin Dashboard",
            command: () => {
                this.navigate("pages/client/homepage");

            }},
          );

      }

      this.items.push(
        {
          icon:"pi pi-user",
          label:"Profile",
            command: () => {
                this.navigate("pages/client/profile");

            }},
        {
          icon:"pi pi-sign-out",
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
