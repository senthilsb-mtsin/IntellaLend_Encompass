export class RetryLOSExportModel {
    TableSchema: string;
    ID: number;
    LoanID: number;
    constructor(TableSchema: string,
        ID: number, LoanID: number) {
        this.TableSchema = TableSchema,
            this.ID = ID;
        this.LoanID = LoanID;
     }
}
