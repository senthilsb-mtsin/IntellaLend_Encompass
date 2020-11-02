import { LoginComponent } from './pages/login/login.page';
import { RouterModule, Routes } from '@angular/router';
import { ForgetpasswordComponent } from './pages/forgetpassword/forgetpassword.page';
import { NewUserComponent } from './pages/newuser/new-user.page';

const loginRoutes: Routes = [
  {
    path: '',
    component: LoginComponent
  },
  {
    path: 'forgetpassword',
    component: ForgetpasswordComponent
  },
  {
    path: 'newuser',
    component: NewUserComponent
  }
];

export const loginRouting = RouterModule.forChild(loginRoutes);
