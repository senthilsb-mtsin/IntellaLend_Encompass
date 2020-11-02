import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, AfterContentChecked } from '@angular/core';
import { Subscription } from 'rxjs';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { DataTableDirective } from 'angular-datatables';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { NotificationService } from '@mts-notification';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { LoanDataAccess } from 'src/app/modules/loantype/loantype.data';
import { JsonPipe } from '@angular/common';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { AppSettings } from '@mts-app-setting';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RemoveDuplicateValues } from 'src/app/modules/loantype/pipes/RemoveDuplicateValue.pipe';

@Component({
    selector: 'mts-customer-checklist',
    styleUrls: ['customer-checklist.page.css'],
    templateUrl: 'customer-checklist.page.html',
    providers: [RuleBuilderService, LoanDataAccess, JsonPipe, AddLoanTypeService, RemoveDuplicateValues]
})
export class CustomerChecklistComponent implements OnInit, OnDestroy, AfterViewInit, AfterContentChecked {

    @ViewChild('cloneMsgModal') cloneMsgModal: ModalDirective;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    promise: Subscription;
    isRowNotSelected = true;
    dtOptions: any = {};
    isRowSelectrDeselect = true;
    CloneCheckListItemName = '';
    showTable = true;
    viewCheckListFormula = '';
    viewCheckListName = '';
    enableRuleBuilder = false;
    ChecklistMapped = true;

    constructor(
        private _upsertCustomerService: UpsertCustomerService,
        private _notificationService: NotificationService,
        private _commonRuleBuilderService: CommonRuleBuilderService
    ) { }

    private _subscriptions: Subscription[] = [];
    private dTable: any;
    private tableAligned = false;

