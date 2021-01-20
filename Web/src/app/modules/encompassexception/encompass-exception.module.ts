import { DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { DataTablesModule } from 'angular-datatables';
import { ModalDirective, ModalModule } from 'ngx-bootstrap/modal';
import { EncompassExceptionDataAccess } from './encompass-exception.data';
import { EncompassRouting } from './encompass-exception.routing';
import { EncompassDownloadExceptionComponent } from './pages/encompass-exception.page';
import { EncompassExceptionService } from './service/encompass-exception.service';

@NgModule({
    imports: [
        EncompassRouting,
        DataTablesModule,
        NgDateRangePickerModule,
        NgBusyModule,
        ModalModule.forRoot()],
    providers: [
        EncompassExceptionService,
        EncompassExceptionDataAccess,
        DatePipe
    ],

    declarations: [EncompassDownloadExceptionComponent, ],
    exports: [EncompassDownloadExceptionComponent]
})

export class EncompassDownloadModule {

}
