export class GetMenuListRequest {
    TableSchema: string;
    RoleID: string;
    UserID: number;

    constructor(tableSchema: string, roleID: string, userID: number) {
        this.TableSchema = tableSchema;
        this.RoleID = roleID;
        this.UserID = userID;
    }
}
