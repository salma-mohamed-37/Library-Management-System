import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { AllBooksComponent } from './components/all-books/all-books.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { FileUploadModule } from 'primeng/fileupload';
import { CalendarModule } from 'primeng/calendar';
import { ReactiveFormsModule } from '@angular/forms';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';


@NgModule({
  declarations: [
    AllBooksComponent,
    AddBookComponent
  ],
  imports: [
    CommonModule,
    BooksRoutingModule,
    FileUploadModule,
    CalendarModule,
    ReactiveFormsModule,
    PaginatorModule,
    TableModule,
  ]
})
export class BooksModule { }
