export class AssignLoanUserRequest {
    TableSchema: string;
    LoanID: number;
    AssignedUserID: number;
    CurrentUserID: number;
    ServiceTypeName: string;
    AssignedBy: string;
    AssignedTo: string;

    constructor(_schema: string, _loanID: number, _assignUserID: number, _currentUserID: number, _serviceName: string, _assignedBy: string, _assignedTo: string) {
        this.TableSchema = _schema;
        this.LoanID = _loanID;
        this.AssignedUserID = _assignUserID;
        this.CurrentUserID = _currentUserID;
        this.ServiceTypeName = _serviceName;
        this.AssignedBy = _assignedBy;
        this.AssignedTo = _assignedTo;
    }
}
