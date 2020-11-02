import { TenantCustomerRequestModel } from './tenant-request.model';

export class LoanSearchModel extends TenantCustomerRequestModel {
  FromDateStr: any;
  ToDateStr: any;
  UserID: number;
  UploadStatus: any;
  FromDate: any;
  ToDate: any;
  constructor(FromDateStr: any, ToDateStr: any, TableSchema: string, UserID: number, UploadStatus: any, CustomerID: number, FromDate: any, ToDate: any) {
    super(TableSchema, CustomerID);
    this.ToDateStr = ToDateStr;
    this.FromDateStr = FromDateStr;
    this.UserID = UserID;
    this.UploadStatus = UploadStatus;
    this.FromDate = FromDate;
    this.ToDate = ToDate;
  }
}
