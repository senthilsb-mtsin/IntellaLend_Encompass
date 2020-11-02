import { AuthInterceptorService } from './shared/resolvers/http.resolver';
import { environment } from './../environments/environment';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { JwtModule } from '@auth0/angular-jwt';

import { tokenGetter } from '@mts-functions/token-getter.function';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { APIService } from './shared/service/api.service';
import { SessionHelper } from './shared/model/session-models/app-session';
import { CustomExceptionHandler } from './shared/handlers/ErrorHandler';
import { LoginApiUrlConstant, BlackListApiUrlConstant } from '@mts-api-url';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { ToastrModule } from 'ngx-toastr';
import { EmailCheckPipe } from '@mts-pipe';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        skipWhenExpired: true,
        whitelistedDomains: [environment.whiteListDomain],
        blacklistedRoutes: [
          environment.apiURL + BlackListApiUrlConstant.ERROR_HANDLER,
          environment.apiURL + LoginApiUrlConstant.LOGIN_SUBMIT,
        ],
      },
    }),
    ToastrModule.forRoot(),
    MalihuScrollbarModule.forRoot()
  ],
  providers: [
    { provide: ErrorHandler, useClass: CustomExceptionHandler },
    APIService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },
    SessionHelper,
    EmailCheckPipe
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
