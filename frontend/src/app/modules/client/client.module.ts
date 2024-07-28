import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientRoutingModule } from './client-routing.module';
import { BookCardComponent } from './components/book-card/book-card.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { BookDetailsComponent } from './components/book-details/book-details.component';
import { DialogModule } from 'primeng/dialog';

@NgModule({
  declarations: [
    BookCardComponent,
    HomePageComponent,
    BookDetailsComponent
  ],
  imports: [
    CommonModule,
    ClientRoutingModule,
    DialogModule

  ]
})
export class ClientModule { }
