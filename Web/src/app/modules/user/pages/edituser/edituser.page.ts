import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { UserService } from '../../services/user.service';
import { AppSettings } from '@mts-app-setting';
import { SelectComponent } from 'src/app/shared/custom-plugins/select/select';
import { Subscription } from 'rxjs/internal/Subscription';
import { CommonService } from 'src/app/shared/common/common.service';
import { UserDatatableModel } from '../../models/user-datatable.model';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
@Component({
    selector: 'mts-edituser',
    templateUrl: 'edituser.page.html',
    styleUrls: ['edituser.page.css'],

})
export class EditUserComponent implements OnInit, OnDestroy {
    @ViewChild('RoleDropDown') select: SelectComponent;
    @ViewChild('CustomerDropDown') CustomerSelect: SelectComponent;
    _editUserData: UserDatatableModel = new UserDatatableModel();
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    roleItems: any[] = [];
    customerItems: any[] = [];
    constructor(
        private _userService: UserService,
        private _commonService: CommonService
    ) {
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

        this.subscription.push(this._userService.UserData.subscribe((res: UserDatatableModel) => {
            this._editUserData = res;
        }));
        this._userService.getRowData();
        this._editUserData.UserCustomerID = 0;

    }

    CloseUser() {
        this._userService.CloseUser();
    }
    EditUser() {
        this._editUserData['UserAddressDetail'] = this._editUserData.UserAddressDetail;
        const roleDetails: any = [];
        if (isTruthy(this.select.active)) {
            if (this.select.active.length > 0) {
                this.select.active.forEach(element => {
                    roleDetails.push({ RoleID: element.id, RoleName: element.text, UserID: this._editUserData.UserID });
                });
                this._editUserData['UserRoleMapping'] = roleDetails;
            } else {
                this._editUserData['UserRoleMapping'] = roleDetails;
            }
        }
        if (this._editUserData.CustomerID !== 0) {
            this._editUserData.CustomerID = this._editUserData.CustomerID;
        } else {
            this._editUserData.CustomerID = 0;
        }

        this._editUserData['CreatedOn'] = convertDateTime(this._editUserData['CreatedOn']);
        this._editUserData['LastModified'] = convertDateTime(this._editUserData['LastModified']);
        const editUserData = { TableSchema: AppSettings.TenantSchema, User: this._editUserData };
        this._userService.EditUserSubmit(editUserData);
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
