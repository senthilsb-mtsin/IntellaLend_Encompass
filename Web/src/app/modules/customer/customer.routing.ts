import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './pages/customer/customer.page';
import { UpsertCustomerComponent } from './pages/upsert-customer/upsert-customer.page';

const customerRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/',
    },
    {
        path: '',
        component: CustomerComponent
    },
    {
        path: 'addcustomer',
        component: UpsertCustomerComponent
    },
    {
        path: 'editcustomer',
        component: UpsertCustomerComponent
    }
];

export const customerRouting = RouterModule.forChild(customerRoutes);
