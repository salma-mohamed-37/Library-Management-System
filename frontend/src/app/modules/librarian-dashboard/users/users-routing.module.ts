import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { BorrowComponent } from './components/borrow/borrow.component';

const routes: Routes = [
  {
    path:"all",
    component:AllUsersComponent
  },
  {
    path:"borrow",
    component:BorrowComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
