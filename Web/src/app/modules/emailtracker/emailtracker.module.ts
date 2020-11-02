import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Emailrouting } from './emailtracker.routing';
import { DataTablesModule } from 'angular-datatables';
import { MyDatePickerModule } from '@mts-date-picker/mydatepicker.module';
import { NgDateRangePickerModule } from '@mts-daterangepicker/ng-daterangepicker.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { EmailDataAccess } from './emailtracker.data';
import { EmailtrackerComponent } from './pages/emailtracker.page';
import { EmailTrackerService } from './service/emailtracker.service';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        Emailrouting,
        DataTablesModule,
        MyDatePickerModule,
        NgDateRangePickerModule,
        ModalModule.forRoot() ],

    providers: [
        DatePipe,
        EmailTrackerService,
        EmailDataAccess
    ],

    declarations: [EmailtrackerComponent],
    exports: [EmailtrackerComponent]
})
export class EmailTrackerModule {

}
