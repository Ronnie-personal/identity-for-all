import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { HomeComponent } from './home/home.component';
import { BlobViewComponent } from './blob-view/blob-view.component';
import { CloseAccountComponent } from './close-account/close-account.component';
const routes: Routes = [
  {
    path: 'blob-view',
    component: BlobViewComponent,
    canActivate: [MsalGuard]
  },
  {
    path: 'close-account',
    component: CloseAccountComponent,
    canActivate: [MsalGuard]
  },
  {
    path: '',
    component: HomeComponent
  },  
  {
    // Needed for hash routing
    path: 'error',
    component: HomeComponent
  },
  {
    // Needed for hash routing
    path: 'state',
    component: HomeComponent
  },
  {
    // Needed for hash routing
    path: 'code',
    component: HomeComponent
  }
];

const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true, initialNavigation: !isIframe ? 'enabledNonBlocking' : 'disabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
