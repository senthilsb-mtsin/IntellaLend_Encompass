
export class SaveBatchModel {
  TableSchema: string;
  JobName: any;
  ExportedBy = '';
  CustomerID: any;
  CoverLetter = false;
  TableOfContent = false;
  PasswordProtected = false;
  Password = '';
  CoverLetterContent = '';
  BatchLoanDoc: any;
  constructor(
    TableSchema: string,
    JobName: any,
    ExportedBy: string,
    CustomerID: any,
    CoverLetter: boolean,
    TableOfContent: boolean,
    PasswordProtected: boolean,
    Password: string,
    CoverLetterContent: string,
    BatchLoanDoc: any) {
    this.TableSchema = TableSchema;
    this.JobName = JobName;
    this.ExportedBy = ExportedBy;
    this.CustomerID = CustomerID;
    this.CoverLetter = CoverLetter;
    this.TableOfContent = TableOfContent;
    this.PasswordProtected = PasswordProtected;
    this.Password = Password;
    this.CoverLetterContent = CoverLetterContent;
    this.BatchLoanDoc = BatchLoanDoc;
  }
}
