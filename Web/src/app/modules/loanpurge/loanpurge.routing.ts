import { PurgemonitordetailComponent } from './pages/purgemonitordetail/purgemonitordetail.page';

import { RouterModule, Routes } from '@angular/router';
import { PurgeloanComponent } from './pages/purgeloan/purgeloan.page';

const purgeRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/'
  },
  { path: '', component: PurgeloanComponent},
  { path: 'purgedetail', component: PurgemonitordetailComponent},
];

export const purgeRouting = RouterModule.forChild(purgeRoutes);
