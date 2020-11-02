import { UserComponent } from './pages/user/user.page';
import { AddUserComponent } from './pages/adduser/adduser.page';
import { EditUserComponent } from './pages/edituser/edituser.page';
import { SelectModule } from '@mts-select2';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserRouting } from './user.routing';
import { ViewUserComponent } from './pages/viewuser/viewuser.page';
import { UserDataAccess } from './user.data';
import { UserService } from './services/user.service';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DataTablesModule } from 'angular-datatables';
import { EmailCheckPipe, CharcCheckPipe, TrimspacePipe, ValidateZipcodePipe, SharedPipeModule } from '@mts-pipe';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SelectModule,
    UserRouting,
    DataTablesModule,
    MyDatePickerModule,
    NgDateRangePickerModule,
    NgBusyModule,
    ModalModule.forRoot(),
    SharedPipeModule
  ],
  providers: [
    EmailCheckPipe,
    CharcCheckPipe,
    TrimspacePipe,
    UserService,
    UserDataAccess,
    ValidateZipcodePipe
  ],
  declarations: [
    UserComponent,
    AddUserComponent,
    EditUserComponent,
    ViewUserComponent,

  ],
  exports: [UserComponent,
    AddUserComponent,
    EditUserComponent,
    ViewUserComponent]
})
export class UserModule {

}
