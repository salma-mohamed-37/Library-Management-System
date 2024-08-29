import { Routes } from '@angular/router';
import { NotFoundComponent } from './modules/not-found/not-found.component';

export const routes: Routes = [
  {
    path:"pages",
    loadChildren : () => import ('./modules/modules.module').then((m)=>m.ModulesModule)
  },
  { path: '', redirectTo: 'pages/client/home-page', pathMatch: 'full' },
  { path: '**', component:NotFoundComponent }  
];
