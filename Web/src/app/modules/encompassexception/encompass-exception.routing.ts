import { RouterModule, Routes } from '@angular/router';
import { ConnectionAuthGuard } from '@mts-auth-guard/authguard.connetion';
import { EncompassDownloadExceptionComponent } from './pages/encompass-exception.page';

const EncompassExceptionRoutes: Routes = [

    {
        path: '',
        component: EncompassDownloadExceptionComponent,
    },
    {
        path: 'login',
        redirectTo: '/'
    },
];
export const EncompassRouting = RouterModule.forChild(EncompassExceptionRoutes);
