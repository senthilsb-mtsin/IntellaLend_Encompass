export class AddDocumentFieldRequestModel {
    DocumentTypeID = 0;
    FieldName = '';
    FieldDisplayName = '';
    constructor(_docID: number, _fieldName: string, _fieldDisplayName: string) {
        this.DocumentTypeID = _docID;
        this.FieldName = _fieldName;
        this.FieldDisplayName = _fieldDisplayName;
    }
}
