import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientRoutingModule } from './client-routing.module';
import { BookCardComponent } from './components/book-card/book-card.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { DialogModule } from 'primeng/dialog';
import { MoreBooksComponent } from './components/more-books/more-books.component';


@NgModule({
  declarations: [
    BookCardComponent,
    HomePageComponent,
    BookDetailsComponent,
    MoreBooksComponent
  ],
  imports: [
    CommonModule,
    ClientRoutingModule,
    DialogModule,


  ]
})
export class ClientModule { }
