import { ParkingSpotRowData } from './../../models/parking-spot-rowdata.model';
import { DataTableDirective } from 'angular-datatables';
import {
  Component,
  OnInit,
  AfterViewInit,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { EncompassPSpotService } from '../../service/encompass-parkingspot.service';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-encompass-parkingspot',
  templateUrl: './encompass-parkingspot.page.html',
  styleUrls: ['./encompass-parkingspot.page.css'],
})
export class EncompassParkingspotComponent
  implements OnInit, AfterViewInit, OnDestroy {
  dtparkingspot: any = {};

  rowSelected = true;
  promise: Subscription;
  @ViewChild(DataTableDirective) dt: DataTableDirective;
  _EncompassParrkingSpot = [];
  dTable: any;
  selectedRow: any;

  constructor(
    private _parkingSpotService: EncompassPSpotService,
    private route: ActivatedRoute,
    private router: Router
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this._parkingSpotService.EmcompassPSpotTabledata.subscribe((res: any) => {
        this.dTable.clear();
        this.dTable.rows.add(res);
        this.dTable.draw();
      })
    );
    this.dtparkingspot = {
      aaData: [],
      select: {
        style: 'single',
        info: false,
        selector: 'td:not(:last-child)',
      },
      order: [[1, 'asc']],
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'ID', mData: 'ID', bVisible: false },
        { sTitle: 'EFolder Name', mData: 'ParkingSpotName' },
        { sTitle: 'Active/Inactive', mData: 'Active' },
      ],
      aoColumnDefs: [
        {
          aTargets: [2],
          mRender: function (data, type, row) {
            if (data === true) {
              return (
                '<label class=\'label label-success ' +
                ' label-table\'' +
                '>' +
                ' Active ' +
                '</label></td>'
              );
            } else {
              return (
                '<label class=\'label label-danger ' +
                ' label-table\'' +
                '>' +
                'InActive' +
                '</label></td>'
              );
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      },
    };
  }
  getRowData(row: Node, data: any) {
    this.rowSelected = $(row).hasClass('selected');
    this.selectedRow = data;
  }

  ngAfterViewInit() {
    this.dt.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      if (typeof this.dTable !== 'undefined') {
        this.GetParkingSpotDetails();
      }
    });
  }
  GetParkingSpotDetails() {
    const inputData: any = { TableSchema: AppSettings.TenantSchema };
    this.promise = this._parkingSpotService.GetParkingSpotDetails(inputData);
  }
  ShowRoleModal(modalType: number) {
    let inputData: ParkingSpotRowData;
    if (modalType !== 0) {
      inputData = {
        ParkingSpotName: this.selectedRow['ParkingSpotName'],
        Active: this.selectedRow['Active'],
        ParkingSpotID: this.selectedRow['ID'],
      };
    }
    if (modalType === 0) {
      this.router.navigate(['addparkingspot'], { relativeTo: this.route });
    } else if (modalType === 1) {
      this.router.navigate(['editparkingspot'], { relativeTo: this.route });
      setTimeout(() => {
        this._parkingSpotService.SetTableRowData(inputData);
      }, 20);
    } else if (modalType === 2) {
      this.router.navigate(['viewparkingspot'], { relativeTo: this.route });
      setTimeout(() => {
        this._parkingSpotService.SetTableRowData(inputData);
      }, 200);
    }
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
