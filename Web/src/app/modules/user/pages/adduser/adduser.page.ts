import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { SelectComponent } from '@mts-select2';
import { Subscription } from 'rxjs/internal/Subscription';
import { UserService } from '../../services/user.service';
import { CommonService } from 'src/app/shared/common/common.service';
import { UserDatatableModel } from '../../models/user-datatable.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { UserAddressDetail, UserRoleMapping, CustomAddressDetail } from '@mts-appsession-model';
@Component({
    selector: 'mts-adduser',
    templateUrl: 'adduser.page.html',
    styleUrls: ['adduser.page.css'],

})
export class AddUserComponent implements OnInit, OnDestroy {
    @ViewChild('RoleDropDown') select: SelectComponent;
    @ViewChild('CustomerDropDown') CustomerSelect: SelectComponent;
    _addUserData: UserDatatableModel = new UserDatatableModel();
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    roleItems: any[] = [];
    customerItems: any[] = [];
    custValue: any = 0;
    constructor(private _userService: UserService, private _commonService: CommonService) {
        this.custValue = 0;
        this._addUserData.UserAddressDetail = new UserAddressDetail();
        this._addUserData.UserRoleMapping = new Array<UserRoleMapping>();
        this._addUserData.CustomAddressDetails = new Array<CustomAddressDetail>();

    }
    private subscription: Subscription[] = [];

    ngOnInit(): void {
        this._commonService.GetCustomerList(AppSettings.TenantSchema);
        this._commonService.GetRoleList(AppSettings.TenantSchema);

        this.subscription.push(this._commonService.CustomerItems.subscribe((res: any) => {
            this.customerItems = res;
        }));

        this.subscription.push(this._commonService.RoleItems.subscribe((res: any) => {
            this.roleItems = res;
        }));
    }

    AddUser() {
        if (isTruthy(this.select.active)) {
            const roleDetails: any = [];
            this._addUserData.UserRoleMapping = roleDetails;
            if (this.select.active.length > 0) {
                this.select.active.forEach(element => {
                    roleDetails.push({ RoleID: element.id, RoleName: element.text });
                });
                this._addUserData.UserRoleMapping = roleDetails;
            }
        }
        if (this.custValue.id !== 0) {
            this._addUserData.CustomerID = this.custValue.id;
        } else {
            this._addUserData.CustomerID = 0;
        }

        this._addUserData['Active'] = true;
        this._addUserData['CustomerID'] = this.custValue.id;
        this._addUserData['UserAddressDetail'] = this._addUserData.UserAddressDetail;
        const _addData = { TableSchema: AppSettings.TenantSchema, User: this._addUserData };
        this._userService.AddUserSumbit(_addData);
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
