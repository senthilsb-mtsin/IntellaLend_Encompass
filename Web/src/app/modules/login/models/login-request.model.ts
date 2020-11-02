export class LoginRequest {
  TableSchema: string;
  UserName: string;
  Password: string;

  constructor(tableSchema: string, userName: string, password: string) {
    this.TableSchema = tableSchema;
    this.UserName = userName;
    this.Password = password;
  }
}
