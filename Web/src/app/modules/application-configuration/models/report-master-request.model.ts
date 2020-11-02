export class ReportMasterRequestModel {
  TableSchema: string;
  DocName: string;
  MasterID: any;
  ServiceType?: any;
  constructor(
    TableSchema: string,
    DocName: string,
    MasterID: any,
    ServiceType?: any
  ) {
    this.TableSchema = TableSchema;
    this.DocName = DocName;
    this.MasterID = MasterID;
    this.ServiceType = ServiceType;
  }
}
