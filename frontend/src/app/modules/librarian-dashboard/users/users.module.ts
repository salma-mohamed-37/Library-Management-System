import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from './users-routing.module';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { TableModule } from 'primeng/table';
import { BorrowComponent } from './components/borrow/borrow.component';
import { PickListModule } from 'primeng/picklist';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PaginatorModule } from 'primeng/paginator';
import { SidebarModule } from 'primeng/sidebar';
import { CheckboxModule } from 'primeng/checkbox';
import { AddUserComponent } from './components/add-user/add-user.component';
import { FileUploadModule } from 'primeng/fileupload';
import { CalendarModule } from 'primeng/calendar';
import { ReactiveFormsModule } from '@angular/forms';
import { UserDetailsComponent } from './components/user-details/user-details.component';


@NgModule({
  declarations: [
    AllUsersComponent,
    BorrowComponent,
    AddUserComponent,
    UserDetailsComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    TableModule,
    PickListModule,
    DragDropModule,
    PaginatorModule,
    SidebarModule,
    CheckboxModule,
    FileUploadModule,
    CalendarModule,
    ReactiveFormsModule
  ]
})
export class UsersModule { }
