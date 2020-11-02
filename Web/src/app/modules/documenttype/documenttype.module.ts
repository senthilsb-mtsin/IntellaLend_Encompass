
import { SelectModule } from '@mts-select2';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { DataTablesModule } from 'angular-datatables';
import { DocumentRouting } from './documenttype.routing';
import { DocumentTypeService } from './services/documenttype.service';
import { DocumentDataAccess } from './documenttype.data';
import { CommonService } from 'src/app/shared/common/common.service';
import { DocumentFieldService } from './services/documentfield.service';
import { DragulaModule, DragulaService } from '@mts-dragula';
import { SubDragulaService } from 'src/app/shared/custom-plugins/ng2-dragula/components/dragula.service';
import { DocumentTypeComponent } from './pages/documenttype/documenttype.page';
import { AddDocumentTypeComponent } from './pages/adddocumenttype/adddocumenttype.page';
import { EditDocumentTypeComponent } from './pages/editdocumenttype/editdocumenttype.page';
import { ViewDocumentTypeComponent } from './pages/viewdocumenttype/viewdocumenttype.page';
import { EditDocumentFieldComponent } from './helper-components/document-field/editdocumentfield/editdocumentfield.page';
import { AddDocumentFieldComponent } from './helper-components/document-field/adddocumentfield/adddocumentfield.page';
import { EditFieldComponent } from './helper-components/document-field/editfield/editfield.page';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SelectModule,
    DocumentRouting,
    DataTablesModule,
    MyDatePickerModule,
    NgDateRangePickerModule,
    NgBusyModule,
    ModalModule.forRoot(),
    DragulaModule.forRoot()
  ],
  providers: [
    DocumentTypeService,
    CommonService,
    DocumentFieldService,
    CommonService,
    DocumentDataAccess,
    DragulaService,
    SubDragulaService
  ],
  declarations: [
    DocumentTypeComponent,
    AddDocumentTypeComponent,
    AddDocumentFieldComponent,
    EditDocumentTypeComponent,
    ViewDocumentTypeComponent,
    EditDocumentFieldComponent,
    EditFieldComponent,
  ],
  exports: [
    DocumentTypeComponent,
    AddDocumentTypeComponent,
    EditDocumentTypeComponent,
    ViewDocumentTypeComponent,
    AddDocumentFieldComponent,
    EditDocumentFieldComponent,
    EditFieldComponent,
  ]
})
export class DocumentTypeModule {

}
