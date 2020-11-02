export class GetLoanSearchFilterRequest {
  TableSchema: string;
  CustomerID: number;
  ConfigKey: string;

  constructor(_tableSchema: string, _customerID: number, _configKey: string) {
    this.TableSchema = _tableSchema;
    this.CustomerID = _customerID;
    this.ConfigKey = _configKey;
  }

}
