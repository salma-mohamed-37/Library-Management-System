import { Component, Input } from '@angular/core';
import { UserDto } from '../../../../interfaces/User/UserDto';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../../services/user.service';
import { ActivatedRoute, Router} from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.scss'
})
export class UserInfoComponent {

  @Input() inId? :string;
  user!:UserDto;
  userId!:string |undefined
  items: MenuItem[]=[]
  constructor(private userService:UserService,public route:ActivatedRoute, private router:Router){}

  ngOnInit()
  {
    if (! this.inId)
    {
      this.route.paramMap.subscribe(params => {
        this.userId = params.get('id')||undefined;
        console.log(this.userId)
      })
    }
    
    else
    {
      this.userId= this.inId
    }
    this.userService.getUserbyID(this.userId).subscribe({
      next:(res)=>
      {
        console.log(res)
          this.user = res;
      },
      error:(err)=>
      {
        console.log(err)
      }
    })

    this.items=[
      {
        label: "Update Profile"
      },
      {
        label: "Change Password"
      }
      
    ]

    if(this.userId)
    {
      this.items.push({
        label : "Delete"
      })
    }

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
