import { AddParkingSpotdata, EditParkingSpotdata } from './models/parking-spot-model';
import { EmcompassPSpotConstant } from './../../shared/constant/api-url-constants/encompass-parkingspot-api-url.constant';
import { APIService } from './../../shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
@Injectable()
export class EncompassPSpotDataAccess {
  constructor(private _apiService: APIService) {}
  GetParkingSpotdetails(pmInputReq: {
    TableSchema: string;
  }): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      EmcompassPSpotConstant.GET_PARKINGSPOT_DETAILS,
      pmInputReq
    );
  }
  AddParkingSpotdetails(pmInputReq: AddParkingSpotdata): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      EmcompassPSpotConstant.ADD_PARKINGSPOT_DETAILS,
      pmInputReq
    );
  }
  EditParkingSpotdetails(pmInputReq: EditParkingSpotdata): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      EmcompassPSpotConstant.EDIT_PARKINGSPOT_DETAILS,
      pmInputReq
    );
  }
}
