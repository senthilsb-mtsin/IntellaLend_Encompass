export class AddLoantypeRequestModel {
  TableSchema: string;
  LoanType: AddLoantypeModel;

  constructor(tableSchema: string, loanTypeID: number,
    loanTypeName: string,
    loanTypePriority: any,
    active: boolean) {
    this.TableSchema = tableSchema;
    this.LoanType = new AddLoantypeModel(loanTypeID,
      loanTypeName,
      loanTypePriority,
      active);

  }
}

class AddLoantypeModel {
  LoanTypeID: number;
  LoanTypeName: string;
  LoanTypePriority: any;
  Active: boolean;
  constructor(loanTypeID: any,
    loanTypeName: any,
    loanTypePriority: any,
    active: boolean) {
    this.LoanTypeID = loanTypeID;
    this.LoanTypeName = loanTypeName;
    this.LoanTypePriority = loanTypePriority;
    this.Active = active;
  }
}
