import { ConfigRequestModel } from './config-request.model';
export class UpdateLoanSearchModel extends ConfigRequestModel {
  ConfigID: number;
  Active: boolean;
  constructor(TableSchema: string, ConfigID: number, Active: boolean) {
    super(TableSchema);
    this.ConfigID = ConfigID;
    this.Active = Active;
  }
}
