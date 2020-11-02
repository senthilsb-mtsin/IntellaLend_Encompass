export class AddDocumentTypeRequestModel {
    DocumentTypeName: any;
    DocumentDisplayName: any;
    DocumentLevel: any;
    ParkingSpotID: number;
    constructor(_docName: any, _docDisplayName: any, _doclevel: any, _parkspotId: number) {
        this.DocumentTypeName = _docName;
        this.DocumentDisplayName = _docDisplayName;
        this.DocumentLevel = _doclevel;
        this.ParkingSpotID = _parkspotId;
    }
}
