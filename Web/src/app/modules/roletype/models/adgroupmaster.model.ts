export class ADGroupMasterModel {
    constructor(public ADGroupID: number,
        public ADGroupName: string) {
    }
}

export class CheckADGroupAssignedForRoleRequestModel {
    constructor(public TableSchema: string,
        public ADGroupID: number,
        public RoleID: number) { }
}
