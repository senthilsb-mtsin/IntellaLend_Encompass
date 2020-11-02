export class AssignStackingOrderRequestModel {
    LoanTypeID: number;
    StackingOrderID: number;
    StackingOrderName: string;

    constructor(_loanType: number, _stackingOrderID: number, _stackingOrderName: string) {
        this.LoanTypeID = _loanType;
        this.StackingOrderID = _stackingOrderID;
        this.StackingOrderName = _stackingOrderName;
    }

}
