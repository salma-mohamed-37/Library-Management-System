import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientRoutingModule } from './client-routing.module';
import { BookCardComponent } from './components/book-card/book-card.component';
import { HomePageComponent } from './components/home-page/home-page.component';




@NgModule({
  declarations: [
    BookCardComponent,
    HomePageComponent

  ],
  imports: [
    CommonModule,
    ClientRoutingModule,

  ]
})
export class ClientModule { }
