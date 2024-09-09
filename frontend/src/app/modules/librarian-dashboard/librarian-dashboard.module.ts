import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { LibrarianDashboardRoutingModule } from './librarian-dashboard-routing.module';


@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    LibrarianDashboardRoutingModule,
  ]
})
export class LibrarianDashboardModule { }
