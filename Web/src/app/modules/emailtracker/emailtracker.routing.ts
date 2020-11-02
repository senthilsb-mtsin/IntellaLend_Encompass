import { RouterModule, Routes } from '@angular/router';
import { EmailtrackerComponent } from './pages/emailtracker.page';

const EmailTrackerRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: EmailtrackerComponent
    },

];
export const Emailrouting = RouterModule.forChild(EmailTrackerRoutes);
