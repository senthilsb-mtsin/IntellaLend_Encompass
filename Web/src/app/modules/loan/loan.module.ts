import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { MentionModule } from '@mts-mention-plus';
import { LoanComponent } from './pages/loan/loan.page';
import { loanRouting } from './loan.routing';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { EmailCheckPipe, SharedPipeModule } from '@mts-pipe';
import { FileUploadModule } from 'ng2-file-upload';
import { DocumentsearchPipeModule } from './pipes/stack-document-search.pipe';
import { LoanStipulationComponent } from './helper-components/loan-stipulation/loan-stipulation.page';
import { LoanReverificationComponent } from './helper-components/loan-reverification/loan-reverification.page';
import { FieldsearchPipeModule } from './pipes/field-search.pipe';
import { SelectModule } from '@mts-select2';
import { LoanDetailDashboardComponent } from './helper-components/loan-detail-dashboard/loan-detail-dashboard.page';
import { LoanHeaderComponent } from './helper-components/loan-header/loan-header.page';
import { CheckListTabComponent } from './helper-components/check-list-tab/check-list-tab.page';
import { HistoryTabComponent } from './helper-components/history-tab/history-tab.page';
import { NotesTabComponent } from './helper-components/notes-tab/notes-tab.page';
import { LoanDetailExportComponent } from './helper-components/loan-detail-export/loan-detail-export.page';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { LoanSharedModule } from './loan-shared.module';
import { LoanVODLetterComponent } from './helper-components/loan-reverification/loan-vod-letter/loan-vod-letter.page';
import { LoanVOELetterComponent } from './helper-components/loan-reverification/loan-voe-letter/loan-voe-letter.page';
import { LoanGiftLetterComponent } from './helper-components/loan-reverification/loan-gift-letter/loan-gift-letter.page';
import { LoanOccupancyLetterComponent } from './helper-components/loan-reverification/loan-occupancy-letter/loan-occupancy-letter.page';
import { LoanCPALetterComponent } from './helper-components/loan-reverification/loan-cpa-letter/loan-cpa-letter.page';
import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';
import { FannieMaeFieldsModule } from '../fanniemaeFields/fanniemaeFields.module';
import { FannieMaeFieldsService } from '../fanniemaeFields/services/fanniemaeFields.service';
import { FannieMaeFieldsDataAccess } from '../fanniemaeFields/fanniemaeFields.data';
@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        MalihuScrollbarModule,
        DataTablesModule,
        ModalModule.forRoot(),
        TypeaheadModule.forRoot(),
        NgBusyModule,
        MentionModule.forRoot(),
        FileUploadModule,
        loanRouting,
        MyDatePickerModule,
        PdfViewerModule,
        DocumentsearchPipeModule,
        FieldsearchPipeModule,
        SelectModule,
        TooltipModule.forRoot(),
        SharedPipeModule,
        LoanSharedModule,
        MonthYearPickerModule,
        FannieMaeFieldsModule
    ],
    providers: [
        EmailCheckPipe,
        FannieMaeFieldsService,
        FannieMaeFieldsDataAccess
     ],
    declarations: [
        LoanComponent,
        LoanDetailDashboardComponent,
        LoanHeaderComponent,
        CheckListTabComponent,
        HistoryTabComponent,
        NotesTabComponent,
        LoanStipulationComponent,
        LoanReverificationComponent,
        LoanDetailExportComponent,
        LoanCPALetterComponent,
        LoanVODLetterComponent,
        LoanVOELetterComponent,
        LoanGiftLetterComponent,
        LoanOccupancyLetterComponent
    ],
})
export class LoanModule { }
