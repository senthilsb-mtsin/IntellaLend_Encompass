import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';
import { MalihuScrollbarModule } from 'ngx-malihu-scrollbar';
import { EncompassExportMonitorComponent } from './pages/encompass-export-monitor/encompass-export-monitor.page';
import { LoanExportMonitorComponent } from './pages/loan-export-monitor/loan-export-monitor.page';
import { ExportService } from './service/export.service';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { NgDateRangePickerModule } from '../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { exportRouting } from './export.routing';
import { ExportComponent } from './pages/export/export.page';
import { ExportDataAccess } from './export.data';
import { ExportLoanWizardComponent } from './pages/export-loan-wizard/export-loan-wizard.page';
import { SelectLoansComponent } from './helper-components/select-loans/select-loans.component';
import { SharedPipeModule } from '@mts-pipe';
import { ConfigurationsComponent } from './helper-components/configurations/configurations.component';
import { SelectDocumentsComponent } from './helper-components/select-documents/select-documents.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { LoanFilterPipe } from './pipes';
import { LosExportMonitorComponent } from './pages/los-export-monitor/los-export-monitor.page';
@NgModule({
  imports: [
    CommonModule,
    exportRouting,
    DataTablesModule,
    NgDateRangePickerModule,
    ModalModule.forRoot(),
    TooltipModule.forRoot(),
    NgBusyModule,
    SharedPipeModule,
    NgDateRangePickerModule,
    MalihuScrollbarModule,
    MonthYearPickerModule
  ],
  providers: [ExportService, ExportDataAccess, DatePipe],
  declarations: [
    LoanFilterPipe,
    ExportComponent,
    LosExportMonitorComponent,
    LoanExportMonitorComponent,
    EncompassExportMonitorComponent,
    ExportLoanWizardComponent,
    SelectLoansComponent,
    ConfigurationsComponent,
    SelectDocumentsComponent
  ]
})
export class ExportModule { }
