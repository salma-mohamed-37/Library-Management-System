import { Component } from '@angular/core';
import { UserDto } from '../../../../../interfaces/User/UserDto';
import { FilterMatchMode } from 'primeng/api';
import { LazyLoadEventDTO } from '../../../../../interfaces/common/TableDtos';
import { PaginationDto } from '../../../../../interfaces/common/PaginationDto';
import { UserService } from '../../../../../services/user.service';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrl: './all-users.component.scss'
})
export class AllUsersComponent {
  users : UserDto[]=[]
  matchModeOptions = [
    { label: 'Contains', value: FilterMatchMode.CONTAINS},
  ];

  first:number =1
  rows:number=5
  loading:boolean=true

  response:PaginationDto<UserDto>={
    pageNumber:0,
    pageSize:0,
    count:0,
    data:[]
  }
  constructor(private userService:UserService){}
  ngOnInit()
  {
    this.getUsers()
  }

  getUsers()
  {
    const pageNumber = Math.floor(this.first / this.rows) + 1;
    console.log(pageNumber)

    this.userService.getUsers(this.rows,pageNumber).subscribe({
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
}
