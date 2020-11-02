import { SelectModule } from '@mts-select2';

// Angular Imports
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { WorkQueueDataAccess } from './workqueue.data';
import { WorkQueueService } from './service/workqueue.service';
import { WorkQueueComponent } from './pages/workqueue.page';
import { Routes, RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgBusyModule } from '@mts-busy/ng-busy.module';

const loantypeRoutes: Routes = [
  {
    path: '',
    component: WorkQueueComponent,
  },
];

@NgModule({
  imports: [
    SelectModule,
    CommonModule,
    RouterModule.forChild(loantypeRoutes),
    DataTablesModule,
    ModalModule.forRoot(),
    NgBusyModule
  ],
  providers: [WorkQueueService, WorkQueueDataAccess, DatePipe],
  declarations: [WorkQueueComponent],
  exports: [WorkQueueComponent],
})
export class WorkQueueModule { }
