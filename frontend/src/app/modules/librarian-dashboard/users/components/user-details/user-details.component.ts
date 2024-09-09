import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../../../services/user.service';
import { UserDto } from '../../../../../interfaces/User/UserDto';
import { environment } from '../../../../../../environments/environment';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrl: './user-details.component.scss'
})
export class UserDetailsComponent {
  constructor(private route: ActivatedRoute, private userService:UserService, private router:Router){}

  userId:string=""
  items: MenuItem[] | undefined;
  current:boolean=false
  history:boolean=false

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('id')!;

    });

    this.items = [
      { label: 'Borrow History', icon: 'pi pi-history' },
      { label: 'Currently Borrowed', icon: 'pi pi-bookmark' }
    ]
  }

  onActiveItemChange(event:any)
  {
    if(event.label == "Borrow History")
    {
        this.current=false
        this.history=true
    }
    else if (event.label == "Currently Borrowed")
    {
      this.history=false
      this.current=true
    }
    
  }

}
