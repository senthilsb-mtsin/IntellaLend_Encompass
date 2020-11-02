import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit, AfterContentChecked } from '@angular/core';
import { CommonService } from 'src/app/shared/common/common.service';
import { Subscription } from 'rxjs';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { NotificationService } from '@mts-notification';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { CloneChecklistRequest } from '../../models/clone-checklist-request.model';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-clone-checklist',
    styleUrls: ['clone-checklist.page.css'],
    templateUrl: 'clone-checklist.page.html',
})
export class CloneChecklistComponent implements OnInit, OnDestroy, AfterContentChecked, AfterViewInit {
    checkListGroupMaster: any[] = [];
    checkDropValue: any = 0;
    sysEditCheckListName = '';
    dtOptions: any = {};
    viewCheckListName = '';
    viewCheckListFormula = '';
    showTable = true;
    checkListType = 'clone';
    promise: Subscription;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    constructor(
        private _commonService: CommonService,
        private _notificationService: NotificationService,
        private _addLoanTypeService: AddLoanTypeService,
        private _commonRuleBuilderService: CommonRuleBuilderService
    ) { }
    private tableAligned = false;
    private dTable: any;
    private _subscription: Subscription[] = [];

    ngOnInit() {

        this.checkListGroupMaster = this._commonService.GetSystemChecklistMaster(false);
        this.checkListType = this._addLoanTypeService.getCheckListType();
        this._subscription.push(this._commonService.SystemCheckListMaster.subscribe(
            (res: any[]) => {
                this.checkListGroupMaster = res;
            }
        ));
        this._subscription.push(this._addLoanTypeService.checkListType.subscribe((res: string) => {
            this.checkListType = res;
        }));
        this._subscription.push(this._addLoanTypeService.SysCheckListDetailTableData.subscribe(
            (res: any[]) => {
                if (isTruthy(this.dTable) && (this.checkListType !== 'edit')) {
                    this.dTable.clear();
                    this.dTable.rows.add(res);
                    this.dTable.draw();
                    this.dTable.columns.adjust();
                }
            }
        ));

        this.dtOptions = {
            data: [],
            'iDisplayLength': 10,
            'scrollY': 'calc(100vh - 520px)',
            'bPaginate': false,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Rule Type', mData: 'RuleType', bSortable: false },
                { sTitle: 'Checklist Item Name', sClass: 'text-left', mData: 'CheckListName', bSortable: false },
                { sTitle: 'Category', mData: 'Category', bVisible: true, bSortable: false },
                { sTitle: 'Last Modified By', mData: 'FirstName', bSortable: false },
                { sTitle: 'Created Date', type: 'date', mData: 'CreatedOn', sClass: 'text-center', bSortable: false },
                { sTitle: 'Status', sClass: 'text-center', mData: 'ChecklistActive', bSortable: false },
                { sTitle: 'View', mData: 'RuleID', sClass: 'text-center', bSortable: false },
                { mData: 'RuleDescription', bVisible: false }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [0],
                    'mRender': function (data, type, row) {
                        let val: any;
                        if (data === 0) {
                            val = 'Automatic';
                        } else if (data === 1) {
                            val = 'Manual';
                        } else {
                            val = '';
                        }
                        return val;
                    }
                },
                {
                    'aTargets': [4],
                    'mRender': function (date) {
                        if (date !== null && date !== '') {
                            return convertDateTime(date);
                        } else {
                            return date;
                        }
                    }
                },
                {
                    'aTargets': [5],
                    'mRender': function (data, type, row) {

                        let statusFlag = '';
                        if (data === true) {
                            statusFlag = 'Active';
                        } else {
                            statusFlag = 'Inactive';
                        }
                        const statusColor = {
                            'true': 'label-success',
                            'false': 'label-danger'
                        };

                        return '<label class=\'label ' + statusColor[row['ChecklistActive']] + ' label-table\'>' + statusFlag + '</label></td>';
                    }
                },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        if (data !== null && data !== '') {
                            return '<span style=\'cursor:pointer\' class=\'view-rule material-icons txt-info\'>pageview</span>';
                        }
                    }
                }
            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .view-rule', row).unbind('click');
                $('td .view-rule', row).bind('click', () => {
                    self.viewRowData(data);
                });
                return row;
            }
        };
    }

    viewRowData(rowData) {
        this.showTable = !this.showTable;
        this.viewCheckListFormula = rowData.RuleDescription;
        this.viewCheckListName = rowData.CheckListName;
    }

    ngAfterContentChecked() {
        if (isTruthy(this.dTable) && !this.tableAligned) {
            $('.dataTables_info').addClass('text-left');
            $('.dataTables_filter').addClass('col-md-12 p0');
            $('.dataTables_length').addClass('col-md-6 p0');
            this.tableAligned = true;
        }
    }

    ngAfterViewInit() {
        if (isTruthy(this.datatableEl)) {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
            });
        }
    }

    AssignCheckList() {
        if (this.checkDropValue !== 0 && this.checkDropValue.CheckListID !== 0) {
            if (this.checkListGroupMaster.filter(x => x.CheckListName.toUpperCase() === this.sysEditCheckListName.toUpperCase()).length > 0) {
                this._notificationService.showError('Checklist Group Already Exists !');
            } else {
                const req = new CloneChecklistRequest(this.sysEditCheckListName);
                this.promise = this._addLoanTypeService.assignChecklist(req);
            }
        } else {
            this._notificationService.showError('Select Checklist Group');
        }
    }

    SysCheckListRedundancyCheck(vals: any) {
        this._addLoanTypeService.checkDuplicateChecklistGrp(vals);
    }

    EditCheckListMaster() {
        if (this.checkDropValue !== 0 && this.checkDropValue.CheckListID !== 0) {
            this.sysEditCheckListName = this.checkDropValue.CheckListName;
            this._addLoanTypeService.setSystemChecklist(this.checkDropValue);
            this._commonRuleBuilderService.setSystemChecklist(this.checkDropValue);
            this.promise = this._addLoanTypeService.getSystemChecklistDetailTable();
        } else {
            this._notificationService.showError('Select Checklist Group');
        }
    }

    CreateClose() {
        this._addLoanTypeService.checkListBack.next('');
    }

    ngOnDestroy() {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
