import { ModalDirective } from 'ngx-bootstrap/modal';
import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  OnDestroy,
} from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { DomSanitizer } from '@angular/platform-browser';
import { NotificationService } from '@mts-notification';

import { AppSettings } from '@mts-app-setting';
import { CategoryListService } from '../../service/category-list.service';
import { Subscription } from 'rxjs';
import { CategoryListData, UpdateCategorymodel, SaveCategorymodel } from '../../models/category-list.model';
import { ConfigRequestModel } from '../../models/config-request.model';
import { ApplicationConfigService } from '../../service/application-configuration.service';

@Component({
  selector: 'mts-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css'],
  providers: [CategoryListService],
})
export class CategoryListComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(DataTableDirective) categorytable: DataTableDirective;
  @ViewChild('CategoryListConfirmModal') categorymodal: ModalDirective;
  dtOptions: any;
  label: string;
  CategoryValue: string;
  isRuleErrMsgs: boolean;
  errMsgStyle: any;
  ruleErrMsgs: any;
  Items: CategoryListData = {
    Category: '',
    Active: true,
    IsMappedCheckList: '',
    OldCategory: '',
  };
  isChecked = false;
  CategoryRowDatas: CategoryListData = {
    Category: '',
    Active: '',
    IsMappedCheckList: '',
    OldCategory: '',
  };
  promise: Subscription;
  dTable: any;
  constructor(
    private _categoryservice: CategoryListService,
    private sanitizer: DomSanitizer,
    private _notificationservice: NotificationService,
    private _appconfigservice: ApplicationConfigService

  ) { }
  private subscription: Subscription[] = [];

  ngOnInit() {
    this.dtOptions = {
      aaData: [],
      iDisplayLength: 10,
      aLengthMenu: [
        [5, 10, 25, 50, -1],
        [5, 10, 25, 50, 'All'],
      ],
      aoColumns: [
        { sTitle: 'Category', mData: 'Category', sWidth: '40%' },
        {
          sTitle: 'Active/Inactive',
          mData: 'Active',
          sClass: 'text-center',
          sWidth: '10%',
        },
        {
          sTitle: 'Edit',
          mData: 'CategoryID',
          sClass: 'text-center',
          sWidth: '10%',
        },
      ],
      aoColumnDefs: [
        {
          aTargets: [1],
          mRender: function (data, type, row) {
            //
            let statusFlag = '';
            if (data === true) {
              statusFlag = 'Active';
            } else {
              statusFlag = 'Inactive';
            }
            const statusColor = {
              true: 'label-success',
              false: 'label-danger',
            };

            return (
              '<label class=\'label ' +
              statusColor[row['Active']] +
              ' label-table\'>' +
              statusFlag +
              '</label></td>'
            );
          },
        },
        {
          aTargets: [2],
          mRender: function (a, row, values) {
            if (values !== null && values !== '') {
              return '<span style=\'cursor:pointer;font-size: 15px;\' class=\'edit-config material-icons txt-info\'>border_color</span>';
            }
          },
        },
      ],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        const self = this;

        $('td .edit-config', row).unbind('click');
        $('td .edit-config', row).bind('click', () => {
          self.getRowData(row, data);
        });
        return row;
      },
    };
    this.subscription.push(
      this._categoryservice.categoryTableData$.subscribe((res: any) => {
        this.dTable.clear();
        this.dTable.rows.add(res);
        this.dTable.draw();
      })
    );
    this.subscription.push(
      this._categoryservice.isUpdated$.subscribe((result: any) => {
        this.categorymodal.hide();
        this.GetAllCategory();
      })
    );
    this.subscription.push(
      this._appconfigservice.addCategory$.subscribe((result: any) => {
        this.label = 'Add Category List';
        this.AddCategorygroup();
      })
    );
  }
  ngAfterViewInit(): void {
    this.categorytable.dtInstance.then((dtInstance: DataTables.Api) => {
      this.dTable = dtInstance;
      dtInstance.search('');
      this.GetAllCategory();
    });
  }
  changeMode() {
    this.Items.Active = !this.Items.Active;
  }

  AddCategorygroup() {

    this.Items.Active = true;
    this.Items.Category = '';
    this.CategoryRowDatas.OldCategory = '';
    this.categorymodal.show();
  }

  GetAllCategory() {
    const inputs = new ConfigRequestModel(AppSettings.SystemSchema);

    this._categoryservice.GetAllCategory(inputs);
  }
  AddEdit() {
    this.Items.Category =
      this.Items.Category.trim() === ''
        ? ''
        : this.Items.Category;
    if (this.Items.Category !== '') {
      this.findDuplicateList();
    } else {
      this._notificationservice.showWarning('Enter valid Name');
    }
    // this.SaveData();
  }

  modalHide() {
    this.categorymodal.hide();
    this.ruleErrMsgs = '';
    this.isRuleErrMsgs = false;
    this.CategoryRowDatas.Category = this.CategoryRowDatas.OldCategory;
    this.CategoryRowDatas.OldCategory = '';
  }
  SaveData() {
    const inputs = new SaveCategorymodel(AppSettings.SystemSchema, this.Items.Category, this.Items.Active);

    this._categoryservice.SaveCategoryData(inputs);
  }

  UpdateData() {
    if (this.Items.IsMappedCheckList === true) {
      // this.Items.Active === false &&
      this._notificationservice.showError(
        'This Category Already Assigned to a Checklist'
      );
      this.CategoryRowDatas.Active = true;
    } else {
      const inputs = new UpdateCategorymodel(AppSettings.SystemSchema, this.CategoryRowDatas);
      this._categoryservice.UpdateCategoryData(inputs);
    }
  }
  findDuplicateList() {
    const fullTableData = this.dTable.rows().data();
    const convertToArray = fullTableData.toArray();
    const newData = [];

    convertToArray.forEach((element) => {
      newData.push(element.Category);
    });

    for (let i = 0; newData.length > i; i++) {
      if (newData[i].toLocaleUpperCase() === this.Items.Category.toLocaleUpperCase() && this.Items.Category.toLocaleUpperCase() !== this.CategoryRowDatas.OldCategory.toLocaleUpperCase()) {
        this.isRuleErrMsgs = true;
        this.errMsgStyle = this.sanitizer.bypassSecurityTrustStyle('red');
        this.ruleErrMsgs = 'Name already Exist in List';
        break;
      } else {
        this.isRuleErrMsgs = false;
      }
    }
    if (!this.isRuleErrMsgs) {
      if (this.label === 'Add Category List') {
        this.SaveData();
      } else {
        this.UpdateData();
      }
    }
  }
  getRowData(rowIndex: Node, rowData: any): void {
    this.CategoryRowDatas = { ...rowData };
    this.Items = null;
    this.Items = this.CategoryRowDatas;
    this.CategoryRowDatas.OldCategory = this.CategoryRowDatas.Category;
    this.label = 'Edit Category Name';
    this.categorymodal.show();
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
