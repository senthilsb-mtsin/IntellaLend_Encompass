export class DeleteLoanRequestModel {
  TableSchema: string;
  LoanID: number[];
  UserName: string;

  constructor(tableSchema: string, loanIDs: number[], userName: string) {
    this.TableSchema = tableSchema;
    this.LoanID = loanIDs;
    this.UserName = userName;
  }
}
