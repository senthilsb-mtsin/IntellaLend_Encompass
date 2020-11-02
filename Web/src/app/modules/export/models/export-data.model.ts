import { Subscription } from 'rxjs';

export class ExportConfigModel {
  commonActiveCustomerItems: any;
  AddCustomer = 0;
  customerSelect: any = 0;
  ExportStatus: any = -1;
  UploadStatus: any = 5;
  promise: Subscription;
  rowData: any;
}
