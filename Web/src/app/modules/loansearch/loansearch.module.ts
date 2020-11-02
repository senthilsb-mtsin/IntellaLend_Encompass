
import { SharedPipeModule, EmailCheckPipe, OrderByPipe } from '@mts-pipe';
import { SelectModule } from '@mts-select2';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Angular Imports
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { LoanSearchDataAccess } from './loansearch.data';
import { LoanSearchService } from './service/loansearch.service';
import { LoanSearchComponent } from './pages/loansearch.page';
import { Routes, RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { LoanService } from '../loan/services/loan.service';
import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';

const loantypeRoutes: Routes = [
  {
    path: '',
    component: LoanSearchComponent,
  },
];

// This Module's Components
@NgModule({
  imports: [
    SelectModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forChild(loantypeRoutes),
    DataTablesModule,
    ModalModule.forRoot(),
    MyDatePickerModule,
    NgDateRangePickerModule,
    NgBusyModule,
    SharedPipeModule,
    MonthYearPickerModule
  ],
  providers: [LoanSearchService, LoanSearchDataAccess, LoanService, DatePipe, OrderByPipe, EmailCheckPipe],
  declarations: [LoanSearchComponent],
  exports: [LoanSearchComponent],
})
export class LoanSearchModule { }
