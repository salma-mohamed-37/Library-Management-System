import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { BorrowComponent } from './components/borrow/borrow.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { UserDetailsComponent } from './components/user-details/user-details.component';
import { BorrowHistoryComponent } from './components/borrow-history/borrow-history.component';
import { CurrentlyBorrowedComponent } from './components/currently-borrowed/currently-borrowed.component';

const routes: Routes = [
  {
    path:"all",
    component:AllUsersComponent
  },
  {
    path:"borrow",
    component:BorrowComponent
  },
  {
    path:"add",
    component:AddUserComponent
  },
  {
    path:"details/:id",
    component:UserDetailsComponent
  },
  {
    path:"current/:id",
    component:BorrowHistoryComponent
  },
  {
    path:"history/:id",
    component:CurrentlyBorrowedComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
