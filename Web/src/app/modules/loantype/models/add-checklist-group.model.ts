export class AddChecklistGroupModel {
    checkListMaster: ChecklistMasterModel;

    constructor(_checklistMaster: ChecklistMasterModel) {
        this.checkListMaster = _checklistMaster;
    }
}

export class ChecklistMasterModel {
    CheckListName: string;
    Active: boolean;
    Sync: boolean;
    constructor(_checkListName: string, _active: boolean, _sync: boolean) {
        this.CheckListName = _checkListName;
        this.Active = _active;
        this.Sync = _sync;
    }
}
