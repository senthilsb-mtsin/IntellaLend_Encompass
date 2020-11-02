export class ChangeRoleRequest {
    TableSchema: string;
    RoleID: any;
    Menus?: any;
    constructor(_tableschema: string, _roleid: any, _menus ?: any) {
        this.TableSchema = _tableschema;
        this.RoleID = _roleid;
        this.Menus = _menus;
    }
}
