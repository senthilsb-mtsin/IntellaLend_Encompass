import { ConfigRequestModel } from './config-request.model';

export class AddUpdateStipulationModel extends ConfigRequestModel {
  SCategory: any;
  Active: boolean;
  ID?: number;
  constructor(
    TableSchema: string,
    SCategory: any,
    Active: boolean,
    ID?: number
  ) {
    super(TableSchema);
    this.SCategory = SCategory;
    this.Active = Active;
    this.ID = ID;
  }
}
