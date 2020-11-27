export class CheckWebHookEventTypeExistModal{
    TableSchema: string;
    EventType: number;
    constructor(TableSchema: string, EventType: number){
        this.TableSchema = TableSchema;
        this.EventType = EventType;
    }
}