
import { Injectable, ViewChild } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { UserDataAccess } from '../user.data';
import { AppSettings } from '@mts-app-setting';
import { ResetExpiredPassword } from '../models/reset-expired-password.model';
import { UserDatatableModel } from '../models/user-datatable.model';
import { NotificationService } from '@mts-notification';
import { GetSchemaRequest } from '../models/get-schema-request..model';
import { SelectComponent } from '@mts-select2';
import { Location } from '@angular/common';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CharcCheckPipe, EmailCheckPipe, TrimspacePipe, ValidateZipcodePipe } from '@mts-pipe';
import { Router } from '@angular/router';

const jwtHelper = new JwtHelperService();

@Injectable()

export class UserService {
    @ViewChild('RoleDropDown') select: SelectComponent;
    @ViewChild('CustomerDropDown') CustomerSelect: SelectComponent;
    setUserTableData = new Subject<any>();
    users = new Subject<any>();
    UserAddressDetails = new Subject<any>();
    UserData = new Subject<any>();
    CustomerName = new Subject<any>();

    constructor(private _userData: UserDataAccess,
        private location: Location,
        private allowChar: CharcCheckPipe,
        private _notifyService: NotificationService,
        private emailPipe: EmailCheckPipe,
        private _trimSpace: TrimspacePipe,
        private _zipcodevalidpipe: ValidateZipcodePipe,
        private _route: Router

    ) {
    }
    private userRoles: any = [];
    private _userRowData: UserDatatableModel = new UserDatatableModel();
    private userData: any = [];
    private selectedAuthLevel: any;
    private returnRole = false;

    SetRowData(inputData: UserDatatableModel) {
        this._userRowData = inputData;
    }

    ResetExpiredPassword(_resBody: ResetExpiredPassword) {
        return this._userData.getResetPassword(_resBody).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result === 'True') {
                this._notifyService.showSuccess
                    ('Password Reseted Successfully');
            } else {
                this._notifyService.showError('Password Not Reseted');
            }
        });
    }

    GetUserList(_resBody: GetSchemaRequest) {
        return this._userData.getUserData(_resBody).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this.userData = data;
            this.setUserTableData.next(data);
        });
    }

    CloseUser() {
        this.location.back();
        this._userRowData.activeRoles = [];
        this.userRoles = [];
    }

    EditUserSubmit(_resBody: any) {
        if (!this.validate(_resBody)) {
            return this._userData.EditUserData(_resBody)
                .subscribe
                (res => {
                    const Result = jwtHelper.decodeToken(res.Data)['data'];
                    if (isTruthy(Result)) {
                        this._notifyService.showSuccess('User Updated Successfully');
                        this.location.back();
                    } else {
                        this._notifyService.showError('User Update Failed');
                        this.location.back();
                    }
                });
        }
    }

    AddUserSumbit(_resBody: any) {
        if (!this.validate(_resBody)) {
            return this._userData
                .AddUserData(_resBody)
                .subscribe(res => {
                    const data = jwtHelper.decodeToken(res.Data)['data'];
                    if (data === true) {
                        this.CloseUser();
                        this._notifyService.showSuccess('User Added Successfully');
                        this._route.navigate(['view/user/']);
                    } else {
                        this._notifyService.showError('Username Already Exist');
                    }
                });
        }
    }

    validate(Request: any): boolean {
        let returVal = false;
        if (isTruthy(Request.User.FirstName) || isTruthy(Request.User.LastName)) {
            Request.User.FirstName = this._trimSpace.transform(Request.User.FirstName) === '' ? '' : Request.User.FirstName;
            Request.User.LastName = this._trimSpace.transform(Request.User.LastName) === '' ? '' : Request.User.LastName;
        }

        if (!isTruthy(Request.User.UserName)) {
            this._notifyService.showError('Email ID Required ');
            returVal = true;
        }
        if (isTruthy(Request.User.UserName) && !this.emailPipe.transform(Request.User.UserName)) {
            this._notifyService.showError('Enter Valid Email ID ');
            returVal = true;
        }
        if (!isTruthy(Request.User.FirstName)) {
            this._notifyService.showError('First Name Required');
            returVal = true;
        }
        if (!isTruthy(Request.User.LastName)) {
            this._notifyService.showError('Last Name Required');
            returVal = true;
        }
        if ((isTruthy(Request.User.FirstName) && !this.allowChar.transform(Request.User.FirstName))) {
            this._notifyService.showError('First Name must be Character');
            returVal = true;
        }
        if ((isTruthy(Request.User.LastName) && !this.allowChar.transform(Request.User.LastName))) {
            this._notifyService.showError('Last Name must be Character');
            returVal = true;
        }
        if (Request.User.UserRoleMapping.length === 0) {
            this._notifyService.showError('Select Role');
            returVal = true;
        }
        if ((Request.User.City !== undefined && Request.User.City !== '') && !this.allowChar.transform(Request.User.City)) {
            this._notifyService.showError('City must be Character');
            returVal = true;
        }

        if ((Request.User.Country !== undefined && Request.User.Country !== '') && !this.allowChar.transform((Request.User.Country))) {
            this._notifyService.showError('Country must be Character');
            returVal = true;
        }
        if ((Request.User.ZipCode !== undefined && Request.User.ZipCode !== '') && !this._zipcodevalidpipe.transform(Request.User.ZipCode)) {
            this._notifyService.showError('Enter Valid ZipCode');
            returVal = true;
        }
        return returVal;

    }

    getRowData(): void {
        if (isTruthy(this._userRowData)) {
            this._userRowData.UserCustomerID = [];
            this.users = this._userRowData.UserName;
            this.userRoles = [];
            this._userRowData.UserAddressDetail = this._userRowData.UserAddressDetail;
            if (this._userRowData.CustomerID !== 0) {
                this._userRowData.UserCustomerID.push({ id: this._userRowData.customerDetail.CustomerID, text: this._userRowData.customerDetail.CustomerName });
                this.CustomerName.next(this._userRowData.UserCustomerID[0].text);
            } else {
                this._userRowData.UserCustomerID.push({ id: 0, text: '--Select ' + AppSettings.AuthorityLabelSingular + '--' });
                this.CustomerName.next(this._userRowData.UserCustomerID[0].text);
            }
            this._userRowData.UserRoleMapping.forEach(element => {
                this.userRoles.push({ id: element.RoleID, text: element.RoleName });
            });
            this._userRowData.activeRoles = this.userRoles;
            this.UserData.next(this._userRowData);
        } else {
            this.CloseUser();
        }
    }
}
