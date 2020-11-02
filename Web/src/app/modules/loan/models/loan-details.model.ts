import { LoanHeaders, StipulationDetails } from './loan-header.model';
import { Checklist, ChecklistCategory, LoanAudit } from './loan-details-sub.model';
import { LoanSearchTableModel } from '../../loansearch/models/loan-search-table.model';

export class Loan {
    LoanID: number;
    LoanNumber: string;
    LoanType: string;
    LoanStatus: string;
    AssignedOwner: string;
    LoanTypeID: number;
    Status: number;
    loanDocuments: Doc[];
    loanGroupDocuments: Doc[];
    distDocuments: Doc[];
    missingDocuments: LoanDocument[];
    failedChecklist: Checklist[];
    allChecklist: Checklist[];
    checkListCategory: ChecklistCategory[];
    loanAudit: LoanAudit[];
    totalCheckListCount: number;
    showDownload: boolean;
    loanStackingOrder: StackingOrder;
    doc: Doc;
    success: boolean;
    loanQuestioner: LoanQuestioner[];
    reverificationCount: number;
    LoanHeaderInfo: LoanHeaders;
    LoanStipulationDetails: StipulationDetails;
    LoanHeaderPopOutInfo: LoanSearchTableModel;
}

export class LoanQuestioner {
    RuleID: any;
    CheckListDetailID: any;
    CheckListName: any;
    Question: any;
    OptionJson: any;
    AnswerJson: any;
    Category: any;
    SequenceID: any;
}

export class Doc {
    DocID: any;
    DocName: any;
    DocumentLevelID: any;
    DocumentLevel: any;
    DocumentLevelIcon: any;
    DocumentLevelIconColor: any;
    SequenceID: any;
    VersionNumber: number;
    FieldOrderBy: string;
    OrderByFieldValue: string;
    FieldOrderVersion: number;
    DocFieldName: string;
    IsDocName: boolean;
    DocValue: string;
    DocVersion: string;
    StackingOrderGroupDetails: any;
    StackingGroupId: number;
    StackingOrderGroupName: string;
    StackingOrderFieldName: string;
    StackingOrderFieldValue: string;
    IsGroup: boolean;
    ToolTipValue: any;
    DocNameVersion: any;
    ToolTipGroup: any;
    Obsolete: boolean;
    IDCStatus: string;
    IDCUrl: string;
    FieldVersionNumber: any;
    orderbyvalue: any;
}

export class StackingOrder {
    StackingOrderID: string;
    StackingOrderName: string;
}

export class LoanDocument {
    DocID: number;
    DocName: string;
    DocStatusDescription: any;
    Obsolete: any;
    DocMissingStatusID: any;
}
