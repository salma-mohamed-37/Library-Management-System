import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { MoreBooksComponent } from './components/more-books/more-books.component';
import { BorrowHistoryComponent } from './components/borrow-history/borrow-history.component';
import { roleGuard } from '../auth/role.guard';
import { CurrentBorrowComponent } from './components/current-borrow/current-borrow.component';
import { ProfileComponent } from './components/profile/profile.component';


const routes: Routes = [
  {
    path:"home-page",
    component: HomePageComponent
  },
  {
    path:"more/:pageNumber",
    component: MoreBooksComponent
  },
  {
    path:"borrow-history",
    canActivate: [roleGuard],
    data: { role: 'USER' },
    component: BorrowHistoryComponent
  },
  {
    path:"current",
    canActivate: [roleGuard],
    data: { role: 'USER' },
    component: CurrentBorrowComponent
  },
  {
    path:"profile",
    component:ProfileComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule { }
