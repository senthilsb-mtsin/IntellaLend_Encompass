
import { RouterModule, Routes } from '@angular/router';
import { ManagerReverificationComponent } from './pages/mreverification/mreverification.page';
import { ViewManagerReverificationComponent } from './pages/viewmreverification/viewmreverification.page';
import { ManagerEditReverificationComponent } from './pages/editmreverification/editmreverification.page';

const ManagerReverificationRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: ManagerReverificationComponent
    },

    {
        path: 'editmreverification',
        component: ManagerEditReverificationComponent
    },
    {
        path: 'viewmreverification',
        component: ViewManagerReverificationComponent
    },

];

export const ManagerReverificationRouting = RouterModule.forChild(ManagerReverificationRoutes);
