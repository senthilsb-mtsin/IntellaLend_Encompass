export class SyncCustomerRequest {
  TableSchema: string;
  LoanTypeID: number;
  UserID: number;
  SyncLevel: number;

  constructor(schema: string, loantypeID: number, userId: number, synclevel: number) {
    this.TableSchema = schema;
    this.LoanTypeID = loantypeID;
    this.UserID = userId;
    this.SyncLevel = synclevel;
  }
}
