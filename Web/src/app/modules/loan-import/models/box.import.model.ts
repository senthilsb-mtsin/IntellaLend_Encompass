export class BoxFileListRequestModel {
  TableSchema: string;
  UserID: number;
  FolderID: number;
  limit: number;
  FileFilter: string;
  constructor(TableSchema: string,
    UserID: number,
    FolderID: number,
    limit: number,
    offSet: number,
    FileFilter: string) {
    this.TableSchema = TableSchema;
    this.UserID = UserID;
    this.FolderID = FolderID;
    this.limit = limit;
    this.FileFilter = FileFilter;
  }
}
export class FolderItemCountRequestModel {
  ReviewType: number;
  UserID: number;
  CustomerID: number;
  TableSchema: string;
  BoxItems: any;
  FileFilter: string;
  constructor(ReviewType: number,
    UserID: number,
    CustomerID: number,
    TableSchema: string,
    BoxItems: any,
    FileFilter: string) {
    this.TableSchema = TableSchema;
    this.UserID = UserID;
    this.ReviewType = ReviewType;
    this.CustomerID = CustomerID;
    this.FileFilter = FileFilter;
    this.BoxItems = BoxItems;
  }
}
