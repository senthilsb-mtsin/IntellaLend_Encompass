import { TenantLoanRequestModel, TenantRequestModel } from '../../loan-import/models/tenant-request.model';

export class EncompassExportModel extends TenantLoanRequestModel {
  ID: number;
  constructor(TableSchema: string, ID: number, LoanID: number) {
    super(TableSchema, LoanID);
    this.ID = ID;
  }
}
export class EncompassSearchExportModel extends TenantRequestModel {
  ID: number;
  Customer: number;
  Status: any;
  EncompassExportedDate: any;
  constructor(TableSchema: string, EncompassExportedDate: any, Status: any, Customer: number) {
    super(TableSchema);
    this.EncompassExportedDate = EncompassExportedDate;
    this.Customer = Customer;
    this.Status = Status;
  }
}
export class SearchExportModel extends TenantRequestModel {
  ID: number;
  Customer: number;
  Status: any;
  ExportedDate: any;
  constructor(TableSchema: string, ExportedDate: any, Status: any, Customer: number) {
    super(TableSchema);
    this.ExportedDate = ExportedDate;
    this.Customer = Customer;
    this.Status = Status;
  }
}
