export class ChecklistItemRowData {
    CheckListDetailID: number;
    RuleID: number;
    RuleType: number;
    CheckListName: string;
    CheckListDescription: string;
    Category: string;
    FirstName: string;
    LastName: string;
    CreatedOn: string;
    RuleDescription: string;
    ChecklistActive: boolean;
    RuleJson: string;
    RuleJsonObject: RuleJObject;
    SequenceID: number;
    LoanType: string;
    DocVersion: string;
    ChecklistGroupId: number;
    LosIsMatched: number;
    LosMatched: boolean;
    LOSFieldDescription: string;
    LOSValue: string;
    DocumentType: string;
    UserID: number;
    LOSFieldToEvalRule: number;
    LOSValueToEvalRule: string;
    CustomerName: string;
    ReviewTypeName: string;

    constructor() {
        this.CheckListDetailID = 0;
        this.RuleID = 0;
        this.RuleType = 0;
        this.CheckListName = '';
        this.CheckListDescription = '';
        this.Category = '';
        this.FirstName = '';
        this.LastName = '';
        this.CreatedOn = '';
        this.RuleDescription = '';
        this.ChecklistActive = true;
        this.RuleJson = '';
        this.RuleJsonObject = new RuleJObject();
        this.SequenceID = 0;
        this.LoanType = '';
        this.DocVersion = '';
        this.ChecklistGroupId = 0;
        this.LosIsMatched = 0;
        this.LosMatched = false;
        this.LOSFieldDescription = '';
        this.LOSValue = '';
        this.DocumentType = '';
        this.UserID = 0;
        this.LOSFieldToEvalRule = 0;
        this.LOSValueToEvalRule = '';
        this.CustomerName = '';
        this.ReviewTypeName = '';
    }
}

export class RuleJObject {
    compareAllRule: any[] = [];
    conditionalRule: any[] = [];
    datatableRule: any[] = [];
    datediffRule: any[] = [];
    docCheckAll: any[] = [];
    docIsExist: any[] = [];
    generalRule: any[] = [];
    groupby: any[] = [];
    inRule: any[] = [];
    isEmptyRule: any[] = [];
    isNotEmptyRule: any[] = [];
    losRule: any[] = [];
    mainOperator = '';
    manualGroup: { CheckBoxChoices: any[], QuestionsTypes: any[], RadioChoices: any[] }[] = [
        { CheckBoxChoices: [], QuestionsTypes: [], RadioChoices: [] }
    ];
}
