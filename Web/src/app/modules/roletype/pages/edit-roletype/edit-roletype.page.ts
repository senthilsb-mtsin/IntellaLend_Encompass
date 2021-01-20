import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { RoleTypeService } from '../../service/roletype.service';
import { Subscription } from 'rxjs';
import { AddRoleTypeRequestModel } from '../../models/roletype-request.model';
import { ChangeRoleRequest } from '../../models/changerole.model';
import { RoleDetailsRequest } from '../../models/roletypedetails.model';
import { Roletypemodel } from '../../models/roletype-datatable.model';
import { environment } from 'src/environments/environment';
import { ADGroupMasterModel, CheckADGroupAssignedForRoleRequestModel } from '../../models/adgroupmaster.model';
import { NotificationService } from '@mts-notification';
@Component({
  selector: 'mts-edit-roletype',
  templateUrl: './edit-roletype.page.html',
  styleUrls: ['./edit-roletype.page.css'],
})
export class EditRoleTypeComponent implements OnInit, OnDestroy {
  promise: Subscription;
  IsMappedMenuView = false;
  selectedRow: any;
  menulist: any;
  _menuarr: any = {};
  _setHomePage: any = [];
  _viewsubmenu = false;
  roletypedata: Roletypemodel;
  AD_login: boolean = environment.ADAuthentication;
  ADGroupMasterList: ADGroupMasterModel[] = [];

  constructor(private _roleTypeService: RoleTypeService
    , private _notificationService: NotificationService) { }

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
      this._roleTypeService.menuArr$.subscribe(
        (res: any) => {
          this._menuarr = res._menuarr;
          this._setHomePage = res._setHomePage;
        })
    );
    this.subscription.push(
      this._roleTypeService.ischeckuser$.subscribe(
        (res: any) => {
          this.roletypedata.Active = true;
        })
    );
    this.subscription.push(this._roleTypeService.ADGroupAssignedForRole$.subscribe(res => {
      if (res) {
        this._notificationService.showError('ADGroup already assigned');
        this.roletypedata.ADGroupID = 0;
      }
    }));

    this.EditRoleDetails();
  }

  EditRoleDetails() {
    const updaterole: RoleDetailsRequest = {
      TableSchema: AppSettings.TenantSchema,
      RoleID: this.roletypedata.RoleID,
      IsMappedMenuView: this.IsMappedMenuView
    };
    this._roleTypeService.EditRoleDetails(updaterole);
  }

  UpdateRoleSubmit() {
    if (!this._roleTypeService.validate(this.roletypedata)) {
      this.roletypedata.StartPage = this._setHomePage[0].StartPage;
      const inputData = new AddRoleTypeRequestModel(AppSettings.TenantSchema, this.roletypedata, this._menuarr);
      this._roleTypeService.UpdateRoleSubmit(inputData);
    }
  }
  rolechangefunction() {
    if (!this.roletypedata.Active) {
      const rolechange = new ChangeRoleRequest(
      AppSettings.TenantSchema, this.roletypedata.RoleID);
      this._roleTypeService.CheckUserRoleDetails(rolechange);
  }
}

  CloseRole() {
    this._roleTypeService.CloseRole();
  }
  ADGroupChange() {
    const req: CheckADGroupAssignedForRoleRequestModel = new CheckADGroupAssignedForRoleRequestModel(AppSettings.TenantSchema,
        this.roletypedata.ADGroupID,
        this.roletypedata.RoleID
      );
      if (this.roletypedata.ADGroupID !== 0) {
        this._roleTypeService.CheckADGroupAssignedForRole(req);
      }
  }

  ngOnDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }
}
