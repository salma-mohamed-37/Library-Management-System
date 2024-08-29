import { Component, Input } from '@angular/core';
import { UserDto } from '../../../../interfaces/User/UserDto';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../../services/user.service';
import { ActivatedRoute, Router} from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { MenuItem } from 'primeng/api';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ChangePasswordDto } from '../../../../interfaces/User/ChangePasswordDto';
import { ToastContentService } from '../../../../services/toast-content.service';

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
  viewChangePassword:boolean=false;
  viewDeleteConfirm:boolean=false
  changeForm!: FormGroup;

  constructor(private userService:UserService,public route:ActivatedRoute, private router:Router, private fb:FormBuilder, private toastContnentService : ToastContentService){}

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
        label: "Update Profile",
        command:()=>{
          if(this.userId ==undefined)
          {
            this.navigate("pages/-/update")
          }
          else
          {
            this.navigate("pages/-/update/"+this.userId)
          }
          
        }
      },
      {
        label: "Change Password",
        command: () => {
          this.viewChangePassword=true
      }}
    ]

    if(this.userId)
    {
      this.items.push({
        label : "Delete",
        command:()=>{
          this.toastContnentService.showConfirm().subscribe({
            next:(res)=>
            {
              if(res)
              {
                this.userService.delete(this.userId).subscribe({
                  next:(res)=>
                  {
                    this.router.navigate(['pages/dashboard/users/all'])
                  }
                })
              }
            }
          })
        }
      })
    }

    this.changeForm=this.fb.group({
      oldPassword:new FormControl('', [Validators.required,Validators.minLength(8)]),
      newPassword:new FormControl('', [Validators.required, Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$'),Validators.minLength(8)])
    })

  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

  navigate(url:string)
  {
    this.router.navigate([url]);
  }

  submit()
  {
    if(this.changeForm.valid)
    {
      console.log(this.changeForm.value)
      const req : ChangePasswordDto ={
        oldPassword:this.changeForm.value["oldPassword"],
        newPassword:this.changeForm.value["newPassword"],
        userId :this.userId
      }

      this.userService.changePassword(req).subscribe({
        next:(res)=>
        {
          this.viewChangePassword=false
        }
      })
    }
    else
    {
      console.log("invalid")
    }
  }

  get oldPassword()
  {
    return this.changeForm.get('oldPassword')
  }

  get newPassword()
  {
    return this.changeForm.get('newPassword')
  }

}
