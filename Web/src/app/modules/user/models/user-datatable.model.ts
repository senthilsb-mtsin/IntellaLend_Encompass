import { CustomAddressDetail, UserRoleMapping, CustomerMaster, UserAddressDetail } from '@mts-appsession-model';

export class UserDatatableModel {
    UserID: any;
    Active: any;
    Email: any;
    CreatedOn: any;
    FirstName: any;
    LastModified: any;
    LastName: any;
    Locked: any;
    MiddleName: any;
    Password: any;
    Status: any;
    UserName: any;
    CustomerID: any;
    CustomerName: any;
    UserCustomerID: any;
    activeRoles: any;
    CustomAddressDetails: CustomAddressDetail[];
    UserAddressDetail: UserAddressDetail;
    UserRoleMapping: UserRoleMapping[];
    customerDetail: CustomerMaster; }
