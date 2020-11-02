export class PurgeStaging {

    BatchCount: any;
    ErrMsg: string;
    Status: any;

  constructor( BatchCount: any, ErrMsg: string, Status: any) {
    this.BatchCount = BatchCount ;
   this.ErrMsg = ErrMsg;
   this.Status = Status;

  }
}
export class RetentionPurge  {

  TableSchema: string;
  UserName: any;
  LoanID: any;
  purgeStaging: PurgeStaging;

  constructor(TableSchema: string, UserName: any, LoanID: any, purgeStaging: PurgeStaging) {
   this.TableSchema = TableSchema ;
   this.UserName = UserName;
   this.LoanID = LoanID;
   this.purgeStaging = purgeStaging;

 }
}
