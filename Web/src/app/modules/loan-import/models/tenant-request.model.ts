export class TenantRequestModel {
  TableSchema: string;
  constructor(TableSchema: string) {
    this.TableSchema = TableSchema;
  }
}
export class TenantLoanRequestModel extends TenantRequestModel {
  LoanID: any;
  constructor(TableSchema: string, LoanID: any) {
    super(TableSchema);
    this.LoanID = LoanID;
  }
}

export class TenantCustomerRequestModel extends TenantRequestModel {
  CustomerID: number;
  constructor(TableSchema: string, CustomerID: number) {
    super(TableSchema);
    this.CustomerID = CustomerID;
  }
}
