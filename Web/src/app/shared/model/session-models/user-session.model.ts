import {
  CustomAddressDetail,
  UserAddressDetail,
  UserRoleMapping,
  CustomerMaster,
  UserSecurityQuestion,
} from '@mts-appsession-model';

export class User {
  UserID: any;
  Active: any;
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
  CustomAddressDetails: CustomAddressDetail[];
  UserAddressDetail: UserAddressDetail;
  UserRoleMapping: UserRoleMapping[];
  customerDetail: CustomerMaster;
  userSecurityQuestion: UserSecurityQuestion;
}
