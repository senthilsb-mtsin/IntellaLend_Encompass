export class PurgeMonitorRequest {
  TableSchema: string;
  FromDate: any;
  ToDate: any;
  WorkFlowStatus: any;

  constructor(tableSchema: string, fromdate: any, todate: any, workflowstatus: any) {
    this.TableSchema = tableSchema;
    this.FromDate = fromdate;
    this.ToDate = todate;
    this.WorkFlowStatus = workflowstatus;
  }
}
