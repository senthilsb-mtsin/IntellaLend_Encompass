import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { SelectComponent } from '@mts-select2';
import { AppSettings } from '@mts-app-setting';
import { UserService } from '../../services/user.service';
import { Subscription } from 'rxjs';
import { UserDatatableModel } from '../../models/user-datatable.model';
import { CommonService } from 'src/app/shared/common/common.service';

@Component({
    moduleId: 'ViewUserComponent',
    selector: 'mts-viewuser',
    templateUrl: 'viewuser.page.html',
    styleUrls: ['viewuser.page.css'],

})
export class ViewUserComponent implements OnInit, OnDestroy {
    @ViewChild('RoleDropDown') select: SelectComponent;
    @ViewChild('CustomerDropDown') CustomerDropDown: SelectComponent;
    _viewUserRowData: UserDatatableModel = new UserDatatableModel();
    roleItems: any[] = [];
    customerItems: any[] = [];
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    disabled = true;
    CustomerName = '';
    constructor(private apiService: APIService, private _userService: UserService, private _commonService: CommonService) {

    }

    private subscription: Subscription[] = [];
    ngOnInit(): void {
        this.subscription.push(this._commonService.CustomerItems.subscribe((res: any) => {
            this.customerItems = res;
        }));

        this.subscription.push(this._commonService.RoleItems.subscribe((res: any) => {
            this.roleItems = res;
        }));

        this.subscription.push(this._userService.UserData.subscribe((res: UserDatatableModel) => {
            this._viewUserRowData = res;
        }));
        this.subscription.push(this._userService.CustomerName.subscribe((res: any) => {
            this.CustomerName = res;
        }));

        this._commonService.GetCustomerList(AppSettings.TenantSchema);
        this._commonService.GetRoleList(AppSettings.TenantSchema);
        this._userService.getRowData();

    }
    CloseUser() {
        this._userService.CloseUser();
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
