import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgBusyModule } from '../../shared/custom-plugins/angularbusy/ng-busy.module';
import { EncompassParkingspotComponent } from './pages/encompass-parkingspot/encompass-parkingspot.page';
import { DataTablesModule } from 'angular-datatables';
import { EmcompassRouting } from './encompass-parkingspot.routing';
import { EncompassPSpotService } from './service/encompass-parkingspot.service';
import { EncompassPSpotDataAccess } from './encompass-parkingspot.data';
import { AddEncompassParkingspotComponent } from './pages/add-encompass-parkingspot/add-encompass-parkingspot.page';
import { ViewEncompassParkingspotComponent } from './pages/view-encompass-parkingspot/view-encompass-parkingspot.page';
import { EditEncompassParkingspotComponent } from './pages/edit-encompass-parkingspot/edit-encompass-parkingspot.page';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    EncompassParkingspotComponent,
    AddEncompassParkingspotComponent,
    EditEncompassParkingspotComponent,
    ViewEncompassParkingspotComponent,
  ],
  imports: [
    CommonModule,
    NgBusyModule.forRoot({
      backdrop: true,
    }),
    DataTablesModule,
    EmcompassRouting,
    FormsModule,
  ],
  providers: [EncompassPSpotService, EncompassPSpotDataAccess],
})
export class EncompassParkingspotModule {}
