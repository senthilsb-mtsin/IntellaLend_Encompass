export class LoanExportModel {
    JobID: any;
    JobName: any;
    ExportedBy: any;
    CustomerID: any;
    CoverLetter: boolean;
    TableOfContent: boolean;
    PasswordProtected: boolean;
    Password: any;
    ConfirmPassword: any;
    CoverLetterContent: any;
    LoanDetail: {
        LoanID: any;
        LoanNumber: any;
        IsSelected: any;
        Documents: any;
        CurrentLoan: any;
        DocumentDetails: any[]
    }[] = [];
    constructor(JobID: any,
        JobName: any,
        ExportedBy: any,
        CustomerID: any,
        CoverLetter: boolean,
        TableOfContent: boolean,
        PasswordProtected: boolean,
        Password: any,
        ConfirmPassword: any,
        CoverLetterContent: any,
        LoanDetail: {
            LoanID: any,
            LoanNumber: any,
            IsSelected: any,
            Documents: any,
            CurrentLoan: any,
            DocumentDetails: any[]
        }[]) {
        this.JobID = JobID;
        this.JobName = JobName;
        this.ExportedBy = ExportedBy;
        this.CustomerID = CustomerID;
        this.CoverLetter = CoverLetter;
        this.TableOfContent = TableOfContent;
        this.PasswordProtected = PasswordProtected;
        this.Password = Password;
        this.ConfirmPassword = ConfirmPassword;
        this.CoverLetterContent = CoverLetterContent;
        this.LoanDetail = LoanDetail;
    }
}
