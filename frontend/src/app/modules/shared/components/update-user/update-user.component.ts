import { Component, Input } from '@angular/core';
import { UserDto } from '../../../../interfaces/User/UserDto';
import { UserService } from '../../../../services/user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastContentService } from '../../../../services/toast-content.service';
import { dateValidator, passwordMatch } from '../../../librarian-dashboard/users/components/add-user/validators';
import { FileSelectEvent } from 'primeng/fileupload';
import { environment } from '../../../../../environments/environment';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.scss'
})
export class UpdateUserComponent {
  @Input() inId? :string;
  user!:UserDto;
  userId!:string |undefined
  updateForm!:FormGroup
  imageSrc: string | ArrayBuffer | null = null;


  constructor(private userService:UserService,public route:ActivatedRoute, private router:Router, private fb:FormBuilder, private toastContnentService : ToastContentService){}


  ngOnInit()
  {
    if (! this.inId)
    {
      this.route.paramMap.subscribe(params => {
        this.userId = params.get('id')||undefined;
      })
    }
    
    else
    {
      this.userId= this.inId
    }
    this.userService.getUserbyID(this.userId).subscribe({
      next:(res)=>
      {
        this.user = res;
        const dateOfBirth = new Date(this.user.dateOfBirth);
        this.updateForm = this.fb.group({
          firstName: new FormControl(this.user.fullname.split(" ")[0], [Validators.required, Validators.pattern('^[a-zA-Z]+$')]),
          lastName: new FormControl(this.user.fullname.split(" ")[1], [Validators.required, Validators.pattern('^[a-zA-Z]+$')]), 
          
          dateOfBirth: new FormControl(dateOfBirth, [Validators.required, dateValidator()]),
          gender: new FormControl(this.user.gender, Validators.required),
          city: new FormControl(this.user.city, [Validators.required, Validators.pattern('^[a-zA-Z]+$')]),
          phoneNumber:new FormControl(this.user.phoneNumber, Validators.pattern('^[0-9]+$')),
          
          imageFile: new FormControl('')
        });
      },
      error:(err)=>
      {
      }
    })
  }


  onImageSelect(event: FileSelectEvent)
  {
    const selectedFile = event.files[0];

    const reader = new FileReader();

      reader.onload = () => {
        this.imageSrc = reader.result;
      };

      reader.readAsDataURL(selectedFile);
      this.updateForm.get('imageFile')!.setValue(selectedFile);
  }

  get firstName()
  {
    return this.updateForm.get('firstName')
  }

  get lastName()
  {
    return this.updateForm.get('lastName')
  }

  get gender()
  {
    return this.updateForm.get('gender')
  }

  get city()
  {
    return this.updateForm.get('city')
  }

  get phoneNumber()
  {
    return this.updateForm.get('phoneNumber')
  }

  get dateOfBirth()
  {
    return this.updateForm.get('dateOfBirth')
  }

  get image()
  {
    return this.updateForm.get('imageFile')
  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }

  
  submit()
  {
    const notToAdd=["firstName",'lastName','confirmPassword']

    if (this.updateForm.valid)
    {
      const fullName = this.updateForm.get('firstName')?.value+" "+this.updateForm.get('lastName')?.value
      const formData = new FormData();
      formData.append('fullName',fullName)

      // Append form values
      for (const key in this.updateForm.controls)
      {
        const value = this.updateForm.get(key)?.value;

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
     
      this.userService.updateUser(formData, this.userId).subscribe({
        next:(res)=>
        {
            
        }
      })
    }
    else
    {
     
    }
  }
}
