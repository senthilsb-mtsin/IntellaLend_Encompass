import { Routes, RouterModule } from '@angular/router';
import { ServiceTypeComponent } from './pages/service-type/service-type.page';
import { AddServiceTypeComponent } from './pages/add-service-type/add-service-type.page';
import { ViewServiceTypeComponent } from './pages/view-service-type/view-service-type.page';

const ServiceTypeRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/',
    },
    {
        path: '',
        component: ServiceTypeComponent
    },
    {
        path: 'addreviewtype',
        component: AddServiceTypeComponent
    },
    {
        path: 'editreviewtype',
        component: AddServiceTypeComponent
    },
    {
        path: 'viewreviewtype',
        component: ViewServiceTypeComponent
    }
];

export const ServiceTypeRouting = RouterModule.forChild(ServiceTypeRoutes);
