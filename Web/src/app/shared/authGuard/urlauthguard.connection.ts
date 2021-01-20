import { SessionHelper } from '@mts-app-session';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class Auth {
  checkUrlRoute(routeData: any): boolean {
    let AccessCheck = false;
    const AccessUrls = SessionHelper.RoleDetails.URLs;

    if (AccessCheck !== null) {
      AccessUrls.forEach(element => {
        if (element.URL.toString().includes(routeData)) {
          AccessCheck = true;
          return false;
        }
      });
    }
    return AccessCheck;
  }
}
