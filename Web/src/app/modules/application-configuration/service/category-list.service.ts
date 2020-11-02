import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigRequestModel } from '../models/config-request.model';
import { SaveCategorymodel, UpdateCategorymodel } from '../models/category-list.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class CategoryListService {
  categoryTableData$ = new Subject();
  isUpdated$ = new Subject();

  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService, ) { }
  GetAllCategory(inputs: ConfigRequestModel) {
    return this._appconfigdata.GetAllCategory(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.categoryTableData$.next(result);
        }
      }
    );
  }
  SaveCategoryData(inputs: SaveCategorymodel) {
    this._appconfigdata.SaveCategoryData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            this.isUpdated$.next(result);
            this._notificationservice.showSuccess('Saved Successfully');
          }
        }
      }
    );
  }

  UpdateCategoryData(inputs: UpdateCategorymodel) {
    this._appconfigdata.UpdateCategoryData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            this._notificationservice.showSuccess('Updated Successfully');
            this.isUpdated$.next(result);
          } else {
            this._notificationservice.showWarning('Updated Failed');
          }
        }
      }
    );
  }
}
