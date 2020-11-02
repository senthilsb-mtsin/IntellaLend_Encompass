
export class ResetExpiredPassword  {
  TableSchema: string;
  UserName: any;
  constructor(tableSchema: string, UserName: any) {
   this.TableSchema = tableSchema;
   this.UserName = UserName;

  }
}
