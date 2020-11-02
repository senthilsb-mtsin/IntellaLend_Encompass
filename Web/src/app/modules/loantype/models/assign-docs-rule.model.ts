export class AssignDocumentsRuleRowData {
    CustomerID: number;
    LoanTypeID: number;
    DocumentTypeID: number;
    Condition: any;
    ConditionObject: ConditionObject;
    ModifiedOn: string;

    constructor() {
        this.CustomerID = 0;
        this.LoanTypeID = 0;
        this.DocumentTypeID = 0;
        this.Condition = '';
        this.ConditionObject = new ConditionObject();
        this.ModifiedOn = '';

    }

}
export class ConditionObject {

    formula: '';
    schema: any[] = [];

}
