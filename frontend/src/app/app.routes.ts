import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path:"pages",
    loadChildren : () => import ('./modules/modules.module').then((m)=>m.ModulesModule)
  },
  { path: '', redirectTo: 'pages/client/home-page', pathMatch: 'full' },
  { path: '**', redirectTo: 'pages/client/home-page' }
];
