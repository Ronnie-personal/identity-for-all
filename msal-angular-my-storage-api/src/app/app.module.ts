import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http"; 
import { FormsModule } from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon'
//import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
//import { TodoEditComponent } from './todo-edit/todo-edit.component';
import { BlobViewComponent } from './blob-view/blob-view.component';
import { BlobService } from './blob.service';

//import { MsalModule, MsalRedirectComponent, MsalGuard, MsalInterceptor } from '@azure/msal-angular';
import { MsalGuard, MsalInterceptor, MsalBroadcastService, MsalInterceptorConfiguration, MsalModule, MsalService, MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG, MsalGuardConfiguration, MsalRedirectComponent } from '@azure/msal-angular';
import { IPublicClientApplication,PublicClientApplication, InteractionType } from '@azure/msal-browser';

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BlobViewComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,    
    AppRoutingModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,    
    HttpClientModule,
    MatTableModule,
    MatCardModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatIconModule,
    FormsModule,    
    MsalModule.forRoot( new PublicClientApplication({
      auth: {
        clientId: '21e4f54f-5077-481d-975d-512a68885167', // Application (client) ID from the app registration
        authority: 'https://login.microsoftonline.com/<tenantId>', // The Azure cloud instance and the app's sign-in audience (tenant ID, common, organizations, or consumers)
        redirectUri: 'http://localhost:4200'// This is your redirect URI
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: isIE, // Set to true for Internet Explorer 11
      }
    }), {
      interactionType: InteractionType.Redirect, // MSAL Guard Configuration
      authRequest: {
        scopes: ['api://47fd2385-3956-4a4d-a9b2-f65ecca82348/ToDoList.Read','api://47fd2385-3956-4a4d-a9b2-f65ecca82348/ToDoList.Write']
      }
  }, {
    interactionType: InteractionType.Redirect, // MSAL Interceptor Configuration
    protectedResourceMap: new Map([ 
        ['https://localhost:7249/MyBlobFile', ['api://47fd2385-3956-4a4d-a9b2-f65ecca82348/ToDoList.Read','api://47fd2385-3956-4a4d-a9b2-f65ecca82348/ToDoList.Write']]
    ])
  })    
  ],
  providers: [    {
    provide: HTTP_INTERCEPTORS,
    useClass: MsalInterceptor,
    multi: true
  },
    MsalGuard,
    BlobService
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
