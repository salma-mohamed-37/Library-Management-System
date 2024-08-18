import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../../../services/user.service';
import { UserDto } from '../../../../../interfaces/User/UserDto';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent {
  constructor(private route: ActivatedRoute, private userService:UserService){}

  userId:string=""
  user!:UserDto

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('id')!;
      console.log(this.userId)
    });

    this.userService.getUserbyID(this.userId).subscribe({
      next:(res)=>
      {
        this.user=res
      }
    })
  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

}
