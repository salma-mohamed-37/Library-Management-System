import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { MenuModule } from 'primeng/menu';
import { DeletedComponent } from "../deleted/deleted.component";


@NgModule({
  declarations: [UserInfoComponent],
  imports: [
    CommonModule,
    SharedRoutingModule,
    MenuModule,
    DeletedComponent
],
exports:[
  UserInfoComponent
]
})
export class SharedModule { }
