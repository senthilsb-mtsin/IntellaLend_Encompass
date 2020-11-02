export class CategoryListData {
  Category = '';
  Active: any = '';
  IsMappedCheckList: any = '';
  OldCategory: any = '';
  constructor(
    Category: string,
    Active: any,
    IsMappedCheckList: any,
    OldCategory: any
  ) {
    this.Category = Category;
    this.Active = Active;
    this.IsMappedCheckList = IsMappedCheckList;
    this.OldCategory = OldCategory;
  }
}
export class SaveCategorymodel {
  TableSchema: string;
  Category: string;
  Active: boolean;

  constructor(TableSchema: string, Category: string, Active: boolean) {
    this.TableSchema = TableSchema;
    this.Category = Category;
    this.Active = Active;
  }
}
export class UpdateCategorymodel {
  TableSchema: string;
  categoryList: CategoryListData;
  constructor(TableSchema: string, categoryList: CategoryListData) {
    this.TableSchema = TableSchema;
    this.categoryList = categoryList;
  }
}
