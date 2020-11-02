export class CloneChecklistRequest {
    LoanTypeID: number;
    CheckListID: number;
    CheckListName: string;

    constructor(_checkListName: string) {
        this.LoanTypeID = 0;
        this.CheckListID = 0;
        this.CheckListName = _checkListName;
    }
}
