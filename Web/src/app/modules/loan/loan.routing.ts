import { RouterModule, Routes } from '@angular/router';
import { LoanComponent } from './pages/loan/loan.page';
import { LoanDetailExportComponent } from './helper-components/loan-detail-export/loan-detail-export.page';
import { LoanViewErrorComponent } from './helper-components/loan-view-error/loan-view-error.page';

const loanRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/',
    },
    {
        path: '',
        component: LoanComponent
    },
    {
        path: 'loanviewerror',
        component: LoanViewErrorComponent
    },
    {
        path: 'export',
        component: LoanDetailExportComponent
    },
    {
        path: ':id',
        component: LoanComponent
    }
];

export const loanRouting = RouterModule.forChild(loanRoutes);
