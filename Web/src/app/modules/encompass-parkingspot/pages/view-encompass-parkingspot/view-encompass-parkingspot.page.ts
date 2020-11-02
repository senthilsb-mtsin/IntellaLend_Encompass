import { ParkingSpotRowData } from './../../models/parking-spot-rowdata.model';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { EncompassPSpotService } from '../../service/encompass-parkingspot.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-view-encompass-parkingspot',
  templateUrl: './view-encompass-parkingspot.page.html',
  styleUrls: ['./view-encompass-parkingspot.page.css'],
})
export class ViewEncompassParkingspotComponent implements OnInit, OnDestroy {
  ParkingSpotName = '';
  ParkingActive: any;

  constructor(private _parkingSpotService: EncompassPSpotService) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this._parkingSpotService.ParkingSpotRowData.subscribe((res: ParkingSpotRowData) => {
        this.ParkingActive = res.Active;
        this.ParkingSpotName = res.ParkingSpotName;
      })
    );

  }
  CloseParkSpot() {
    this._parkingSpotService.CloseParkSpot();
    // this.router.navigate([''], {relativeTo: this.route});
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
