import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes =
[
  {
    path:"client",
    loadChildren : () => import ('./client/client.module').then((m)=>m.ClientModule)
  },
  {
    path:"auth",
    loadChildren : () => import ('./auth/auth.module').then((m)=>m.AuthModule)
  },
  {
    path:"dashboard",
    loadChildren : () => import ('./librarian-dashboard/librarian-dashboard.module').then((m)=>m.LibrarianDashboardModule)
  },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ModulesRoutingModule { }
