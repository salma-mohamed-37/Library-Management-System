import { Component } from '@angular/core';
import { FileSelectEvent } from 'primeng/fileupload';
import { AuthorService } from '../../../../../services/author.service';
import { CategoryService } from '../../../../../services/category.service';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { dateValidator } from '../../../users/components/add-user/validators';
import { BookService } from '../../../../../services/book.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrl: './add-book.component.scss'
})
export class AddBookComponent {
  imageSrc: string | ArrayBuffer | null = null;
  addForm!:FormGroup

  genres: { label: string, value: number }[] = [{ label: "Select genre", value: null as unknown as number }];
  authors: { label: string, value: number }[] = [{ label: "Select author", value: null as unknown as number }];



  ngOnInit()
  {
    this.addForm = this.fb.group({
      name: new FormControl('', [Validators.required]),
      categoryId: new FormControl('', [Validators.required]), 
      authorId: new FormControl('', Validators.required),
      publishDate: new FormControl('', [Validators.required, dateValidator()]),
      coverFile: new FormControl('', Validators.required)
    })

    this.getCategories()
    this.getAuthors()
  }

  constructor(private authorsService : AuthorService, private categoryService:CategoryService, private fb:FormBuilder, private bookService:BookService, private router:Router ){}


  onImageSelect(event: FileSelectEvent)
  {
    const selectedFile = event.files[0];

    const reader = new FileReader();

      reader.onload = () => {
        this.imageSrc = reader.result;
      };

      reader.readAsDataURL(selectedFile);
      this.addForm.get('coverFile')!.setValue(selectedFile);
  }


  getCategories()
  {
    this.categoryService.getCategories().subscribe({
      next:(res)=>
      {
        this.genres.push(...res.map(category => ({ label: category.name, value: category.id })))
      }
    })
  }

  getAuthors()
  {
    this.authorsService.getAuthors().subscribe({
      next:(res)=>
      {
        this.authors.push(...res.map(a => ({ label: a.name, value: a.id })))
      }
    })
  }

  get name()
  {
    return this.addForm.get('name')
  }

  get publishDate()
  {
    return this.addForm.get('publishDate')
  }

  get categoryId()
  {
    return this.addForm.get('categoryId')
  }

  get authorId()
  {
    return this.addForm.get('authorId')
  }

  get imageFile()
  {
    return this.addForm.get('coverFile')
  }

  submit()
  {
      if(this.addForm.valid)
      {
        const formData = new FormData();
        for (const key in this.addForm.controls)
          {
            const value = this.addForm.get(key)?.value;
    
            if (key == "publishDate")
            {
              formData.append(key, value.toISOString())
            }
            else
            {
              formData.append(key, value);
            }
          }
          for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
          }
          this.bookService.addBook(formData).subscribe({
            next:(res)=>
            {
              this.router.navigate(["pages/dashboard/books"])
            }

          })
        }

      else
      {
        Object.keys(this.addForm.controls).forEach(key => {
          const controlErrors: ValidationErrors | null = this.addForm.get(key)!.errors;
          if (controlErrors) {
            Object.keys(controlErrors).forEach(keyError => {
              console.log('Control:', key, 'Error:', keyError, 'Value:', controlErrors[keyError]);
            });
          }
        });
      }
  }


}
