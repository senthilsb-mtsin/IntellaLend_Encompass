import { IMyOptions, IMyDate } from '@mts-date-picker/interfaces';
import { AppSettings } from '@mts-app-setting';

export class ImportConfigModel {
  myDatePickerOptions: IMyOptions = {
    dateFormat: 'mm/dd/yyyy',
    showClearDateBtn: false
  };
  AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
  selDate: IMyDate = { year: 0, month: 0, day: 0 };
  reviewTypeItems: any[];
  loanSelect: any;
  BoxAuditDate: any = new Date();

  customerSelect: any;
  reviewselect: any = [];
  reviewTypeId: any;
  selectedPriority: any;
  commonActiveCustomerItems: any;
  loanTypeItems: any[];
  loanTypeId: any;
  enableUpload = true;
  constructor() {
    this.customerSelect = 0;
    this.reviewTypeId = 0;
    this.selectedPriority = 0;
    this.loanSelect = 0;
    this.loanTypeItems = [];
    this.reviewTypeItems = [];
  }
}
