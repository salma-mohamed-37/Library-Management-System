import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FileSelectEvent, FileUpload, FileUploadEvent } from 'primeng/fileupload';
import { Password } from 'primeng/password';
import { UserService } from '../../../../../services/user.service';
import { dateValidator, passwordMatch } from './validators';
import { Router } from '@angular/router';


@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent {

  constructor(private fb:FormBuilder, private userService:UserService, private router:Router){}

  addForm!:FormGroup
  imageSrc: string | ArrayBuffer | null = null;


  ngOnInit()
  {
    this.addForm=this.fb.group({
      firstName :['', [Validators.required, Validators.pattern('^[a-zA-Z]+$')]],
      lastName:['', [Validators.required, Validators.pattern('^[a-zA-Z]+$')]],
      username:['', Validators.required],
      email:['', [Validators.required, Validators.email]],
      dateOfBirth:['', [Validators.required, dateValidator()]],
      gender:['', Validators.required],
      city:['', [Validators.required, Validators.pattern('^[a-zA-Z]+$')]],
      phoneNumber:['', [Validators.pattern('^[0-9]+$')]],
      password:['', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$')]],
      confirmPassword:['', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]+$')]],
      imageFile:['', Validators.required]
    }, {validator: passwordMatch('password', 'confirmPassword') })
  }

  submit()
  {
    const notToAdd=["firstName",'lastName','confirmPassword']

    if (this.addForm.valid)
    {
      const fullName = this.addForm.get('firstName')?.value+" "+this.addForm.get('lastName')?.value
      const formData = new FormData();
      formData.append('fullName',fullName)

      // Append form values
      for (const key in this.addForm.controls)
      {
        const value = this.addForm.get(key)?.value;

        if (notToAdd.includes(key))
        {
          continue;
        }
        else if (key == "dateOfBirth")
        {
          formData.append(key, value.toISOString())
        }
        else
        {
          formData.append(key, value);
        }
      }
      for (var pair of formData.entries()) {
        console.log(pair[0]+ ', ' + pair[1]);
      }
      this.userService.addUser(formData).subscribe({
        next:(res)=>
        {
            this.router.navigate(["pages/dashboard/users/all"])
        }
      })
    }
    else
    {
      console.log(this.addForm.errors)
      Object.keys(this.addForm.controls).forEach(key => {
        const controlErrors = this.addForm.get(key)?.errors;
        console.log(controlErrors)
      })
    }
  }
  onImageSelect(event: FileSelectEvent)
  {
    const selectedFile = event.files[0];

    const reader = new FileReader();

      reader.onload = () => {
        this.imageSrc = reader.result;
      };

      reader.readAsDataURL(selectedFile);
      this.addForm.get('imageFile')!.setValue(selectedFile);
  }

  get firstName()
  {
    return this.addForm.get('firstName')
  }

  get lastName()
  {
    return this.addForm.get('lastName')
  }

  get gender()
  {
    return this.addForm.get('gender')
  }

  get city()
  {
    return this.addForm.get('city')
  }

  get phoneNumber()
  {
    return this.addForm.get('phoneNumber')
  }

  get dateOfBirth()
  {
    return this.addForm.get('dateOfBirth')
  }

  get email()
  {
    return this.addForm.get('email')
  }

  get username()
  {
    return this.addForm.get('username')
  }

  get password()
  {
    return this.addForm.get('password')
  }

  get confirmPassword()
  {
    return this.addForm.get('confirmPassword')
  }

  get image()
  {
    return this.addForm.get('imageFile')
  }
}
function res(value: Object): void {
  throw new Error('Function not implemented.');
}

