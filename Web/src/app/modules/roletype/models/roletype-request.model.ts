import { Roletypemodel } from './roletype-datatable.model';

export class AddRoleTypeRequestModel {
  TableSchema: string;
  RoleType: Roletypemodel;
  menus: any = [];

  constructor(tableschema: string, RoleType: Roletypemodel, menus: any) {
    this.TableSchema = tableschema;
    this.menus = menus;
    this.RoleType = RoleType;
  }
}
