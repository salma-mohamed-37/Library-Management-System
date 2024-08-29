import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { UpdateUserComponent } from './components/update-user/update-user.component';

const routes: Routes = [
  {
    path:"info",
    component:UserInfoComponent
  },
  {
    path:"info/:id",
    component:UserInfoComponent
  },
  {
    path:"update/:id",
    component:UpdateUserComponent
  },
  {
    path:"update",
    component:UpdateUserComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SharedRoutingModule { }
