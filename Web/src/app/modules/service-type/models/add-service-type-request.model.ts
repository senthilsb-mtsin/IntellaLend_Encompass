import { ServiceTypeModel } from './service-type.model';

export class AddServiceTypeRequestModel {
  constructor(public TableSchema: string,
    public ReviewType: ServiceTypeModel) {
  }
}
