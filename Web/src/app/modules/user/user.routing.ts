
import { RouterModule, Routes } from '@angular/router';
import { AddUserComponent } from './pages/adduser/adduser.page';
import { EditUserComponent } from './pages/edituser/edituser.page';
import { ViewUserComponent } from './pages/viewuser/viewuser.page';
import { UserComponent } from './pages/user/user.page';

const UserRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: UserComponent
    },
    {
        path: 'adduser',
        data: { routeURL: 'View\\User\\AddBtn' },
        component: AddUserComponent
    },
    {
        path: 'edituser',
        data: { routeURL: 'View\\User\\EditBtn' },
        component: EditUserComponent
    },
    {
        path: 'viewuser',
        data: { routeURL: 'View\\User\\ViewBtn' },
        component: ViewUserComponent
    },
];

export const UserRouting = RouterModule.forChild(UserRoutes);
