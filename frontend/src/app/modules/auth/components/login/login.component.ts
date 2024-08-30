import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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
  constructor(private fb: FormBuilder, private authService : AuthService, private router:Router ){}

  ngOnInit()
  {
    this.loginForm = this.fb.group({
      email:[''],
      password:['']
    })
  }

  submit()
  {
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
        }
      })
    }
    else
    {
      console.log("not valid")
    }

  }

}
