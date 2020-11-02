export class AssignDocumentTypeRequestModel {
  LoanTypeID: number;
  DocMappingDetails: DocMappingDetail[];

  constructor(_loanTypeID: number, DocMappingDetails: DocMappingDetail[]) {
    this.LoanTypeID = _loanTypeID;
    this.DocMappingDetails = DocMappingDetails;
  }
}
export class DocMappingDetail {
  DocumentTypeID: any;
  Name: any;
  DocumentLevel: any;
  Condition: any;
  DocumentFieldMasters: any;
  DocumetTypeTables: any;
  RuleDocumentTables: any;

}
