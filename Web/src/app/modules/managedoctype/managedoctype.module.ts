
import { SelectModule } from '@mts-select2';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DataTablesModule } from 'angular-datatables';
import { CommonService } from 'src/app/shared/common/common.service';
import { DragulaModule, DragulaService } from '@mts-dragula';
import { SubDragulaService } from 'src/app/shared/custom-plugins/ng2-dragula/components/dragula.service';

import { ManagerDocumentTypeComponent } from './pages/managedoctype/managedoctype.page';
import { ManagerDocumentRouting } from './managedoctype.routing';
import { ManagerDocTypeDataAccess } from './managedoctype.data';
import { ManagerDoctypeService } from './services/manage-doctype.service';
import { DocumentFieldApiUrlConstant } from '@mts-api-url';
import { DocumentDataAccess } from '../documenttype/documenttype.data';
import { ManageViewDocumentTypeComponent } from './pages/manageviewdoctype/manageviewdoctype.page';
import { ManagerEditDocumentFieldComponent } from './helpercomponents/editmdocfield/editmdocfield.page';
import { ManagerEditDocumentTypeComponent } from './pages/manageeditdoctype/manageeditdoctype.page';
import { ManagerEditFieldComponent } from './helpercomponents/editmfield/editmfield.page';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SelectModule,
    ManagerDocumentRouting,
    DataTablesModule,
    MyDatePickerModule,
    NgDateRangePickerModule,
    NgBusyModule,
    ModalModule.forRoot(),
    DragulaModule.forRoot()
  ],
  providers: [
    CommonService,
    ManagerDoctypeService,
    ManagerDocTypeDataAccess,
    DocumentDataAccess,
    DragulaService,
    SubDragulaService
  ],
  declarations: [
    ManagerDocumentTypeComponent,
    ManagerEditDocumentTypeComponent,
    ManageViewDocumentTypeComponent,
    ManagerEditDocumentFieldComponent,
    ManageViewDocumentTypeComponent,
    ManagerEditFieldComponent

  ],

})
export class ManagerDocumentTypeModule {

}
