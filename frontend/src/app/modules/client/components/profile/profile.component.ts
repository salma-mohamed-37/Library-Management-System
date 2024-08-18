import { Component } from '@angular/core';
import { UserService } from '../../../../services/user.service';
import { UserDto } from '../../../../interfaces/User/UserDto';
import { environment } from '../../../../../environments/environment';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  user!:UserDto;
  constructor(private userService:UserService,public router:Router){}
  items: MenuItem[]=[]


  ngOnInit()
  {
    this.userService.getUserbyID().subscribe({
      next:(res)=>
      {
        console.log(res)
          this.user = res;
      }
    })
    this.items=[
      {
        label: "Update Profile"
      },
      {
        label: "Change Password"
      },
      {
        label : "Delete"
      }
    ]

  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }


  navigate(url:string)
  {
    this.router.navigate([url]);
  }
}
