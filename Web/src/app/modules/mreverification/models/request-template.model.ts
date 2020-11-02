export class RequestTemplateModel {
    TableSchema: any;
    MappingID: any;
    TemplateFieldJson: any;
    constructor(tableSchema: any, MappingID: any,
        TemplateFieldJson: any,
       ) {
        this.TableSchema = tableSchema;
        this.MappingID = MappingID;
        this.TemplateFieldJson = TemplateFieldJson;

    }
}
