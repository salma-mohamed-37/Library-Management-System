import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes =
[
  {
    path: "",
    component: HomeComponent
  },
  {
    path:"users",
    loadChildren : () => import ('./users/users.module').then((m)=>m.UsersModule)
  },
]
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule]
})
export class LibrarianDashboardRoutingModule { }
