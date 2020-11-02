
import { SharedPipeModule } from '@mts-pipe';
import { SelectModule } from '@mts-select2';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Angular Imports
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { MonthYearPickerModule } from '@mts-month-year-picker/month-year-picker.module';
import { LoanPopOutComponent } from './loan-popout.page';
import { LoanSharedModule } from '../../loan-shared.module';
import { RouterModule, Routes } from '@angular/router';

const loanPopRoutes: Routes = [
    {
        path: '',
        component: LoanPopOutComponent
    }
];

export const loanPopRouting = RouterModule.forChild(loanPopRoutes);

@NgModule({
    imports: [
        SelectModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DataTablesModule,
        ModalModule.forRoot(),
        MyDatePickerModule,
        NgDateRangePickerModule,
        NgBusyModule,
        SharedPipeModule,
        MonthYearPickerModule,
        LoanSharedModule,
        loanPopRouting
    ],
    providers: [],
    declarations: [
        LoanPopOutComponent
    ]
})
export class LoanPopOutModule { }
