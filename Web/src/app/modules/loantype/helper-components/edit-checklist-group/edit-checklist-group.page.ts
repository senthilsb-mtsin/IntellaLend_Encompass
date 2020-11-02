import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit, AfterContentChecked } from '@angular/core';
import { CommonService } from 'src/app/shared/common/common.service';
import { NotificationService } from '@mts-notification';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { Subscription } from 'rxjs';
import { ChecklistMasterModel, AddChecklistGroupModel } from '../../models/add-checklist-group.model';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-edit-checklist-group',
    styleUrls: ['edit-checklist-group.page.css'],
    templateUrl: 'edit-checklist-group.page.html',
})
export class EditChecklistGroupComponent implements OnInit, AfterViewInit, AfterContentChecked, OnDestroy {

    @ViewChild('cloneMsgModal') cloneMsgModal: ModalDirective;
    @ViewChild('DeleteMsgModal') deleteMsgModal: ModalDirective;

    checkListGroupMaster: any[] = [];
    dtOptions: any = {};
    viewCheckListName = '';
    viewCheckListFormula = '';
    sysCheckListName = '';
    showTable = true;
    isRowNotSelected = true;
    isRowSelectrDeselect = true;
    enableNewChecklistItem = false;
    checkListType = 'createEdit';
    cloneSysCheckListItem = '';
    DeleteChecklistItem = '';
    enableRuleBuilder = false;
    promise: Subscription;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;

    constructor(
        private _commonService: CommonService,
        private _notificationService: NotificationService,
        private _addLoanTypeService: AddLoanTypeService,
        private _commonRuleBuilderService: CommonRuleBuilderService) {
    }
    private tableAligned = false;
    private dTable: any;
    private _subscription: Subscription[] = [];
    private _rowData: any[] = [];

