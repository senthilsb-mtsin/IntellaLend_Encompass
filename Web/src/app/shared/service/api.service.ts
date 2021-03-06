import { Router } from '@angular/router';
import {
  MTSAPIResponse,
  MTSAPIExceptionResponse,
} from '@mts-api-response-model';
import { environment } from './../../../environments/environment';
import { Injectable, Injector, NgZone } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { BlackListApiUrlConstant, LoginApiUrlConstant } from '@mts-api-url';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ToastrService } from 'ngx-toastr';
import { AppSettings } from '../constant/app-setting-constants/app-setting.constant';
import { SessionHelper } from '@mts-app-session';

const jwtHelper = new JwtHelperService();
const headers = new HttpHeaders().set('Content-Type', 'application/json');
@Injectable()
export class APIService {

  constructor(private http: HttpClient, private injector: Injector, private _route: Router) { }
  private tokenExpirationTimer: any;
  private _toastService?: ToastrService;

  Post(URL: string, input: any): Observable<MTSAPIResponse> {
    return this.http
      .post<MTSAPIResponse>(environment.apiURL + URL, input, { headers })
      .pipe(
        map((response) => {
          return new MTSAPIResponse(
            response['token'],
            response['data'],
            response['response-message[\'message-id\']'],
            response['response-message[\'message-desc\']']
          );
        }),
        tap((resData) => {
          this.handleAuthentication(resData);
        })
      );
  }

