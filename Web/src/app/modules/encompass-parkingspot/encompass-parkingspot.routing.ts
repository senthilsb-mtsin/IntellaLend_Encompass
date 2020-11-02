import { ViewEncompassParkingspotComponent } from './pages/view-encompass-parkingspot/view-encompass-parkingspot.page';
import { EditEncompassParkingspotComponent } from './pages/edit-encompass-parkingspot/edit-encompass-parkingspot.page';
import { AddEncompassParkingspotComponent } from './pages/add-encompass-parkingspot/add-encompass-parkingspot.page';
import { EncompassParkingspotComponent } from './pages/encompass-parkingspot/encompass-parkingspot.page';
import { Routes, RouterModule } from '@angular/router';
import { ConnectionAuthGuard } from '@mts-auth-guard/authguard.connetion';

const ParkingspotRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/',
  },
  { path: '', component: EncompassParkingspotComponent },
  {
    path: 'addparkingspot',
    canActivate: [ConnectionAuthGuard],
    component: AddEncompassParkingspotComponent,
  },
  {
    path: 'editparkingspot',
    canActivate: [ConnectionAuthGuard],
    component: EditEncompassParkingspotComponent,
  },
  {
    path: 'viewparkingspot',
    canActivate: [ConnectionAuthGuard],
    component: ViewEncompassParkingspotComponent,
  },
];

export const EmcompassRouting = RouterModule.forChild(ParkingspotRoutes);
