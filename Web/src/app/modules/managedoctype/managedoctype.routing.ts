
import { RouterModule, Routes } from '@angular/router';
import { ManagerDocumentTypeComponent } from './pages/managedoctype/managedoctype.page';
import { ManageViewDocumentTypeComponent } from './pages/manageviewdoctype/manageviewdoctype.page';
import { ManagerEditDocumentTypeComponent } from './pages/manageeditdoctype/manageeditdoctype.page';

const ManagerDocumentRoutes: Routes = [
    {
        path: 'login',
        redirectTo: '/'
    },
    {
        path: '',
        component: ManagerDocumentTypeComponent
    },

     {
         path: 'manageeditdoctype',
         component: ManagerEditDocumentTypeComponent
     },
     {
         path: 'manageviewdoctype',
         component: ManageViewDocumentTypeComponent
     }
];

export const ManagerDocumentRouting = RouterModule.forChild(ManagerDocumentRoutes);
