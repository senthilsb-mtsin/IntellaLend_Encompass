import { SelectModule } from '@mts-select2';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { LoginComponent } from './pages/login/login.page';
import { loginRouting } from './login.routing';
import { CommonModule } from '@angular/common';
import { LoginDataAccess } from './login.data';
import { ForgetPasswordService } from './services/forget-password.service';
import { LoginService } from './services/login.service';
import { ForgetpasswordComponent } from './pages/forgetpassword/forgetpassword.page';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NewUserComponent } from './pages/newuser/new-user.page';
@NgModule({
  imports: [
    SelectModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TooltipModule.forRoot(),
    loginRouting,
  ],
  providers: [
    LoginService,
    LoginDataAccess,
    ForgetPasswordService
  ],
  declarations: [LoginComponent, ForgetpasswordComponent, NewUserComponent],
  exports: [LoginComponent, ForgetpasswordComponent, NewUserComponent]
})
export class LoginModule { }
