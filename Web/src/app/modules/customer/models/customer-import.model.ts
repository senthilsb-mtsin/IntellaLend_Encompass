export class CustomerImportStagingRequestModel {
    TableSchema: string;
    Status: number;
    ImportDateFrom: Date;
    ImportDateTo: Date;
    AssignType: number

    constructor(
        TableSchema: string,
        Status: number,
        ImportDateFrom: Date,
        ImportDateTo: Date,
        AssignType: number ) {
            this.TableSchema = TableSchema;
            this.Status = Status;
            this.ImportDateFrom = ImportDateFrom;
            this.ImportDateTo = ImportDateTo;
            this.AssignType = AssignType;
    }
}

export class CustomerImportStagingDetailsRequestModel {
    TableSchema: string;
    CustomerImportStagingID: number;

    constructor (
        TableSchema: string,
        CustomerImportStagingID: number ) {
            this.TableSchema = TableSchema;
            this.CustomerImportStagingID = CustomerImportStagingID;
    }
}
