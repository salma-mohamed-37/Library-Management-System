import { Component } from '@angular/core';
import { UserService } from '../../../../services/user.service';
import { UserDto } from '../../../../interfaces/User/UserDto';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  user!:UserDto;
  constructor(private userService:UserService){}


  ngOnInit()
  {
    this.userService.getUserbyID().subscribe({
      next:(res)=>
      {
          this.user = res;
      }
    })
  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

}
