
export class Checklist {
    Category: string;
    CheckListName: string;
    Formula: string;
    Expression: string;
    Result: string;
    RoleType: string;
    Message: string;
    SequenceID: number;
    ErrorMessage: string;
    RuleType: string;
}

export class ChecklistCategory {
    CheckListID: number;
    Name: string;
    Description: any;
    Active: boolean;
    UserID: number;
    Rule_Type: number;
    Category: string;
}

export class LoanAudit {
    AuditID: string;
    AuditDescription: string;
    LoanID: string;
    UploadedUserID: string;
    ReviewTypeID: string;
    LoanTypeID: string;
    LoggedUserID: string;
    Status: string;
    SubStatus: string;
    LoanCreatedOn: string;
    LoanLastModifiedOn: string;
    FileName: string;
    CustomerID: string;
    AuditDateTime: any;
}

export class ILoanNote {
    UserId: any;
    UserName: any;
    Timestamp: any;
    Note: any;
    Custom1?: any;
    Custom2?: any;
}
export class Loaninfo {
    imageState = true;
    busyState: boolean;
    checkListState = false;
    cords: any;
    pageNo: any;
    pageNumberArray: any = [];
    lastPageNumber = 0;
    SingleimageState = true;
    onLoading = false;
}
