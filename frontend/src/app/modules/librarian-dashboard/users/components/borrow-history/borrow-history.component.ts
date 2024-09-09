import { Component, Input } from '@angular/core';
import { UserProfileService } from '../../../../../services/user-profile.service';
import { GetBorrowedBookForUser } from '../../../../../interfaces/book/GetBorrowedBookForUser';
import { PaginationDto } from '../../../../../interfaces/common/PaginationDto';

@Component({
  selector: 'app-borrow-history',
  templateUrl: './borrow-history.component.html',
  styleUrl: './borrow-history.component.scss'
})
export class BorrowHistoryComponent {
  constructor(private userProfileService: UserProfileService){}

  books!: PaginationDto<GetBorrowedBookForUser>
  first : number=0
  rows:number=6
  rowsPerPageOptions = [6,12,18];
  loading:boolean=true

  @Input() id!:string;

  ngOnInit()
  {
    this.getNewPage()
  }

  getNewPage()
  {
    this.loading=true
    const pageNumber = Math.floor(this.first / this.rows) + 1;
    this.userProfileService.getBorrowHistoryForUser(this.rows,pageNumber,this.id).subscribe({
      next:(res)=>
      {
        this.books=res
        this.first= (this.books.pageNumber -1)*this.books.pageSize
        this.loading=false
      },
      error:(err)=>{}
    })
  }

  onPageChange(event:any)
  {
    this.rows=event.rows
    this.first=event.first
    this.getNewPage()
  }


}
