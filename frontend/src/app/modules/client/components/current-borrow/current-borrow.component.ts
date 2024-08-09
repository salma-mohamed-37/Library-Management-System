import { Component } from '@angular/core';
import { UserProfileService } from '../../../../services/user-profile.service';
import { GetBorrowedBookForUser } from '../../../../interfaces/book/GetBorrowedBookForUser';

@Component({
  selector: 'app-current-borrow',
  templateUrl: './current-borrow.component.html',
  styleUrl: './current-borrow.component.scss'
})
export class CurrentBorrowComponent {
  constructor(private userProfileService: UserProfileService){}

  books!: GetBorrowedBookForUser []
  loading:boolean=true

  ngOnInit()
  {
    this.userProfileService.getMyCurrentlyBorrowedBooks().subscribe({
      next:(res)=>
      {
        this.books=res
        this.loading=false
      },
      error:(err)=>{}
    })
  }
}
