export class GetMenuListRequest {
    TableSchema: string;
    RoleID: string;
    UserID: number;
    ADLogin: boolean;

    constructor(tableSchema: string, roleID: string, userID: number, adLogin: boolean) {
        this.TableSchema = tableSchema;
        this.RoleID = roleID;
        this.UserID = userID;
        this.ADLogin = adLogin;
    }
}
