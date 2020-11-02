import { Injectable } from '@angular/core';
import { Router, ActivatedRouteSnapshot, ActivatedRoute, CanActivate } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Auth } from '@mts-auth-guard/urlauthguard.connection';
import { SessionHelper } from '@mts-app-session';
import { CommonService } from '../common/common.service';

const jwtHelper = new JwtHelperService();

@Injectable({ providedIn: 'root' })
export class ConnectionAuthGuard implements CanActivate {

  constructor(
    public _route: Router,
    public activeRoute: ActivatedRoute,
    public urlAuth: Auth,
    private _commonService: CommonService
  ) { }

  canActivate(route: ActivatedRouteSnapshot) {

    if (localStorage.getItem('id_token')) {
      if (!jwtHelper.isTokenExpired(localStorage.getItem('id_token'))) {
        SessionHelper.setSessionVariables();
        if (this.urlAuth.checkUrlRoute(route.data['routeURL'])) {
          if (route['_routerState'].url !== '/loanpopout') {
            localStorage.removeItem('loan_details');
          }
          return true;
        }
      }
    }
    this._commonService.UnLock();
    return false;
  }
}
