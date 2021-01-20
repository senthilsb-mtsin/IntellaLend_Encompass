export class CustLoanTypeMappingModel {
    CustomerID: number;
    LoanTypeID: number;
    LoanTypeName: string;
    DocumentTypeSync: boolean;
    LoanTypeSync: boolean;
    ID: number;

    constructor(_Id: number, _custID: number, _loanTypeID: number, _loanTypeName: string, _documentTypeSync: boolean = false, _loanTypeSync: boolean = false) {
        this.ID = _Id;
        this.CustomerID = _custID;
        this.LoanTypeID = _loanTypeID;
        this.LoanTypeName = _loanTypeName;
        this.DocumentTypeSync = _documentTypeSync;
        this.LoanTypeSync = _loanTypeSync;
    }
}
