import { Router } from '@angular/router';
import { ElementRef, HostListener, Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import { CommonService } from 'src/app/shared/common/common.service';
import { LayoutService } from '../../service/layout.service';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'mts-userprofile',
  templateUrl: 'userprofile.page.html',
  styleUrls: ['userprofile.page.css'],
})
export class UserprofileComponent implements OnInit {
  showHideProfile = false;
  ischangeRole = false;
  user: any;
  userCurrentRoleName: any;
  CurrentRole: any;
  userCurrentRoleID: any;
  menus: any;
  resetLink: any = 'resetpassword';
  AD_login: boolean = environment.ADAuthentication;
  constructor(
    private _route: Router,
    private _eref: ElementRef,
    private _commonService: CommonService,
    private _layoutService: LayoutService
  ) {
    this.user = SessionHelper.UserDetails;
    this.userCurrentRoleName = SessionHelper.RoleDetails.RoleDetails.RoleName;
    this.userCurrentRoleID = SessionHelper.RoleDetails.RoleDetails.RoleID;
  }
  private _subscription: Subscription[] = [];

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this._eref.nativeElement.contains(event.target)) {
      this.showHideProfile = true;
    } else {
      this.showHideProfile = false;
    }
  }

  ngOnInit() {

    this._subscription.push(this._layoutService.ischangeRole.subscribe((res: boolean) => {
      this.ischangeRole = res;
    }));

    this._subscription.push(this._layoutService.defaultRoute.subscribe((res: string) => {
      this.dashBoardRoute(res);
    }));
  }

  setMenuRouteChangeRole(roleID) {
    if (isTruthy(roleID)) {
      const input = { TableSchema: AppSettings.TenantSchema, RoleID: roleID, UserID: SessionHelper.UserDetails.UserID, ADLogin : environment.ADAuthentication };
      this._layoutService.setDefaultRoute(input);
    }
  }

  dashBoardRoute(startpage) {
    this._route.navigate(['view/' + startpage]);
    window.location.reload();
  }

  signOut() {
    this._commonService.UnLock('');
  }

  ngDestroy() {
    this._subscription.forEach(element => {
      element.unsubscribe();
    });
  }
}
