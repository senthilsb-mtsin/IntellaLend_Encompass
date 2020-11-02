
import { AddParkingSpotdata, EditParkingSpotdata } from './../models/parking-spot-model';
import { ParkingSpotRowData } from './../models/parking-spot-rowdata.model';
import { EncompassPSpotDataAccess } from '../encompass-parkingspot.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
const jwtHelper = new JwtHelperService();
@Injectable()
export class EncompassPSpotService {
  EmcompassPSpotTabledata = new Subject();
  ParkingSpotRowData = new Subject();

  constructor(
    private parkingspotdata: EncompassPSpotDataAccess,
    private _notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) { }
  private encompassdata = [];
  GetParkingSpotDetails(pmInputReq: { TableSchema: string }) {
    return this.parkingspotdata
      .GetParkingSpotdetails(pmInputReq)
      .subscribe((res) => {
        if (res !== null) {
          const _EncompassParrkingSpot = jwtHelper.decodeToken(res.Data)[
            'data'
          ];
          if (_EncompassParrkingSpot.length > 0) {
            this.encompassdata = [..._EncompassParrkingSpot];
            this.EmcompassPSpotTabledata.next(_EncompassParrkingSpot);
          }
        }
      });
  }

  AddParkingSpotDetails(inputData: AddParkingSpotdata) {
    return this.parkingspotdata
      .AddParkingSpotdetails(inputData)
      .subscribe((res) => {
        if (res !== null) {
          const _EncompassParrkingSpot = jwtHelper.decodeToken(res.Data)[
            'data'
          ];
          if (_EncompassParrkingSpot.length > 0) {
            this.EmcompassPSpotTabledata.next(_EncompassParrkingSpot);
            this._notificationService.showSuccess(
              'Encompass ParkSpot Added Successfully'
            );
            this.CloseParkSpot();
          }
        }
      });
  }
  EditParkingSpotDetails(inputData: EditParkingSpotdata) {
    return this.parkingspotdata
      .EditParkingSpotdetails(inputData)
      .subscribe((res) => {
        if (res !== null) {
          const _EncompassParrkingSpot = jwtHelper.decodeToken(res.Data)[
            'data'
          ];
          if (_EncompassParrkingSpot.length > 0) {
            this.EmcompassPSpotTabledata.next(_EncompassParrkingSpot);
            this._notificationService.showSuccess(
              'Encompass ParkSpot Updated Successfully'
            );
            this.CloseParkSpot();
          }
        }
      });
  }
  SetTableRowData(inputData: ParkingSpotRowData) {
    this.ParkingSpotRowData.next(inputData);

  }
  CloseParkSpot() {
    this.location.back();
    // this.router.navigate(['encompassparkingspot'], {relativeTo: this.route});
  }
  Validate(inputdata: any) {

    let count = false;
    if (inputdata.ParkingSpotName.trim() === '') {

      return 'Parking-Spot name required';
    } else if (!inputdata.hasOwnProperty('ParkingSpotID')) {
      count = this.checkcount(inputdata.ParkingSpotName, -1) > 0;
    } else if (inputdata.hasOwnProperty('ParkingSpotID')) {
      count = this.checkcount(inputdata.ParkingSpotName, inputdata.ParkingSpotID) > 0;
    }

    return count ? 'Parking-Spot name already exists' : '';
  }

  checkcount(ParkingSpotName: string, id: number) {
    return this.encompassdata.filter(a => a.ParkingSpotName.toLocaleUpperCase() === ParkingSpotName.toLocaleUpperCase() && a.ID !== id).length;
  }
}
