import { Routes, RouterModule } from '@angular/router';
import { LoanImportComponent } from './pages/loan-import/loan-import.page';

const importRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/'
  },
  { path: '', component: LoanImportComponent },

];
export const importRouting = RouterModule.forChild(importRoutes);
