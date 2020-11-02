import { RouterModule, Routes } from '@angular/router';
import { LoanTypeComponent } from './pages/loantype/loantype.page';
import { AddLoanTypeComponent } from './pages/add-loantype/add-loantype.page';

const loantypeRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/',
  },
  {
    path: '',
    component: LoanTypeComponent
  },
  {
    path: 'addloantype',
    component: AddLoanTypeComponent
  },
  {
    path: 'editloantype',
    component: AddLoanTypeComponent
  }
];

export const loantypeRouting = RouterModule.forChild(loantypeRoutes);
