
import { AddParkingSpotdata } from './../../models/parking-spot-model';
import { Component, OnInit } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { EncompassPSpotService } from '../../service/encompass-parkingspot.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-encompass-parkingspot',
  templateUrl: './add-encompass-parkingspot.page.html',
  styleUrls: ['./add-encompass-parkingspot.page.css'],
})
export class AddEncompassParkingspotComponent implements OnInit {
  ParkingSpotName = '';
  ParkingActive = true;
  promise: Subscription;
  validationMsg = '';
  constructor(
    private _parkingSpotService: EncompassPSpotService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void { }
  AddParkSpotSubmit() {

    const inputData: AddParkingSpotdata = {
      TableSchema: AppSettings.TenantSchema,
      ParkingSpotName: this.ParkingSpotName.trim(),
      Active: this.ParkingActive,
    };
    const check = this._parkingSpotService.Validate(inputData);
    if (check === '') {
      this.promise = this._parkingSpotService.AddParkingSpotDetails(inputData);
    } else {
      this.validationMsg = check;

    }
  }
  CloseParkSpot() {
    this._parkingSpotService.CloseParkSpot();

  }
}
