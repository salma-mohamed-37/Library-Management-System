import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { AllUsersComponent } from './components/all-users/all-users.component';
import { TableModule } from 'primeng/table';


@NgModule({
  declarations: [
    AllUsersComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    TableModule
  ]
})
export class UsersModule { }
