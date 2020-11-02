import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { StatusConstant } from '@mts-status-constant';
import { LoanPurgeService } from '../../service/loanpurge.service';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-purgemonitordetail',
  templateUrl: './purgemonitordetail.page.html',
  styleUrls: ['./purgemonitordetail.page.css']
})
export class PurgemonitordetailComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild(DataTableDirective) dt: DataTableDirective;
  purgeMonitorBatchDetailsDTOptions: any = {};
  dTable: any;
  constructor(private _purgeservice: LoanPurgeService) { }
  private subscription: Subscription[] = [];

  ngOnInit(): void {

    this.purgeMonitorBatchDetailsDTOptions = {
      select: false,
      iDisplayLength: 10,
      aaData: [],
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Loan Number', mData: 'LoanNumber', sClass: 'text-center' },
        { sTitle: 'Loan Type', mData: 'LoanType' },
        { sTitle: 'Cutomer Name', mData: 'CustomerName' },
        { sTitle: 'Borrower Name', mData: 'BorrowerName' },
        { sTitle: 'Status', mData: 'Status', sClass: 'text-center' },
        { sTitle: 'Error Message', mData: 'ErrorMSG' },
      ],
      aoColumnDefs: [
        {
          aTargets: [4],
          orderable: false,
          mRender: function (data) {
            return (
              '<label class=\'label ' +
              StatusConstant.STATUS_COLOR[data] +
              ' label-table\'>' +
              StatusConstant.STATUS_DESCRIPTION[data] +
              '</label>'
            );
          },
        },
        {
          aTargets: [5],
          orderable: false,
          mRender: function (data) {
            if (data === '' || data === undefined || data === null) {
              return (data = '');
            } else {
              return data;
            }
          },
        },
      ],
    };
  }
  ngAfterViewInit(): void {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.subscription.push(
        this._purgeservice.purgeMonitorBatchDetailsDTables.subscribe((res) => {
          this.dTable.clear();
          this.dTable.rows.add(res);
          this.dTable.draw();
        })
      );
    });
  }

  BacktoPurgeMonitor() {
    this._purgeservice.BacktoPurgeMonitor();
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => { element.unsubscribe(); });

  }
}
