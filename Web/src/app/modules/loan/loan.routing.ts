import { RouterModule, Routes } from '@angular/router';
import { LoanComponent } from './pages/loan/loan.page';
import { LoanDetailExportComponent } from './helper-components/loan-detail-export/loan-detail-export.page';
import { LoanPopOutComponent } from './helper-components/loan-popout/loan-popout.page';

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
        path: 'export',
        component: LoanDetailExportComponent
    }
];

export const loanRouting = RouterModule.forChild(loanRoutes);
