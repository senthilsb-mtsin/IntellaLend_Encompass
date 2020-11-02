export class AssignDocTypeRequestModel {
  ReverificationID: number;
  DocTypeIDs: number[];
  constructor(_reverifyID: number, _docIDs: number[]) {
    this.ReverificationID = _reverifyID;
    this.DocTypeIDs = _docIDs;
  }
}
