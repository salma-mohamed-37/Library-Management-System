import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { MenuModule } from 'primeng/menu';
import { DeletedComponent } from "../deleted/deleted.component";
import { DialogModule } from 'primeng/dialog';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [UserInfoComponent],
  imports: [
    CommonModule,
    SharedRoutingModule,
    MenuModule,
    DeletedComponent,
    DialogModule,
    ReactiveFormsModule
],
exports:[
  UserInfoComponent
]
})
export class SharedModule { }
