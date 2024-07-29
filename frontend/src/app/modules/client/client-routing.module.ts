import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './components/home-page/home-page.component';
import { MoreBooksComponent } from './components/more-books/more-books.component';


const routes: Routes = [
  {
    path:"home-page",
    component: HomePageComponent
  },
  {
    path:"more/:pageNumber",
    component: MoreBooksComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientRoutingModule { }
