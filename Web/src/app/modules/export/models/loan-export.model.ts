import { TenantLoanRequestModel, TenantRequestModel } from '../../loan-import/models/tenant-request.model';

export class LoanExportModel extends TenantLoanRequestModel {
  JobID: number;
  constructor(TableSchema: string, JobID: number, LoanID: number) {
    super(TableSchema, LoanID);
    this.JobID = JobID;
  }
}
export class LoanJobModel extends TenantRequestModel {
  JobID: number;
  constructor(TableSchema: string, JobID: number) {
    super(TableSchema);
    this.JobID = JobID;
  }
}
