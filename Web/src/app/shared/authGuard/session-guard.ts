import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate } from '@angular/router';
import { SessionHelper } from '@mts-app-session';

@Injectable({ providedIn: 'root' })
export class SessionGuard implements CanActivate {

  canActivate(route: ActivatedRouteSnapshot) {
    SessionHelper.cleanSessionVariables();
    return true;
  }
}
