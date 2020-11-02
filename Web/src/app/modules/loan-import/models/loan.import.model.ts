import { TenantLoanRequestModel, TenantRequestModel } from './tenant-request.model';

export class DeleteLoanModel extends TenantLoanRequestModel {
  UserName: any;
  constructor(TableSchema: string, LoanID: any, UserName: any) {
    super(TableSchema, LoanID);
    this.UserName = UserName;
  }
}
export class OverideLoanUserModel extends TenantLoanRequestModel {
  PickUpUserID: any;
  constructor(TableSchema: string, LoanID: any, PickUpUserID: any) {
    super(TableSchema, LoanID);
    this.PickUpUserID = PickUpUserID;
  }
}
export class CheckCurrentUserModel extends TenantLoanRequestModel {
  CurrentUserID: any;
  constructor(TableSchema: string, LoanID: any, CurrentUserID: any) {
    super(TableSchema, LoanID);
    this.CurrentUserID = CurrentUserID;
  }
}
export class UpdateLoanMonitorModel extends TenantLoanRequestModel {
  UserName: string;
  LoanTypeID: number;
  constructor(TableSchema: string, LoanID: any, LoanTypeID: number, UserName: string) {
    super(TableSchema, LoanID);
    this.UserName = UserName;
    this.LoanTypeID = LoanTypeID;
  }
}

export class CustomerReviewLoanTypeModel extends TenantRequestModel {
  CustomerID: number;
  ReviewTypeID: number;
  constructor(TableSchema: string, CustomerID: number, ReviewTypeID: number) {
    super(TableSchema);
    this.CustomerID = CustomerID;
    this.ReviewTypeID = ReviewTypeID;
  }

}
