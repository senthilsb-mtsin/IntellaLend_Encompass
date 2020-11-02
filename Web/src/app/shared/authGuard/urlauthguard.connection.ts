import { SessionHelper } from '@mts-app-session';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class Auth {
  checkUrlRoute(routeData: any): boolean {
    let AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;

    if (AccessCheck !== null) {
      AccessUrls.forEach(element => {
        if (element.URL === routeData) {
          AccessCheck = true;
          return false;
        }
      });
    }
    return AccessCheck;
  }
}
