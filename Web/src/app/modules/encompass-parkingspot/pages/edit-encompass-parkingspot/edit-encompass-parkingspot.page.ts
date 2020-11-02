import { EditParkingSpotdata } from './../../models/parking-spot-model';
import { ParkingSpotRowData } from './../../models/parking-spot-rowdata.model';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { EncompassPSpotService } from '../../service/encompass-parkingspot.service';
import { Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';

@Component({
  selector: 'app-edit-encompass-parkingspot',
  templateUrl: './edit-encompass-parkingspot.page.html',
  styleUrls: ['./edit-encompass-parkingspot.page.css'],
})
export class EditEncompassParkingspotComponent implements OnInit, OnDestroy {
  ParkingSpotName = '';
  ParkingActive = true;
  promise: Subscription;
  ParkingSpotID: any;
  selectedRow: any;
  validationMsg = '';
  constructor(private _parkingSpotService: EncompassPSpotService) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this._parkingSpotService.ParkingSpotRowData.subscribe(
        (res: ParkingSpotRowData) => {
          this.ParkingActive = res.Active;
          this.ParkingSpotName = res.ParkingSpotName;
          this.ParkingSpotID = res.ParkingSpotID;
        }
      )
    );
  }

  EditParkSpotSubmit() {

    const inputData: EditParkingSpotdata = {
      TableSchema: AppSettings.TenantSchema,
      ParkingSpotID: this.ParkingSpotID,
      ParkingSpotName: this.ParkingSpotName.trim(),
      Active: this.ParkingActive,
    };
    const check = this._parkingSpotService.Validate(inputData);
    if (check === '') {
      this.promise = this._parkingSpotService.EditParkingSpotDetails(inputData);
    } else {
      this.validationMsg = check;
    }

  }
  CloseParkSpot() {
    this._parkingSpotService.CloseParkSpot();

  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
