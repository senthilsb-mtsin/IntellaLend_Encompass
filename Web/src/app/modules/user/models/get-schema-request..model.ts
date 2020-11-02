
export class GetSchemaRequest {
    TableSchema: string;
    constructor(_tableschema: string) {

        this.TableSchema = _tableschema;
    }
}
