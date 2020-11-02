export class SaveCustReviewLoanMappingModel {
    TableSchema: string;
    CustomerID: number;
    ReviewTypeID: number;
    LoanTypeID: number;
    BoxUploadPath: string;
    LoanUploadPath: string;
    isRetainUpdate: boolean;

    constructor(_schema: string, _custID: number, _reviewID: number, _loanTypeID: number, _boxPath: string = '', _uploadPath: string = '', _isRetainUpdate: boolean = false) {
        this.TableSchema = _schema;
        this.CustomerID = _custID;
        this.ReviewTypeID = _reviewID;
        this.LoanTypeID = _loanTypeID;
        this.BoxUploadPath = _boxPath;
        this.LoanUploadPath = _uploadPath;
        this.isRetainUpdate = _isRetainUpdate;
    }
}