    ngOnInit() {
        this.checkListType = this._addLoanTypeService.getCheckListType();
        this.sysCheckListName = this._addLoanTypeService.getChecklist().CheckListName;
        this.checkListGroupMaster = this._commonService.GetSystemChecklistMaster(false);

        this._subscription.push(this._commonService.SystemCheckListMaster.subscribe(
            (res: any[]) => {
                this.checkListGroupMaster = res;
            }
        ));

        this._subscription.push(this._addLoanTypeService.isRowNotSelected.subscribe((res: boolean) => {
            this.isRowNotSelected = res;
        }));

        this._subscription.push(this._commonRuleBuilderService.RuleAdded.subscribe((res: boolean) => {
            if (res) {
                this.promise = this._addLoanTypeService.getSystemChecklistDetailTable();
            }
        }));

        this._subscription.push(this._addLoanTypeService.HideCloneMsgModal.subscribe((res: boolean) => {
            if (res) {
                this.cloneMsgModal.hide();
            } else {
                this.cloneMsgModal.show();
            }
        }));

        this._subscription.push(this._commonRuleBuilderService.EnableRuleBuilder.subscribe((res: boolean) => {
            this.enableRuleBuilder = res;
        }));

        this._subscription.push(this._addLoanTypeService.EnableEditChecklistFeatures.subscribe(
            (res: boolean) => {
                this.enableNewChecklistItem = res;
            }
        ));

        this.dtOptions = {
            data: [],
            displayLength: 2,
            dom: 'Blrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-sm btn-info waves-effect waves-light',
                    text: '<i class="fa fa-file-excel-o"></i> Download',
                    filename: 'Check List',
                    exportOptions: {
                        columns: [17, 3, 4, 5, 8, 9, 15]
                    },
                    title: 'Check List Item'
                }
            ],
            'iDisplayLength': 10,
            rowReorder: {
                dataSrc: 'SequenceID',
                selector: 'td:nth-child(2)'
            },
            'bPaginate': false,
            'scrollY': 'calc(100vh - 500px)',
            'select': {
                style: 'multi',
                info: false,
                selector: 'td:first-child'
            },
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { mData: 'CheckListDetailID', sClass: 'select-checkbox', bVisible: true },
                { mData: 'RuleID', bVisible: false },
                { sTitle: 'Rule Type', mData: 'RuleType', bSortable: false, sClass: 'text-left' },
                { sTitle: 'Checklist Item Name', sClass: 'text-left', mData: 'CheckListName' },
                { sTitle: 'Category', mData: 'Category', class: 'text-left', bVisible: true },
                { sTitle: 'Last Modified By', mData: 'FirstName', sClass: 'text-left' },
                { sTitle: 'FirstName', mData: 'FirstName', bVisible: false },
                { sTitle: 'LastName', mData: 'LastName', bVisible: false },
                { sTitle: 'Created Date', type: 'date', mData: 'CreatedOn', sClass: 'text-center' },
                { sTitle: 'RuleDescription', mData: 'RuleDescription', bVisible: false },
                { sTitle: 'Status', sClass: 'text-center', mData: 'ChecklistActive' },
                { sTitle: 'RuleJson', mData: 'RuleJson', bVisible: false },
                { sTitle: 'Edit', mData: 'RuleID', sClass: 'text-center', bSortable: false },
                { sTitle: 'View', mData: 'RuleID', sClass: 'text-center', bSortable: false },
                { sTitle: 'ID', mData: 'SequenceID', bVisible: false },
                { sTitle: 'Loan Type', mData: 'LoanType', bVisible: false },
                { sTitle: 'DocVersion', mData: 'DocVersion', bVisible: false },
                { sTitle: 'Rule Type', mData: 'RuleType', bVisible: false },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [8],
                    'mRender': function (date) {
                        if (date !== null && date !== '') {
                            return convertDateTime(date);
                        } else {
                            return date;
                        }
                    }
                }, {
                    'aTargets': [5],
                    'mRender': function (data, type, row) {
                        return row.LastName + ' ' + row.FirstName;
                    }
                },
                {
                    'aTargets': [10],
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
                    'aTargets': [0],
                    'orderable': false,
                    'mRender': function (date) {
                        return '';
                    }
                },
                {
                    'aTargets': [13],
                    'mRender': function (data, type, row) {
                        if (data !== null && data !== '') {
                            return '<span style=\'cursor:pointer\' class=\'view-rule material-icons txt-info\'>pageview</span>';
                        }
                    }
                },
                {
                    'aTargets': [12],
                    'mRender': function (data, type, row) {
                        if (data !== null && data !== '') {
                            return '<span style="cursor:pointer" class="edit-rule material-icons txt-warm">rate_review</span>';
                        }
                    }
                },
                {
                    'aTargets': [16],
                    'mRender': function (data) {
                        let val: any;
                        if (data === '1') {
                            val = 'All';
                        } else if (data === '2') {
                            val = 'Any';
                        } else if (data === '3') {
                            val = 'Final';
                        } else if (data === '4') {
                            val = 'Initial';
                        } else {
                            val = '';
                        }
                        return val;
                    }
                },
                {
                    'aTargets': [2, 17],
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

                }
            ],
            rowCallback: (row: Node, data: any[] | Object, index: number) => {
                const self = this;
                $('td .edit-rule', row).unbind('click');
                $('td .edit-rule', row).bind('click', () => {
                    self.editSysCheckListData(data);
                });

                $('td .view-rule', row).unbind('click');
                $('td .view-rule', row).bind('click', () => {
                    self.viewRowData(data);
                });

                return row;
            }
        };

        this._subscription.push(this._addLoanTypeService.SysCheckListDetailTableData.subscribe(
            (res: any[]) => {
                if (isTruthy(this.dTable)) {
                    this.dTable.clear();
                    this.dTable.rows.add(res);
                    this.dTable.draw();
                    setTimeout(() => { this.dTable.columns.adjust(); }, 100);
                }
            }
        ));
    }

    viewRowData(rowData) {
        this.showTable = !this.showTable;
        this.viewCheckListFormula = rowData.RuleDescription;
        this.viewCheckListName = rowData.CheckListName;
    }

    SaveSysCheckList() {
        if (isTruthy(this.sysCheckListName)) {
            if (this._addLoanTypeService.checkDuplicateChecklist(this.sysCheckListName)) {
                const req = new AddChecklistGroupModel(new ChecklistMasterModel(this.sysCheckListName, true, false));
                this._addLoanTypeService.SaveSysCheckList(req);
            }
        } else {
            this._notificationService.showError('Enter Checklist Group Name');
        }
    }

    CloseAddSysCheckListModal() {
        this._addLoanTypeService.checkListBack.next('');
    }

    CheckListNameDuplicationCheck(vals: any) {
        this._addLoanTypeService.checkDuplicateChecklist(vals);
    }

    selectRowData() {
        let isDisabled = true;
        if (this.isRowNotSelected && this.dTable.rows().data().length > 0) {
            this.dTable.rows().select();
            this._rowData = this.dTable.rows().data();
            isDisabled = false;
        } else {
            this.dTable.rows().deselect();
            isDisabled = true;
        }
        this.isRowSelectrDeselect = isDisabled;
        this._addLoanTypeService.isRowNotSelected.next(isDisabled);
    }

    CloneSysCheckListItemCreation() {
        const tableRowSelected = this.dTable.rows('.selected').data();
        if (tableRowSelected.length > 1) {
            this._notificationService.showError('Select only one row to clone !');
        } else {
            this.cloneSysCheckListItem = tableRowSelected[0].CheckListName + '_clone';
            this._addLoanTypeService.HideCloneMsgModal.next(false);
        }
    }

    CloneSysConfirmMsg() {
        if (this._commonRuleBuilderService.validateCheckListItemName(this.cloneSysCheckListItem)) {
            const id = [];
            id.push(this.dTable.rows('.selected').data()[0].CheckListDetailID);
            const req = { CheckListDetailsID: id, ModifiedCheckListName: this.cloneSysCheckListItem, LoanTypeID: 0 };
            this._addLoanTypeService.CloneChecklistItem(req);
        }
    }

    sysChecklistItemChange() {
        this._commonRuleBuilderService.validateCheckListItemName(this.cloneSysCheckListItem);
    }

    DeleteConfirm() {
        const rows = this.dTable.rows('.selected').data();
        const id = [];
        for (let i = 0; i < rows.length; i++) {
            id.push(rows[i].CheckListDetailID);
        }
        const req = { CheckListDetailsID: id, LoanTypeID: 0 };
        this._addLoanTypeService.DeleteChecklistItems(req);
        this.deleteMsgModal.hide();
    }

    editSysCheckListData(rowData) {
        this._commonRuleBuilderService.setRuleBuilderSteps({ Step1: true, Step2: false, Step3: false });
        this._commonRuleBuilderService.EnableRuleBuilder.next(true);
        this._addLoanTypeService.setEditChecklistItem(rowData);
        this._commonRuleBuilderService.setEditChecklistItem(rowData);
    }

    NewCheckListItemCreation() {
        this._commonRuleBuilderService.clearSavedChecklistItem();
        this._addLoanTypeService.clearSavedChecklistItem();
        this._commonRuleBuilderService.setRuleBuilderSteps({ Step1: true, Step2: false, Step3: false });
        this._commonRuleBuilderService.EnableRuleBuilder.next(true);
    }

    ngAfterContentChecked() {
        if (isTruthy(this.dTable) && !this.tableAligned) {
            $('.dataTables_info').addClass('col-md-6 p0 text-left');
            $('.dataTables_paginate').addClass('col-md-6 p0');
            $('.dataTables_filter').addClass('col-md-12 p0');
            $('.dataTables_length').addClass('col-md-6 p0');
            $('.dt-buttons > button').appendTo('#downloadButton');
            this.tableAligned = true;
        }
    }

    ngAfterViewInit() {
        if (isTruthy(this.datatableEl)) {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
                dtInstance.on('select', (s) => {
                    this.isRowNotSelected = !(this.dTable.rows('.selected').data().length > 0);
                });
                dtInstance.on('deselect', (s) => {
                    this.isRowNotSelected = !(this.dTable.rows('.selected').data().length > 0);
                });

                const self = this;
                $('.wrapperTable thead tr th').each(function (i) {
                    const title = $(this).text();
                    const firstIndex = 1;

                    if (i >= firstIndex) {
                        if (title !== 'Edit' && title !== 'View' && title !== 'Status') {
                            if (title === 'Checklist Item Name') {
                                $(this).html('<p style="margin:0;">' + title + '</p><input type="text" style="width:100%" class="form-control input-sm"/>');
                            } else {
                                $(this).html('<p style="margin:0;">' + title + '</p><input type="text" class="form-control input-sm"/>');
                            }

                            $('input', this).on('keyup change', function () {
                                const val = this['value'];
                                let thIndex = i;
                                let objIndex = 0;
                                self.dTable.columns().indexes('visible').each(function (element) {
                                    if (element !== null && element === i) {
                                        thIndex = objIndex;
                                    }
                                    objIndex++;
                                });
                                if (self.dTable.column(thIndex).search() !== val) {
                                    self.dTable
                                        .column(thIndex)
                                        .search(val)
                                        .draw();
                                }
                            });
                        }

                        if (title === 'Rule Type') {
                            $(this).html('<p style="margin:0;">' + title + '</p><select class="form-control input-sm"><option value="">All</option><option value="0">Automatic</option><option value="1">Manual</option></select>');
                            $('select').on('change', function () {
                                const val = this['value'];
                                if (self.dTable.column(17).search() !== val && val !== '') {
                                    self.dTable.column(17).search(parseInt(val, 10)).draw();
                                } else if (val === '') {
                                    self.dTable.column(17).search('').draw();
                                }
                            });
                        }
                    }
                });
                self.dTable.columns.adjust();

                if (this.checkListType === 'edit') {
                    this._addLoanTypeService.EnableEditChecklistFeatures.next(true);
                    this.promise = this._addLoanTypeService.getSystemChecklistDetailTable();
                }
            });
        }
    }

    ngOnDestroy() {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
