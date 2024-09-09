import { Component } from '@angular/core';
import { UserDto } from '../../../../../interfaces/User/UserDto';
import { FilterMatchMode } from 'primeng/api';
import { PaginationDto } from '../../../../../interfaces/common/PaginationDto';
import { UserService } from '../../../../../services/user.service';
import { Router } from '@angular/router';
import { BorrowService } from '../../../../../services/borrow.service';
import { Book } from '../../../../../interfaces/book/Book';
import { UserProfileService } from '../../../../../services/user-profile.service';
import { environment } from '../../../../../../environments/environment';
import { SearchforUser} from '../../../../../interfaces/User/searchByname';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrl: './all-users.component.scss'
})
export class AllUsersComponent {

  first:number =1
  rows:number=5
  loading:boolean=true

  request: SearchforUser = {
    name: '',
    isDeleted: null
  };

  response:PaginationDto<UserDto>={
    pageNumber:0,
    pageSize:0,
    count:0,
    data:[]
  }

  returnVisible:boolean=false
  books:Book[]=[]
  returnedBooks:any[]=[]
  checked: boolean = false;


  constructor(private userProfileService:UserProfileService, private userService:UserService, private router:Router, private borrowService:BorrowService){}
  ngOnInit()
  {
    this.getUsers()
  }

  getUsers()
  {
    const pageNumber = Math.floor(this.first / this.rows) + 1;

    this.userService.getUsers(this.rows,pageNumber, this.request).subscribe({
      next:(res)=>
      {
        this.response=res
        this.loading=false
      }
    })
  }

  onPageChange(event:any)
  {
    this.first=event.first
    this.rows=event.rows
    this.getUsers()
  }

  search(form:NgForm)
  {
    this.getUsers()
  }

  navigate(url:string)
  {
    this.router.navigate([url])
  }

  borrow(id:string)
  {
    this.borrowService.userId= id
    this.router.navigate(["pages/dashboard/users/borrow"])
  }

  return(userId:string)
  {
    this.borrowService.clear()
    this.borrowService.userId= userId
    this.returnVisible=true
      this.userProfileService.getCurrentlyBorrowedBooks(userId).subscribe({
        next:(res)=>
        {
          this.books=res
        }
      })
  }

  returnBooks()
  {
    this.returnVisible=false
    var ids = this.returnedBooks.map(b=>b.id)
    var uniqueIds = [...new Set(ids)];
    this.borrowService.booksIds=uniqueIds
    this.borrowService.return().subscribe({
      next:(res)=>
      {
        this.borrowService.clear()
        this.returnedBooks=[]
      }
    })
    
  }

  getFullImageUrl(path?: string): string
  {
    return `${environment.apiUrl}${path}`;
  }
}
