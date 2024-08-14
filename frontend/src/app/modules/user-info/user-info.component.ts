import { Component, Input } from '@angular/core';
import { UserDto } from '../../interfaces/User/UserDto';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent {

  @Input() user!:UserDto;
  viewDates:boolean=false;

  constructor(private authService :AuthService){}

  ngOnInit()
  {
    const role = this.authService.getCurrentUserRole()
    if(role == "lIBRARIAN")
    {
      this.viewDates=true
    }
    else
    {
      this.viewDates=false
    }
  }


}
