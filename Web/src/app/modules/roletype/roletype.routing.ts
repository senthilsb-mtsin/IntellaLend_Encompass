import { RouterModule, Routes } from '@angular/router';
import { RoleTypeComponent } from './pages/roletype.page';
import { AddRoleTypeComponent } from './pages/add-roletype/add-roletype.page';
import { EditRoleTypeComponent } from './pages/edit-roletype/edit-roletype.page';
import { ViewRoleTypeComponent } from './pages/view-roletype/view-roletype.page';

const RoletypeRoutes: Routes = [
  {
    path: 'login',
    redirectTo: '/',
  },
  {
     path: '',
     component: RoleTypeComponent
  },
  {
    path: 'addroletype',
    component: AddRoleTypeComponent
  },
  {
    path: 'editroletype',
    component: EditRoleTypeComponent
  },
  {
    path: 'viewroletype',
    component: ViewRoleTypeComponent
  },

];

export const RoleTypeRouting = RouterModule.forChild(RoletypeRoutes);
