
import { ReportMasterService } from './../../service/report-master.service';
import {
  Component,
  OnInit,
  ViewChild,
  OnDestroy,
  AfterViewInit,
} from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { ConfigRequestModel } from '../../models/config-request.model';
import { ReportMasterRequestModel } from '../../models/report-master-request.model';
import { ApplicationConfigService } from '../../service/application-configuration.service';

@Component({
  selector: 'mts-report-master',
  templateUrl: './report-master.component.html',
  styleUrls: ['./report-master.component.css'],
  providers: [ReportMasterService],
})
export class ReportMasterComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(DataTableDirective) reportmastertable: DataTableDirective;
  @ViewChild('ReportMasterModal') ReportMasterModal: ModalDirective;
  @ViewChild('DeleteReportMasterModal') DeleteReportMasterModal: ModalDirective;
  reportData: any = [];
  masterInfo: any = [];
  reportMaster: any = [];

  dtOptions: any = {};
  docslist: any = [];
  docName: any = 0;
  serName: string;
  ReportConfig: any = [];
  errMsgStyle: any;
  ruleErrMsgs: any;
  isRuleErrMsgs = false;

  ReportmasterRowData: any = {
    DocumentName: '',
    ReportID: '',
  };
  constructor(
    private _reportservice: ReportMasterService,
    private sanitizer: DomSanitizer,
    private _appconfigservice: ApplicationConfigService
  ) { }
  private subscription: Subscription[] = [];
  private dTable: any = {};
  ngOnInit(): void {
    this.dtOptions = {
      aaData: [],
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Document Name', mData: 'DocumentName', sWidth: '40%' },
        {
          sTitle: 'ReportID',
          mData: 'ReportID',
          sClass: 'text-center',
          sWidth: '10%',
          bVisible: false,
        },
        {
          sTitle: 'Delete',
          mData: 'ReportID',
          sClass: 'text-center',
          sWidth: '10%',
        },
      ],
      aoColumnDefs: [
        {
          aTargets: [2],
          mRender: function (a, row, values) {
            if (values) {
              return '<span style=\'cursor:pointer\' class=\'delete-user material-icons txt-red\'>delete_forever</span>';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td .delete-user', row).unbind('click');
        $('td .delete-user', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      },
    };
    this.subscription.push(
      this._reportservice.reportMasterdata$.subscribe((Result: any) => {
        if (Result !== null) {
          this.masterInfo = {};
          this.masterInfo = Result;
          this.masterInfo.reportMaster.forEach((element) => {
            this.reportMaster.push({
              ReportMasterID: element.ReportMasterID,
              ReportName: element.ReportName,
              ReviewTypeTD: element.ReviewTypeTD,
              ReviewTypeName: element.ReviewTypeName,
            });
            this.serName = element.ReviewTypeName;
          });
          Result.reportConfig.forEach((element) => {
            this.ReportConfig.push(element);
          });
          this.dTable.clear();
          this.dTable.rows.add(Result.reportConfig);
          this.dTable.draw();
        }
      })
    );
    this.subscription.push(
      this._reportservice.getDocumentlist$.subscribe((Result: any) => {
        if (Result !== null) {
          Result.forEach((element) => {
            this.docslist.push(element.Name);
          });
          this.docName = 0;
          this.ReportMasterModal.show();
        }
      })
    );
    this.subscription.push(
      this._reportservice.saveReportConfig$.subscribe((Result: any) => {
        this.GetReportMasterData();
        this.ReportMasterModal.hide();
      })
    );
    this.subscription.push(
      this._reportservice.deleteReportConfig$.subscribe((Result: any) => {
        this.GetReportMasterData();
        this.DeleteReportMasterModal.hide();
      })
    );
    this.subscription.push(
      this._appconfigservice.addReport$.subscribe((Result: any) => {
        this.GetDocsList();
      })
    );
  }
  ngAfterViewInit(): void {
    this.reportmastertable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      this.GetReportMasterData();
    });
  }
  GetReportMasterData() {
    const inputs = new ConfigRequestModel(AppSettings.SystemSchema);

    this._reportservice.GetReportMasterData(inputs);
  }
  GetDocsList() {
    const inputData = new ConfigRequestModel(AppSettings.SystemSchema);

    this._reportservice.GetDocsList(inputData);
  }
  SaveEditDocs() {
    const find = this.ReportConfig.find((x) => x.DocumentName === this.docName);
    if (find === null || find === undefined) {
      this.isRuleErrMsgs = false;
      const inputs = new ReportMasterRequestModel(
        AppSettings.SystemSchema,
        this.docName,
        this.reportMaster.ReportMasterID,
        this.serName
      );

      this._reportservice.SaveEditDocs(inputs);
    } else {
      this.errMsgStyle = this.sanitizer.bypassSecurityTrustStyle('red');
      this.isRuleErrMsgs = true;
      this.ruleErrMsgs = 'Document Name already Exist in List';
    }
  }
  DeleteReportMaster() {
    const inputs = new ReportMasterRequestModel(
      AppSettings.SystemSchema,
      this.ReportmasterRowData.DocumentName,
      this.ReportmasterRowData.ReportID
    );

    this._reportservice.DeleteReportMaster(inputs);
  }
  getRowData(rowIndex: Node, rowData: any): void {
    this.ReportmasterRowData = rowData;
    this.DeleteReportMasterModal.show();
  }
  modalHide() {
    this.ReportMasterModal.hide();
    this.ruleErrMsgs = '';
    this.isRuleErrMsgs = false;
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
