export class ServiceTypeModel {
  constructor(public ReviewTypeID: number,
    public ReviewTypeName: string,
    public ReviewTypePriority: number,
    public Active: boolean,
    public BatchClassInputPath: string,
    public SearchCriteria: number,
    public UserRoleID: number) {
  }
}
