export class CheckDocumentDuplicateRequestModel {
    DocumentTypeID: any;
    Name: any;
    DisplayName: any;
    ParkingSpotName: any;
    DocumentLevel: any;
    Active: any;
    ParkingSpotID: number;
     constructor( DocumentTypeID: any, _docName: any, _docDisplayName: any, ParkingSpotName: any, Active: any, _doclevel: any, _parkspotId: number) {
            this.DocumentTypeID = DocumentTypeID;
            this.Name = _docName;
            this.DisplayName = _docDisplayName;
            this.ParkingSpotName = ParkingSpotName;
            this.Active = Active;
            this.DocumentLevel = _doclevel;
            this.ParkingSpotID = _parkspotId;
        }

}
