export class RoleDetailsRequest {
    TableSchema: string;
    RoleID: any;
    IsMappedMenuView = false;
    constructor(_tableschema: string, _roleid: any, _IsMappedMenuView: boolean) {

        this.TableSchema = _tableschema;
        this.RoleID = _roleid;
        this.IsMappedMenuView = _IsMappedMenuView;
    }
}
