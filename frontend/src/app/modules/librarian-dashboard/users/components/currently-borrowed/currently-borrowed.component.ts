import { Component, Input } from '@angular/core';
import { UserProfileService } from '../../../../../services/user-profile.service';
import { GetBorrowedBookForUser } from '../../../../../interfaces/book/GetBorrowedBookForUser';

@Component({
  selector: 'app-currently-borrowed',
  templateUrl: './currently-borrowed.component.html',
  styleUrl: './currently-borrowed.component.scss'
})
export class CurrentlyBorrowedComponent {
  constructor(private userProfileService: UserProfileService){}

  books!: GetBorrowedBookForUser []
  loading:boolean=true

  @Input() id!:string;

  ngOnInit()
  {
    this.userProfileService.getCurrentlyBorrowedBooks(this.id).subscribe({
      next:(res)=>
      {
        this.books=res
        this.loading=false
      },
      error:(err)=>{}
    })
  }

}
