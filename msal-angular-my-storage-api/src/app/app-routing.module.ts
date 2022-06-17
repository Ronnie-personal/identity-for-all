import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { HomeComponent } from './home/home.component';
import { BlobViewComponent } from './blob-view/blob-view.component';

const routes: Routes = [
  {
    path: 'blob-view',
    component: BlobViewComponent,
    canActivate: [MsalGuard]
  },
  {
    path: '',
    component: HomeComponent
  },  
];

const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, { initialNavigation: !isIframe ? 'enabledNonBlocking' : 'disabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
