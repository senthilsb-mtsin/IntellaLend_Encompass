import { ApplicationConfigurationComponent } from './pages/application-configuration/application-configuration.page';
import { Routes, RouterModule } from '@angular/router';

const ApplicationConfigRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/',
  },
  { path: '', component: ApplicationConfigurationComponent },
];
export const appconfigrouting = RouterModule.forChild(ApplicationConfigRoutes);
