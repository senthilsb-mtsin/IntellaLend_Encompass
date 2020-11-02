import { TenantCustomerRequestModel, TenantLoanRequestModel } from './tenant-request.model';

export class RetryEncompassDownloadModel extends TenantLoanRequestModel {
  EDownloadID: number;
  constructor(TableSchema: string, LoanID: number, EDownloadID: number) {
    super(TableSchema, LoanID);
    this.EDownloadID = EDownloadID;
  }
}
export class GetEphesofturlModel extends TenantCustomerRequestModel {
  EphesoftBatchInstanceID: number;
  EphesoftURL: any;
  constructor(TableSchema: string, EphesoftBatchInstanceID: any, EphesoftURL: any, CustomerID: number) {
    super(TableSchema, CustomerID);
    this.EphesoftBatchInstanceID = EphesoftBatchInstanceID;
    this.EphesoftURL = EphesoftURL;
  }
}
