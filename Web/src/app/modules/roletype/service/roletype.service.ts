import { Injectable } from '@angular/core';
import { Subject, ReplaySubject } from 'rxjs';
import { RoleTypeDataAccess } from '../roletype.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { NotificationService } from '@mts-notification';
import { Location } from '@angular/common';
import { AddRoleTypeRequestModel } from '../models/roletype-request.model';
import { RoleTypeRequest } from '../models/table-request.model';
import { ChangeRoleRequest } from '../models/changerole.model';
import { RoleDetailsRequest } from '../models/roletypedetails.model';
import { Roletypemodel } from '../models/roletype-datatable.model';
import { ADGroupMasterModel } from '../models/adgroupmaster.model';
const jwtHelper = new JwtHelperService();

@Injectable()
export class RoleTypeService {
  setRoleAdminTableData = new Subject();
  RoleTypeRowData = new ReplaySubject(1);
  menulist$ = new Subject();
  menuArr$ = new Subject();
  ischeckuser$ = new Subject();
  isMapped$ = new Subject();
  menulist: any = [];
  AddRoleData = new Subject<AddRoleTypeRequestModel[]>();
  roleAdminData: any[] = [];
  ADGroupMasterList$ = new Subject<any>();
  _setHomePage: any;

  constructor(private _roleAdminData: RoleTypeDataAccess, private _notificationService: NotificationService, private location: Location) { }
  private _menuarr: any[];
  private addRoleData: any[] = [];
  private menuColors: any = ['txt-indigo', 'txt-orange', 'txt-green', 'txt-themeRed', 'txt-warm', 'txt-info', 'txt-brown'];