    ngOnInit() {

        this._commonRuleBuilderService.setTenantSchema(AppSettings.TenantSchema);

        this._subscriptions.push(this._upsertCustomerService.CurrentCheckList$.subscribe((res: { CheckListID: number, CheckListName: string, SyncEnabled: boolean }) => {
            if (res.CheckListID > 0) {
                this.promise = this._upsertCustomerService.GetCheckListDetail();
            } else { this.ChecklistMapped = false; }
        }));

        this._subscriptions.push(this._upsertCustomerService.CheckListDetailTable$.subscribe((res: any[]) => {
            if (isTruthy(this.dTable)) {
                this.dTable.clear();
                this.dTable.rows.add(res);
                this.dTable.draw();
                setTimeout(() => { this.dTable.columns.adjust(); }, 100);
            }
        }));

        this._subscriptions.push(this._upsertCustomerService.isRowNotSelected$.subscribe((res: boolean) => {
            this.isRowNotSelected = res;
        }));

        this._subscriptions.push(this._commonRuleBuilderService.EnableRuleBuilder.subscribe((res: boolean) => {
            this.enableRuleBuilder = res;
        }));

        this._subscriptions.push(this._commonRuleBuilderService.RuleAdded.subscribe((res: boolean) => {
            if (res) {
                this.promise = this._upsertCustomerService.GetCheckListDetail();
            }
        }));

        this._subscriptions.push(this._upsertCustomerService.CloneModal$.subscribe((res: boolean) => {
            res ? this.cloneMsgModal.show() : this.cloneMsgModal.hide();
        }));

        this.dtOptions = {
            data: [],
            dom: 'Blrtip',
            buttons: [
                {
                    extend: 'excel',
                    className: 'btn btn-sm btn-info waves-effect waves-light',
                    text: '<i class="fa fa-file-excel-o"></i> Download',
                    filename: 'Check List',
                    exportOptions: {
                        columns: [18, 14, 15, 16, 3, 4, 8, 6]
                    },
                    title: 'Check List Item'
                }
            ],
            rowReorder: {
                dataSrc: 'SequenceID',
                selector: 'td:nth-child(2)'
            },

            'bPaginate': false,
            'scrollY': 'calc(100vh - 440px)',
            'iDisplayLength': 10,
            'select': {
                style: 'multi',
                info: false,
                selector: 'td:first-child'
            },
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { mData: 'CheckListDetailID', sClass: 'select-checkbox', bVisible: true, bSortable: false },
                { mData: 'RuleID', bVisible: false },
                { sTitle: 'Rule Type', mData: 'RuleType', bSortable: false, sClass: 'text-left' },
                { sTitle: 'Checklist Item Name', mData: 'CheckListName', bSortable: false, sClass: 'text-left' },
                { sTitle: 'Category', mData: 'Category', bVisible: true, bSortable: false, sClass: 'text-left' },
                { sTitle: 'Last Modified By', mData: 'FirstName', bSortable: false, sClass: 'text-left' },
                { sTitle: 'First Name', mData: 'FirstName', bVisible: false },
                { sTitle: 'Last Name', mData: 'LastName', bVisible: false },
                { sTitle: 'Created Date', type: 'date', mData: 'CreatedOn', sClass: 'text-center', bSortable: false },
                { sTitle: 'Rule Description', mData: 'RuleDescription', bVisible: false },
                { sTitle: 'Status', sClass: 'text-center', mData: 'ChecklistActive', bSortable: false },
                { sTitle: 'Rule Json', mData: 'RuleJson', bVisible: false },
                { sTitle: 'Edit', mData: 'RuleID', sClass: 'text-center', bVisible: true, bSortable: false },
                { sTitle: 'View', mData: 'RuleID', sClass: 'text-center', bSortable: false },
                { sTitle: 'Customer', mData: 'CustomerName', bVisible: false },
                { sTitle: 'Service Type', mData: 'ReviewTypeName', bVisible: false },
                { sTitle: 'Loan Type', mData: 'LoanType', bVisible: false },
                { sTitle: 'DocVersion', mData: 'DocVersion', bVisible: false },
                { sTitle: 'Rule Type', mData: 'RuleType', bVisible: false },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [12],
                    'mRender': function (a, row, values) {
                        if (values !== null && values !== '') {
                            return '<span style="cursor:pointer" class="edit-user material-icons txt-warm">rate_review</span>';
                        }
                    }
                }, {
                    'aTargets': [13],
                    'mRender': function (a, row, values) {
                        if (values !== null && values !== '') {
                            return '<span style=\'cursor:pointer\' class=\'view-user material-icons txt-info\'>pageview</span>';
                        }
                    }
                }, {
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
                }, {
                    'aTargets': [0],
                    'orderable': false,
                    'mRender': function (date) {
                        return '';
                    }
                }, {
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
                }, {
                    'aTargets': [17],
                    'mRender': function (data, type, row) {
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
                }, {
                    'aTargets': [2],
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
                $('td .edit-user', row).unbind('click');
                $('td .edit-user', row).bind('click', () => {
                    self.editSysCheckListData(data);
                });

                $('td .view-user', row).unbind('click');
                $('td .view-user', row).bind('click', () => {
                    self.viewRowData(data);
                });
                return row;
            }
        };
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

    viewRowData(rowData) {
        this.showTable = !this.showTable;
        this.viewCheckListFormula = rowData.RuleDescription;
        this.viewCheckListName = rowData.CheckListName;
    }

    selectRowData() {
        let isDisabled = true;
        if (this.isRowNotSelected && this.dTable.rows().data().length > 0) {
            this.dTable.rows().select();
            isDisabled = false;
        } else {
            this.dTable.rows().deselect();
            isDisabled = true;
        }
        this.isRowSelectrDeselect = isDisabled;
        this._upsertCustomerService.isRowNotSelected$.next(isDisabled);
    }

    editSysCheckListData(rowData) {
        this._commonRuleBuilderService.setRuleBuilderSteps({ Step1: true, Step2: false, Step3: false });
        this._commonRuleBuilderService.EnableRuleBuilder.next(true);
        this._commonRuleBuilderService.setEditChecklistItem(rowData);
    }

    isNewCheckListButton() {
        this._commonRuleBuilderService.clearSavedChecklistItem();
        this._commonRuleBuilderService.setRuleBuilderSteps({ Step1: true, Step2: false, Step3: false });
        this._commonRuleBuilderService.EnableRuleBuilder.next(true);
    }

    checklistItemChange() {
        this._upsertCustomerService.validateCheckListItemName(this.CloneCheckListItemName);
    }

    CloneConfirmMsg() {
        const rows = this.dTable.rows('.selected').data();
        const id = [];
        for (let i = 0; i < rows.length; i++) {
            id.push(rows[i].CheckListDetailID);
        }
        this.promise = this._upsertCustomerService.CloneCheckListItem(this.CloneCheckListItemName, id);
    }

    CloneCheckListItem() {
        const rows = this.dTable.rows('.selected').data();
        if (rows.length > 1) {
            this._notificationService.showError('Select only one item to clone');
        } else {
            this.CloneCheckListItemName = rows[0].CheckListName + '_clone';
            this._upsertCustomerService.CloneModal$.next(true);
        }
    }

    DeleteConfirm() {
        const rows = this.dTable.rows('.selected').data();
        const id = [];
        for (let i = 0; i < rows.length; i++) {
            id.push(rows[i].CheckListDetailID);
        }
        this.promise = this._upsertCustomerService.DeleteCheckListItem(id);
    }

    ngAfterViewInit() {
        if (isTruthy(this.datatableEl)) {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
                const dt = this.datatableEl;
                dtInstance.on('select', (s) => {
                    this.isRowNotSelected = !(this.dTable.rows('.selected').data().length > 0);
                });

                dtInstance.on('deselect', (s) => {
                    this.isRowNotSelected = !(this.dTable.rows('.selected').data().length > 0);
                });

                const self = this;
                $('.wrapperTable thead tr th').each(function (i) {
                    if (i !== 0) {
                        const title = $(this).text();
                        if (title !== 'Edit' && title !== 'View' && title !== 'Status') {
                            if (title === 'Checklist Item Name') {
                                $(this).html('<p style="margin:0;">' + title + '</p><input type="text" style="width:100%" class="form-control input-sm"/>');
                            } else {
                                $(this).html('<p style="margin:0;">' + title + '</p><input type="text" class="form-control input-sm"/>');
                            }

                            $('input', this).on('keyup change', function () {
                                const val = this['value'];
                                let thIndex = i;
                                const d = self.dTable;
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
                                if (self.dTable.column(18).search() !== val && val !== '') {
                                    self.dTable.column(18).search(parseInt(val, 10)).draw();
                                } else if (val === '') {
                                    self.dTable.column(18).search('').draw();
                                }
                            });
                        }
                    }
                });
                self.dTable.columns.adjust();

                this.promise = this._upsertCustomerService.GetCheckListGroup();

            });
        }
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
