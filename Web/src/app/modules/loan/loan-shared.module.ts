import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { LoanStackingOrderComponent } from './helper-components/loan-stacking-order/loan-stacking-order.page';
import { LoanImageViewerComponent } from './helper-components/loan-image-viewer/loan-image-viewer.page';
import { LoanDocumentTableComponent } from './helper-components/document-table/document-table.page';
import { LoanDocumentFieldComponent } from './helper-components/document-field/loan-document-field.page';
import { FileUploadModule } from 'ng2-file-upload';
import { DocumentsearchPipeModule } from './pipes/stack-document-search.pipe';
import { FieldsearchPipeModule } from './pipes/field-search.pipe';
import { SelectModule } from '@mts-select2';
import { SharedPipeModule } from '@mts-pipe';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        MalihuScrollbarModule,
        DataTablesModule,
        ModalModule.forRoot(),
        NgBusyModule,
        FileUploadModule,
        MyDatePickerModule,
        DocumentsearchPipeModule,
        FieldsearchPipeModule,
        SelectModule,
        SharedPipeModule
    ],
    providers: [

    ],
    declarations: [
        LoanStackingOrderComponent,
        LoanImageViewerComponent,
        LoanDocumentTableComponent,
        LoanDocumentFieldComponent
    ],
    exports: [
        LoanStackingOrderComponent,
        LoanImageViewerComponent,
        LoanDocumentTableComponent,
        LoanDocumentFieldComponent
    ]
})
export class LoanSharedModule { }
