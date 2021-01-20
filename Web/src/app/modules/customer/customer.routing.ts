import { RouterModule, Routes } from '@angular/router';
import { CustomerImportMoniterComponent } from './pages/customer-import-moniter/customer-import-moniter.page';
import { CustomerComponent } from './pages/customer/customer.page';
import { SyncConfigComponent } from './pages/syncconfig/SyncConfig.page';
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
    },
    {
        path: 'import',
        component: CustomerImportMoniterComponent
    },
    {
        path: 'syncconfig',
        component: SyncConfigComponent
    }

];

export const customerRouting = RouterModule.forChild(customerRoutes);
