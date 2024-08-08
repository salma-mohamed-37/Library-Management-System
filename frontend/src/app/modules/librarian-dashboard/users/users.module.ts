import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { TableModule } from 'primeng/table';
import { BorrowComponent } from './components/borrow/borrow.component';
import { PickListModule } from 'primeng/picklist';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PaginatorModule } from 'primeng/paginator';


@NgModule({
  declarations: [
    AllUsersComponent,
    BorrowComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    TableModule,
    PickListModule,
    DragDropModule,
    PaginatorModule
  ]
})
export class UsersModule { }
