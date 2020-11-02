
import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
} from '@angular/common/http';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { throwError } from 'rxjs';
import { NotificationService } from '@mts-notification';
import { SessionHelper } from '@mts-app-session';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private _notificationService: NotificationService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    if (req.method === 'POST') {
      const input = this.AppendUserDetails(req.body);
      const modifiedReq = req.clone({ body: input });
      return next.handle(modifiedReq); // .pipe(catchError(this.handleError));
    } else {
      return next.handle(req); // .pipe(catchError(this.handleError));
    }
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }

    this._notificationService.showError(errorMessage);

    return throwError(errorMessage);
  }

  private AppendUserDetails(input: any) {
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
}
