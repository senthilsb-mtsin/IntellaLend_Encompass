import { ExportLoanWizardComponent } from './pages/export-loan-wizard/export-loan-wizard.page';
import { ExportComponent } from './pages/export/export.page';
import { RouterModule, Routes } from '@angular/router';
const exportRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/'
  },
  { path: '', component: ExportComponent },
  { path: 'addbatchloan', component: ExportLoanWizardComponent },
];

export const exportRouting = RouterModule.forChild(exportRoutes);
