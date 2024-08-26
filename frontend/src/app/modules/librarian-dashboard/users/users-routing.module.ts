import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { BorrowComponent } from './components/borrow/borrow.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { UserDetailsComponent } from './components/user-details/user-details.component';

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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
