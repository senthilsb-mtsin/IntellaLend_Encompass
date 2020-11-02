export class GetBoxValueModel implements ClientData {
  TableSchema: string;
  clientid: any;
  clientsecretid: any;
  boxuserid: any;
  isUpdate: any;
  constructor(
    TableSchema: string,
    clientid: any,
    clientsecretid: any,
    boxuserid: any,
    isUpdate: any
  ) {
    this.TableSchema = TableSchema;
    this.clientid = clientid;
    this.clientsecretid = clientsecretid;
    this.boxuserid = boxuserid;
    this.isUpdate = isUpdate;
  }
}
export class CheckBoxTokenModel {
  TableSchema: string;
  UserID: any;

  constructor(TableSchema: string, UserID: any) {
    this.TableSchema = TableSchema;
    this.UserID = UserID;
  }
}
export class GetBoxTokenModel extends CheckBoxTokenModel {
  AuthCode: any;
  constructor(TableSchema: string, UserID: any, AuthCode: any) {
    super(TableSchema, UserID);
    this.AuthCode = AuthCode;
  }
}
export class BoxSettingData implements ClientData {
  BoxAuthURL: any;
  BoxAuthCode: any;
  userDetails: any;
  boxUserID: any;
  boxConnectStatus: any;
  boxConnectLabel: any;
  clientid: any;
  clientsecretid: any;
  boxURL: any;
}
export interface ClientData {
  clientid: any;
  clientsecretid: any;
}
