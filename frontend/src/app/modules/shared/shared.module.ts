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
import { BorrowCardComponent } from './components/borrow-card/borrow-card.component';
import { OverlayPanelModule } from 'primeng/overlaypanel';


@NgModule({
  declarations: [UserInfoComponent, UpdateUserComponent, BorrowCardComponent],
  imports: [
    CommonModule,
    SharedRoutingModule,
    MenuModule,
    DeletedComponent,
    DialogModule,
    FileUploadModule,
    CalendarModule,  
    ReactiveFormsModule,
    OverlayPanelModule,
],
exports:[
  UserInfoComponent,
  BorrowCardComponent
]
})
export class SharedModule { }
