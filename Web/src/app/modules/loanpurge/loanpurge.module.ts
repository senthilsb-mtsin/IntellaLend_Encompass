import { LoanPurgeService } from './service/loanpurge.service';

import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { ExpiredloansComponent } from './pages/expiredloans/expiredloans.page';
import { PurgeloanComponent } from './pages/purgeloan/purgeloan.page';
import { DataTablesModule } from 'angular-datatables';
import { NgBusyModule } from '@mts-busy/ng-busy.module';
import { PurgeDataAccess } from './loanpurge.data';
import { purgeRouting } from './loanpurge.routing';
import { PurgeMonitorComponent } from './pages/purgemonitor/purgemonitor.page';
import { NgDateRangePickerModule } from '../../shared/custom-plugins/ng-daterangepicker-master/ng-daterangepicker.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PurgemonitordetailComponent } from './pages/purgemonitordetail/purgemonitordetail.page';
import { OrderByPipe } from '@mts-pipe';
@NgModule({

  imports: [
    CommonModule,
    purgeRouting,
    DataTablesModule,
    NgDateRangePickerModule
    , ModalModule.forRoot()
    , NgBusyModule
  ],
  providers: [PurgeDataAccess, LoanPurgeService, DatePipe, OrderByPipe],
  declarations: [ExpiredloansComponent, PurgeloanComponent, PurgeMonitorComponent, PurgemonitordetailComponent]
})
export class LoanpurgeModule { }
