
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { RoleTypeService } from '../../service/roletype.service';
import { Subscription } from 'rxjs';
import { AddRoleTypeRequestModel } from '../../models/roletype-request.model';
import { Roletypemodel } from '../../models/roletype-datatable.model';
import { environment } from 'src/environments/environment';
import { ADGroupMasterModel, CheckADGroupAssignedForRoleRequestModel } from '../../models/adgroupmaster.model';
import { NotificationService } from '@mts-notification';
@Component({
  selector: 'mts-add-roletype',
  templateUrl: './add-roletype.page.html',
  styleUrls: ['./add-roletype.page.css'],
})
export class AddRoleTypeComponent implements OnInit, OnDestroy {
  id: any;
  RoleName: '';
  _menuarr: any = {};
  _setHomePage: any = [];
  startpage = 'HomePage';
  promise: Subscription;
  roletypedata: Roletypemodel;
  AD_login: boolean = environment.ADAuthentication;
  ADGroupMasterList: ADGroupMasterModel[] = [];

  constructor(private _roleTypeService: RoleTypeService,
    private _notificationService: NotificationService
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit() {
    this.roletypedata = new Roletypemodel();

    this._roleTypeService.GetMenuList();
    this.subscription.push(
      this._roleTypeService.menuArr$.subscribe(
        (res: any) => {
          this._menuarr = res._menuarr;
          this._setHomePage = res._setHomePage;
        })
    );

    this._roleTypeService.SetTableRowData(this.roletypedata);
    this._roleTypeService.getADGroupMasterList();

    this.subscription.push(this._roleTypeService.ADGroupMasterList$.subscribe((res: ADGroupMasterModel[]) => {
      this.ADGroupMasterList = [...res];
    }));
    this.subscription.push(this._roleTypeService.ADGroupAssignedForRole$.subscribe(res => {
      if (res) {
        this._notificationService.showError('ADGroup already assigned');
        this.roletypedata.ADGroupID = 0;
      }
    }));
  }

  AddRoleSubmit() {
    if (!this._roleTypeService.validate(this.roletypedata)) {
      this.roletypedata.StartPage = this._setHomePage[0].StartPage;
      const req = new AddRoleTypeRequestModel(AppSettings.TenantSchema, this.roletypedata, this._menuarr);
      this._roleTypeService.GetAddRoleSubmit(req);
    }
  }
  ADGroupChange() {
    const req: CheckADGroupAssignedForRoleRequestModel = new CheckADGroupAssignedForRoleRequestModel(AppSettings.TenantSchema,
        this.roletypedata.ADGroupID,
        0
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
  CloseRole() {
    this._roleTypeService.CloseRole();
  }
}