  Get(URL: string) {
    return this.http
      .get<MTSAPIResponse>(environment.apiURL + URL, { headers })
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  GetWithInput(URL: string, input: any) {
    return this.http
      .post<MTSAPIResponse>(environment.apiURL + URL, input, { headers })
      .pipe(
        map((response) => {
          if (response['token'] === null) {
            throw new Error('Token is NULL for the request(' + URL + '). Check API for further logs.');
          } else {
            return new MTSAPIResponse(
              response['token'],
              response['data'],
              response['response-message[\'message-id\']'],
              response['response-message[\'message-desc\']']
            );
          }
        })
      );
  }

  authHttpPost(URL: string, input: any) {
    if (jwtHelper.isTokenExpired(localStorage.getItem('id_token'))) {
      this._route.navigate(['']);
      AppSettings.SessionErrorMsg = false;
    } else {
      const browserBaseUrl = window.location.href;
      const inputReq = this.AppendUserDetails(input);
      const inputReqWithUser = { Error: { message: 'Request Logging'+', BrowserUrl : '+browserBaseUrl, stack: JSON.stringify(inputReq) } };
      this.Post(BlackListApiUrlConstant.ERROR_HANDLER, inputReqWithUser).subscribe();
      return this.http
        .post<MTSAPIResponse>(environment.apiURL + URL, input, { headers, observe: 'response' })
        .pipe(
          map((response) => {
            if (response.status === 401 || response.status === 0) {
              if (!isTruthy(this._toastService)) {
                this._toastService = this.injector.get(ToastrService);
              }
              if (AppSettings.SessionErrorMsg) {
                this._toastService.error('Session is no longer active', null, { onActivateTick: true });
              }
              this._route.navigate(['']);
              AppSettings.SessionErrorMsg = false;
            }
            if (response.body['token'] === null) {
              throw new Error('Token is NULL for the request(' + URL + '). Check API for further logs.');
            } else {
              return new MTSAPIResponse(
                response.body['token'],
                response.body['data'],
                response.body['response-message[\'message-id\']'],
                response.body['response-message[\'message-desc\']']
              );
            }
          }),
          catchError((err) => {
            if (err.status === 401 || err.status === 0) {
              if (!isTruthy(this._toastService)) {
                this._toastService = this.injector.get(ToastrService);
              }
              if (AppSettings.SessionErrorMsg) {
                this._toastService.error('Session is no longer active', null, { onActivateTick: true });
              }
              this._route.navigate(['']);
              AppSettings.SessionErrorMsg = false;
            }
            return throwError(err);
          }),
          tap((resData) => {
            this.handleAuthentication(resData);
          })
        );
    }
  }
  private AppendUserDetails(input: any) {
    if (localStorage.getItem('userDetails') === null) {
      const routerService = this.injector.get(Router);
      const ngZone = this.injector.get(NgZone);
      ngZone.run(() => {
        routerService.navigate(['']);
      });
    }
    else
      SessionHelper.setUserSessionVariables();

    if (isTruthy(SessionHelper.UserDetails)) {
      let requestObj = input;
      if (this.GetObjectType(input) === 'String') {
        requestObj = JSON.parse(input);
      }
      const RequestUserInfo = {
        RequestUserID: SessionHelper.UserDetails.UserID,
        RequestUserTableSchema: AppSettings.TenantSchema,
      };
      requestObj['RequestUserInfo'] = RequestUserInfo;
      input = JSON.stringify(requestObj);
    }

    return input;
  }
  private GetObjectType(object) {
    const stringConstructor = 'test'.constructor;
    const arrayConstructor = [].constructor;
    const objectConstructor = {}.constructor;

    if (object === null) {
      return 'null';
    } else if (object === undefined) {
      return 'undefined';
    } else if (object.constructor === stringConstructor) {
      return 'String';
    } else if (object.constructor === arrayConstructor) {
      return 'Array';
    } else if (object.constructor === objectConstructor) {
      return 'Object';
    } else {
      return 'Unknown';
    }
  }

  authHttpGet(URL: string) {
    if (jwtHelper.isTokenExpired(localStorage.getItem('id_token'))) {
      this._route.navigate(['']);
      AppSettings.SessionErrorMsg = false;
    } else {
      return this.http
        .get<MTSAPIResponse>(environment.apiURL + URL, { headers, observe: 'response' })
        .pipe(
          map((response) => {
            if (response.status === 401 || response.status === 0) {
              if (!isTruthy(this._toastService)) {
                this._toastService = this.injector.get(ToastrService);
              }
              if (AppSettings.SessionErrorMsg) {
                this._toastService.error('Session is no longer active', null, { onActivateTick: true });
              }
              this._route.navigate(['']);
              AppSettings.SessionErrorMsg = false;
            }
            if (response.body['token'] === null) {
              throw new Error('Token is NULL for the request(' + URL + '). Check API for further logs.');
            } else {
              return new MTSAPIResponse(
                response.body['token'],
                response.body['data'],
                response.body['response-message[\'message-id\']'],
                response.body['response-message[\'message-desc\']']
              );
            }
          }),
          catchError((err) => {
            if (err.status === 401 || err.status === 0) {
              if (!isTruthy(this._toastService)) {
                this._toastService = this.injector.get(ToastrService);
              }
              if (AppSettings.SessionErrorMsg) {
                this._toastService.error('Session is no longer active', null, { onActivateTick: true });
              }
              this._route.navigate(['']);
              AppSettings.SessionErrorMsg = false;
            }
            return throwError(err);
          }),
          tap((resData) => {
            this.handleAuthentication(resData);
          })
        );
    }
  }

  authHttpPostWithoutParse(URL: string, input: any): Observable<any> {
    if (jwtHelper.isTokenExpired(localStorage.getItem('id_token'))) {
      this._route.navigate(['']);
      AppSettings.SessionErrorMsg = false;
    } else {
      return this.http.post(environment.apiURL + URL, input, {
        headers,
        responseType: 'blob',
      });
    }
  }

  authHttpGetWithoutParse(URL: string): Observable<any> {
    if (jwtHelper.isTokenExpired(localStorage.getItem('id_token'))) {
      this._route.navigate(['']);
      AppSettings.SessionErrorMsg = false;
    } else {
      return this.http.get(environment.apiURL + URL, {
        headers,
        responseType: 'blob',
      });
    }
  }

  autoLogout(expirationDuration: number) {
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
    }
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  logout() {
    const reqBody = { TableSchema: AppSettings.TenantSchema, UserID: SessionHelper.UserDetails.UserID, Lock: false };
    this.http
    .post<MTSAPIResponse>(environment.apiURL + LoginApiUrlConstant.UNLOCK_USER, reqBody, { headers, observe: 'response' })
    .pipe(
      map((response) => {
          if(response.body['data'] == 'True'){
            this._route.navigate(['']);
            AppSettings.SessionErrorMsg = false;
            if (this.tokenExpirationTimer) {
              clearTimeout(this.tokenExpirationTimer);
            }
            this.tokenExpirationTimer = null;
          }
      })
    ).subscribe(() => {this._route.navigate(['']);});
    //this._commonService.UnLock();
  }

  sendOffLineError() {
    if (localStorage.getItem('offlineError') !== null) {
      this.http
        .post<MTSAPIExceptionResponse>(
          BlackListApiUrlConstant.ERROR_HANDLER,
          localStorage.getItem('offlineError'),
          { headers }
        )
        .subscribe(
          (res) => {
            if (res.Status) { localStorage.removeItem('offlineError'); }
          },
          (err) => {
            console.clear();
          }
        );
    }
  }

  private handleAuthentication(response: MTSAPIResponse) {
    const token = jwtHelper.decodeToken(response.Token);
    if (isTruthy(token)) {
      this.autoLogout((token.expMin * 60) * 1000);
      localStorage.setItem('id_token', response.Token);
    } else {
      if (this.tokenExpirationTimer) {
        clearTimeout(this.tokenExpirationTimer);
      }
      this.tokenExpirationTimer = null;
    }
  }
}
