export class SaveChecklistItem {
    CheckListDetailID: number;
    CheckListID: number;
    Name: string;
    Description: string;
    Active: boolean;
    UserID: number;
    RuleType: number;
    Rule_Type: number;
    Category: string;
    LOSFieldToEvalRule: number;
    LOSValueToEvalRule: string;
    SystemID: number;
    SequenceID: number;
    LosIsMatched: number;
}

export class SaveRuleMasters {
    RuleID: number;
    CheckListDetailID: number;
    RuleDescription: string;
    Active: boolean;
    RuleJson: string;
    DocumentType: string;
    ActiveDocumentType: string;
    DocVersion: string;
}
