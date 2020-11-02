export class ChecklistRowItem {
    Category: any = '';
    CheckListName: any = '';
    Result: any;
    Type: any = '';
    Message: any = '';
    Error: any = '';
    Checklist: CheckListResult = new CheckListResult();
    Formula: any = [];
    AssociatedDoc: any = [];
    SequenceID: any = 0;
    qitem: QItem = new QItem();
}

export class CheckListResult {
    CheckListName: any = '';
    Formula: any = '';
    Expression: any = '';
    Result: any;
    ErrorMessage: any = '';
    Message: any = '';
    SequenceID: any = '';
    Category: any = '';
}

export class QItem {
    Category: any = '';
    RuleID = 0;
    CheckListDetailID = 0;
    CheckListName: any = '';
    Question: any = '';
    inputList: any = [];
    answer: [];
    NotesEnabled = false;
    QuestionNotes: any = '';
    SequenceID = 0;
}
