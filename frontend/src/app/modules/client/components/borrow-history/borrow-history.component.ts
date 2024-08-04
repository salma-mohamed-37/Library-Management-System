import { Component } from '@angular/core';
import { UserProfileService } from '../../../../services/user-profile.service';
import { PaginationDto } from '../../../../interfaces/common/PaginationDto';
import { GetBorrowedBookForUser } from '../../../../interfaces/book/GetBorrowedBookForUser';
import { Paginator } from 'primeng/paginator';

@Component({
  selector: 'app-borrow-history',
  templateUrl: './borrow-history.component.html',
  styleUrl: './borrow-history.component.scss'
})
export class BorrowHistoryComponent {
  constructor(private userProfileService: UserProfileService){}

  books!: PaginationDto<GetBorrowedBookForUser>
  first : number=0
  rowsPerPageOptions = [4,10, 20, 30];

  ngOnInit()
  {
    this.getNewPage(4,1)
  }

  getNewPage(pageSize:number, pageNumber:number)
  {
    this.userProfileService.getMyBorrowHistory(pageSize,pageNumber).subscribe({
      next:(res)=>
      {
        this.books=res
        this.first= (this.books.pageNumber -1)*this.books.pageSize
      },
      error:(err)=>{}
    })
  }

  onPageChange(event:any)
  {
    console.log(event)
  }

}
