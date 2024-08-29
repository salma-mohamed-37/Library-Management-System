import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedRoutingModule } from './shared-routing.module';
import { UserInfoComponent } from './components/user-info/user-info.component';
import { MenuModule } from 'primeng/menu';
import { DeletedComponent } from "../deleted/deleted.component";
import { DialogModule } from 'primeng/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { UpdateUserComponent } from './components/update-user/update-user.component';
import { FileUploadModule } from 'primeng/fileupload';
import { CalendarModule } from 'primeng/calendar';


@NgModule({
  declarations: [UserInfoComponent, UpdateUserComponent],
  imports: [
    CommonModule,
    SharedRoutingModule,
    MenuModule,
    DeletedComponent,
    DialogModule,
    FileUploadModule,
    CalendarModule,  
    ReactiveFormsModule
],
exports:[
  UserInfoComponent
]
})
export class SharedModule { }
