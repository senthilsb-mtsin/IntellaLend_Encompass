import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DataTableDirective } from 'angular-datatables';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { FannieMaeFields } from '../../loansearch/models/loan-search-table.model';
import { FannieMaeFieldsModule } from '../fanniemaeFields.module';
import { FannieMaeFieldsService } from '../services/fanniemaeFields.service';

@Component({
  selector: 'mts-fanniemae-fields',
  templateUrl: 'fanniemaeFields.page.html',
  styleUrls: ['fanniemaeFields.page.css'],

})
export class FannieMaeFieldsComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
   FannieMaeFieldsdtOptions: any = {};
  FannieMaeFieldsTable: any;
  _fields: any = [];
  constructor(private _fannieMaeFieldService: FannieMaeFieldsService) {

  }
  private subscription: Subscription[] = [];

  ngOnInit() {
    this.FannieMaeFieldsdtOptions = {
      aaData: this._fannieMaeFieldService._fannieMaeFields,
      'select': {
        style: 'single',
        info: false,
      },
      'scrollY': 'calc(100vh - 440px)',
      'iDisplayLength': 10,
      'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
      aoColumns: [
        { sTitle: 'Field ID', mData: 'FieldID' },
        { sTitle: 'Field Value', mData: 'FieldValue' },
      ],
      aoColumnDefs: [

      ],
      rowCallback: (row: Node, data: object, index: number) => {

        const self = this;
        return row;
      }
    };
  }
  ngAfterViewInit() {
    this.subscription.push(this._fannieMaeFieldService._fannieMaeFields$.subscribe((res: FannieMaeFields[]) => {
      this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
        this.FannieMaeFieldsTable = dtInstance;
        this.FannieMaeFieldsTable.clear();
        this.FannieMaeFieldsTable.search('');
        this.FannieMaeFieldsTable.rows.add(res);
        setTimeout(() => {
          this.FannieMaeFieldsTable.columns.adjust().draw();
      }, 300);
        this._fannieMaeFieldService.showModal$.next(true);
      });
    }));
  }
  ngOnDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }
}
