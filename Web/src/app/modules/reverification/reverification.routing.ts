import { RouterModule, Routes } from '@angular/router';
import { ReverificationComponent } from './pages/reverification/reverification.page';
import { ViewReverificationComponent } from './pages/viewreverification/viewreverification.page';
import { EditReverificationComponent } from './pages/editreverification/editreverification.page';
import { AddReverificationComponent } from './pages/addreverification/addreverification.page';

const ReverificationRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: ReverificationComponent
    },
    {
        path: 'addreverification',
        data: { routeURL: 'View\\addreverification\\addreverification' },
        component: AddReverificationComponent
    },
    {
        path: 'editreverification',
        data: { routeURL: 'View\\editreverification\\editreverification' },
        component: EditReverificationComponent
    },
    {
        path: 'viewreverification',
        data: { routeURL: 'View\\reverification\\viewreverification' },
        component: ViewReverificationComponent
    },

];

export const ReverificationRouting = RouterModule.forChild(ReverificationRoutes);
