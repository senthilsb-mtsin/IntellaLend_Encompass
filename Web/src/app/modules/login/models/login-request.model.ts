export class LoginRequest {
  TableSchema: string;
  UserName: string;
  Password: string;
  ADLogin: boolean;

  constructor(tableSchema: string, userName: string, password: string, adLogin: boolean) {
    this.TableSchema = tableSchema;
    this.UserName = userName;
    this.Password = password;
    this.ADLogin = adLogin;
  }
}
