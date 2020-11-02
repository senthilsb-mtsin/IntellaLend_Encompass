import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { RoleTypeService } from '../../service/roletype.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { RoleDetailsRequest } from '../../models/roletypedetails.model';
import { Roletypemodel } from '../../models/roletype-datatable.model';
import { environment } from 'src/environments/environment';
import { ADGroupMasterModel } from '../../models/adgroupmaster.model';
@Component({
  selector: 'mts-view-roletype',
  templateUrl: 'view-roletype.page.html',
  styleUrls: ['view-roletype.page.css']
})
export class ViewRoleTypeComponent implements OnInit, OnDestroy {
  menulist: any;
  roletypedata: any = {};
  isSubMenu = 'none';
  MenuGroup: any = ['MenuGroupID', 'MenuGroupTitle', 'subMenus'];
  _viewsubmenu = false;
  IsMappedMenuView = true;
  AD_login: boolean = environment.ADAuthentication;
  ADGroupMasterList: ADGroupMasterModel[] = [];

  constructor(private _roleTypeService: RoleTypeService
  ) {

  }
  private subscription: Subscription[] = [];

  ngOnInit(): void {

    this._roleTypeService.getADGroupMasterList();

    this.subscription.push(this._roleTypeService.ADGroupMasterList$.subscribe((res: ADGroupMasterModel[]) => {
      this.ADGroupMasterList = [...res];
    }));

    this.subscription.push(
      this._roleTypeService.RoleTypeRowData.subscribe(
        (res: Roletypemodel) => {
          this.roletypedata = res;
        })
    );
    this.subscription.push(
      this._roleTypeService.menulist$.subscribe(
        (res: any) => {
          this.menulist = res;
        }
      )
    );
    this.EditRoleDetails();
  }

  ShowSubMenus() {
    if (this.isSubMenu === 'none') {
      this.isSubMenu = 'block';
      this._viewsubmenu = true;
    } else {
      this.isSubMenu = 'none';
      this._viewsubmenu = false;
    }

  }
  EditRoleDetails() {
    const updaterole: RoleDetailsRequest = {
      TableSchema: AppSettings.TenantSchema,
      RoleID: this.roletypedata.RoleID,
      IsMappedMenuView: this.IsMappedMenuView
    };
    this._roleTypeService.EditRoleDetails(updaterole);
  }

  CloseRole() {
    this._roleTypeService.CloseRole();
  }

  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
