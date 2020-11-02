
export class GetRowDataRequest {
    TableSchema: string;
    ID: number;
    constructor(_tableSchema: string, _id: number) {
        this.TableSchema = _tableSchema;
        this.ID = _id;
    }
}
