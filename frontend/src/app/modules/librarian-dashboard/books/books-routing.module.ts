import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddBookComponent } from './components/add-book/add-book.component';
import { AllBooksComponent } from './components/all-books/all-books.component';

const routes: Routes = [
  {
    path:"add",
    component:AddBookComponent
  },
  {
    path:"",
    component:AllBooksComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
