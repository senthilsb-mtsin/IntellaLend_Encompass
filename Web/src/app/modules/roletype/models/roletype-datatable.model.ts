
export class Roletypemodel {
  RoleName: any;
  Active = true;
  RoleID: any;
  StartPage: any;
  IsChecked = false;
  IncludeKpi = false;
  ADGroupID: any;
  constructor(RoleName: any = '', Active: boolean = true, StartPage: any = [], IsChecked: boolean = false, IncludeKpi: boolean = false, RoleID: any = -1, ADGroupID: any = 0) {
    this.RoleName = RoleName;
    this.Active = Active;
    this.RoleID = RoleID;
    this.StartPage = StartPage;
    this.IsChecked = IsChecked;
    this.IncludeKpi = IncludeKpi;
    this.ADGroupID = ADGroupID;
  }
}
