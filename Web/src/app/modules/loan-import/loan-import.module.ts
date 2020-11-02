import { SharedPipeModule } from './../../shared/pipes/shared-pipe.module';
import { CheckFileExtension } from './pipes/fileExtension.pipe';
import { BoxLoanImportComponent } from './pages/box-loan-import/box-loan-import.page';
import { LoanImportDataAccess } from './loan-import.data';
import { LoanImportService } from './services/loan-import.service';
import { NgDateRangePickerModule } from './../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { LoanImportComponent } from './pages/loan-import/loan-import.page';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { LoanImportMonitorComponent } from './pages/loan-import-monitor/loan-import-monitor.page';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { DataTablesModule } from 'angular-datatables';
import { importRouting } from './loan-import.routing';
import { FileUploadModule } from 'ng2-file-upload';
import { AdHocLoanImportComponent } from './pages/ad-hoc-loan-import/ad-hoc-loan-import.page';
import { OrderByPipe } from '@mts-pipe';
import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';
@NgModule({

  imports: [
    CommonModule,
    FormsModule,
    NgBusyModule,
    MyDatePickerModule,
    ModalModule,
    MonthYearPickerModule,
    NgDateRangePickerModule,
    DataTablesModule,
    SharedPipeModule,
    FileUploadModule,
    importRouting
  ],
  declarations: [LoanImportComponent, LoanImportMonitorComponent, BoxLoanImportComponent, AdHocLoanImportComponent],
  providers: [LoanImportService, LoanImportDataAccess, DatePipe, CheckFileExtension, OrderByPipe]
})
export class LoanImportModule { }
