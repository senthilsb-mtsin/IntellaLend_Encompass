export class LosExportdateModel {
    ExportedDate: any;
    TableSchema: string;
    Customer: number;
    LoanType: number;
    ServiceType: number;
   constructor(TableSchema: string, Customer: number,

        ExportedDate: any, LoanType: number, ServiceType: number) {
        this.TableSchema = TableSchema,
            this.ExportedDate = ExportedDate;
            this.Customer =  Customer;
            this.LoanType = LoanType;
            this.ServiceType = ServiceType;

    }
}