  getRoleAdminList(InputReq: RoleTypeRequest) {
    return this._roleAdminData.GetRoleAdminList(InputReq).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this.roleAdminData = data;
      this.setRoleAdminTableData.next(this.roleAdminData);
    });
  }

  getADGroupMasterList() {
    const InputReq = new RoleTypeRequest(
      AppSettings.SystemSchema,
    );
    return this._roleAdminData.GetADGroupMasterList(InputReq).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this.ADGroupMasterList$.next(data);
    });
  }
  GetMenuList() {
    const inputData = new RoleTypeRequest(
      AppSettings.TenantSchema);
    return this._roleAdminData.GetMenuList(inputData).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this.menulist = [...data];
      for (let i = 0; i < this.menulist.length; i++) {
        if (this.menulist[i].MenuGroupID > 0) {
          this.menulist[i]['color'] = this.menuColors[i % 7];
        }

        for (let x = 0; x < this.menulist[i].SubMenus.length; x++) {
          this.menulist[i].SubMenus[x]['color'] = this.menuColors[x % 7];
          if (this.menulist[i].SubMenus[x].Accesslevel === 1) {
            this.menulist[i].SubMenus[x].MenuTitle = 'Manager ' + this.menulist[i].SubMenus[x].MenuTitle;
          }
        }
      }
      this.menulist$.next([...this.menulist]);
    });
  }
  CloseRole() {
    this.location.back();
  }
  GetAddRoleSubmit(req: any) {
    this._roleAdminData.GetAddRoleSubmit(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result.Success) {
            this._notificationService.showSuccess('Role Type Added Successfully');
            this.CloseRole();
          } else {
            this._notificationService.showError('Role Type Name already exist');
          }
        }
        this.AddRoleData.next(this.addRoleData);
      });
  }
  EditRoleDetails(updaterole: RoleDetailsRequest) {
    return this._roleAdminData.GetEditRoleDetails(updaterole).subscribe(res => {
      const Data = jwtHelper.decodeToken(res.Data)['data'];
      this.menulist = [];
      this.menulist = Data.Menus;
      for (let i = 0; i < this.menulist.length; i++) {
        if (this.menulist[i].MenuGroupID > 0) {
          this.menulist[i]['color'] = this.menuColors[i % 7];
        }
        for (let x = 0; x < this.menulist[i].SubMenus.length; x++) {
          this.menulist[i].SubMenus[x]['color'] = this.menuColors[x % 7];
          this.menulist[i].SubMenus[x].Active = this.menulist[i].SubMenus[x].IsMapped;
          if (this.menulist[i].SubMenus[x].Accesslevel === 1) {
            this.menulist[i].SubMenus[x].MenuTitle = 'Manager ' + this.menulist[i].SubMenus[x].MenuTitle;
          }
          if (this.menulist[i].SubMenus[x].RouteLink === Data.RoleDetails.StartPage) {
            this.menulist[i].SubMenus[x].IsChecked = true;
          } else {
            this.menulist[i].SubMenus[x].IsChecked = false;
          }
        }
      }
      this.menulist$.next([...this.menulist]);
    }
    );
  }
  CheckUserRoleDetails(rolechange: ChangeRoleRequest) {
    this._roleAdminData.CheckUserRoleDetails(rolechange).subscribe(res => {
      if (res !== null) {
        const Data = jwtHelper.decodeToken(res.Data)['data'];
        if (Data !== null) {
          this._notificationService.showError('Role assigned to users');
          this.ischeckuser$.next();
        }
      }
    });
  }
  UpdateRoleSubmit(inputData: AddRoleTypeRequestModel) {
    this._roleAdminData.UpdateRoleDetails(inputData).subscribe(
      res => {
        if (res !== null) {
          const Data = jwtHelper.decodeToken(res.Data)['data'];
          if (Data !== null) {
            if (Data.Success === true) {
              this._notificationService.showSuccess('Role Details Updated Successfully');
              this.CloseRole();
            } else {
              this._notificationService.showError('Role Name Already Exist');
            }
          }
        }
        this.AddRoleData.next(this.addRoleData);
      });
  }
  SetTableRowData(inputData: Roletypemodel) {
    this.RoleTypeRowData.next(inputData);
  }
  validate(roletypedata: any): boolean {
    roletypedata.RoleName = roletypedata.RoleName.trim();

    const type = roletypedata.RoleID === -1 ? 'add' : 'edit';
    const menucheck = false;
    let returVal = false;
    this._menuarr = [];
    this._setHomePage = [];
    for (let x = 0; x < this.menulist.length; x++) {
      if (this.menulist[x].MenuGroupID > 0) {
        for (let m = 0; m < this.menulist[x].SubMenus.length; m++) {
          if ((type === 'add' && this.menulist[x].SubMenus[m].Active === true) || (type === 'edit' && this.menulist[x].SubMenus[m].IsMapped === true)) {
            this._menuarr.push({
              MenuID: this.menulist[x].SubMenus[m].MenuID,
              RouteLink: this.menulist[x].SubMenus[m].RouteLink
            });
          }
          if (this.menulist[x].SubMenus[m].IsChecked === true) {
            this._setHomePage.push({
              IsChecked: this.menulist[x].SubMenus[m].IsChecked,
              StartPage: this.menulist[x].SubMenus[m].RouteLink,
            });
          }
        }
      } else {
        for (let m = 0; m < this.menulist[x].SubMenus.length; m++) {
          if ((type === 'add' && this.menulist[x].SubMenus[m].Active === true) || (type === 'edit' && this.menulist[x].SubMenus[m].IsMapped === true)) {

            this._menuarr.push({
              MenuID: this.menulist[x].SubMenus[m].MenuID,
              RouteLink: this.menulist[x].SubMenus[m].RouteLink
            });
          }
          if (this.menulist[x].SubMenus[m].IsChecked === true) {
            this._setHomePage.push({
              IsChecked: this.menulist[x].SubMenus[m].IsChecked,
              StartPage: this.menulist[x].SubMenus[m].RouteLink
            });
          }
        }
      }
    }
    const input = { '_menuarr': this._menuarr, '_setHomePage': this._setHomePage };
    this.menuArr$.next(input);
    const _data = this._menuarr.filter(x => x.RouteLink === 'dashboard' || x.RouteLink === 'workqueue' || x.RouteLink === 'boxupload' || x.RouteLink === 'export');
    const _dataloansearch = this._menuarr.filter(x => x.RouteLink === 'loansearch');
    if ((_data !== undefined || _data !== null) && (_dataloansearch === undefined || _dataloansearch === null || _dataloansearch.length === 0)) {
      this._notificationService.showError('Please Active Loan Search');
      return returVal = true;
    }
    if (roletypedata.RoleName === undefined || roletypedata.RoleName === '' || roletypedata.RoleName === '') {
      this._notificationService.showError('RoleName Required');
      return true;
    } else if (this._menuarr.length === 0) {

      this._notificationService.showError('Select any one Menu ');
      return true;
    } else if (this._setHomePage.length === 0) {
      this._notificationService.showError('Select any one Home Page ');
      return true;
    } else {

      const obj = this.roleAdminData.filter(r => r.RoleName === roletypedata.RoleName);

      if (typeof obj !== 'undefined' && obj.length === 0) {
        return false;
      } else if (obj.length > 0 && obj[0].RoleID === roletypedata.RoleID && obj[0].RoleName === roletypedata.RoleName) {
        return false;
      } else if (obj.length > 0 && obj[0].RoleName === roletypedata.RoleName) {
        this._notificationService.showError('RoleName Already Exists');
        return true;
      }

    }

  }

  GetChangeMenuActive(ChangeMenu: ChangeRoleRequest) {
    this._roleAdminData.GetChangeMenuActive(ChangeMenu).subscribe(res => {
      if (res !== null) {
        const Data = jwtHelper.decodeToken(res.Data)['data']; {

          if (Data !== null && Data.length <= 0 && ChangeMenu.Menus.IsMapped === false) {
            this.isMapped$.next(true);
          } else if (Data !== null && Data.length >= 0 && Data.IsMapped === true) {
            this.isMapped$.next(true);
          } else {
            this.isMapped$.next(false);
          }
        }
      }
    });
  }
}
