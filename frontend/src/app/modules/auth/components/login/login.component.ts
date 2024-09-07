import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Password } from 'primeng/password';
import { AuthService } from '../../../../services/auth.service';
import { LoginDto } from '../../../../interfaces/User/LoginDto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginForm! : FormGroup;
  submitted:boolean=false;
  constructor(private fb: FormBuilder, private authService : AuthService, private router:Router ){}

  ngOnInit()
  {
    this.loginForm = this.fb.group({
      email:['',[Validators.required, Validators.email]],
      password:['',[Validators.required]]
    })
  }

  submit()
  {
    this.submitted=true
    if (this.loginForm.valid)
    {
      var data : LoginDto =  {
        email:this.loginForm.value.email,
        password:this.loginForm.value.password
      }
      this.authService.logIn(data).subscribe({
        next:(res)=>
        {
          if(res)
           this.router.navigate(["pages/client/home-page"])
        },
        complete:()=>
        {
          this.submitted=false
        }
      })
    }
    else
    {
      console.log("not valid")
    }
  }

  get email ()
  {
    return this.loginForm.get("email")
  }

  get password ()
  {
    return this.loginForm.get("password")
  }
}
