export class SearchEmailTrackerDetailsRequest {
    TableSchema: string;
    EmailTracker: EmailTracker;
    constructor(_tableSchema: string, _emailtracker: any) {
        this.TableSchema = _tableSchema;
        this.EmailTracker = _emailtracker;
    }

}
class EmailTracker {
    FromDateStr: any;
    ToDateStr: any;
    constructor(fromDateStr: any, toDatStr: any) {
        this.FromDateStr = fromDateStr;
        this.ToDateStr = toDatStr;
    }
}
