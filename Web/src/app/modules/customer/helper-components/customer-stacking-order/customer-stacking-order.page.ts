import { Component, OnInit, OnDestroy, ViewChild, AfterContentChecked, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { NotificationService } from '@mts-notification';
import { DataTableDirective } from 'angular-datatables';
import { StackingOrderDetailTable, StackingOrderGroupMasters } from '../../models/stacking-order-detail-table.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DragulaService } from '@mts-dragula';

@Component({
    selector: 'mts-customer-stacking-order',
    styleUrls: ['customer-stacking-order.page.css'],
    templateUrl: 'customer-stacking-order.page.html'
})
export class CustomerStackingOrderComponent implements OnInit, OnDestroy, AfterContentChecked, AfterViewInit {

    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    @ViewChild('addGroupModal') _addGroupModal: ModalDirective;
    promise: Subscription;
    dtOptions: any = {};
    dTable: any;
    showEditor = false;
    docStackingOrder: StackingOrderDetailTable[] = [];
    docGroupStackingOrder: StackingOrderDetailTable[] = [];
    StackingOrderGroupName = '';
    StackingOrderMapped = true;

    constructor(
        private dragService: DragulaService,
        private _upsertCustomerService: UpsertCustomerService,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private _stackingOrderDetail: StackingOrderDetailTable[] = [];
    private tableAligned = false;

    ngOnInit() {

        this.StackingOrderMapped = this._upsertCustomerService.GetCurrentStackingOrder().StackingOrderID > 0;

        this._subscriptions.push(this._upsertCustomerService.StackingOrderDetailTable$.subscribe((res: StackingOrderDetailTable[]) => {
            this.showEditor = false;
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();
            setTimeout(() => {
                this.dTable.columns.adjust();
            }, 300);
            this._stackingOrderDetail = res.slice();
        }));

        this._subscriptions.push(this.dragService.dropModel('nested-bag').subscribe((value) => {
            this.onDropModel(value);
        }));
        this._subscriptions.push(this.dragService.removeModel('nested-bag').subscribe((value) => {
            this.onRemoveModel(value.item.slice(1));
        }));
        this._subscriptions.push(this.dragService.drag('nested-bag').subscribe((value) => {
            this.onDragElement(value);
        }));

        this.dtOptions = {
            select: false,
            'iDisplayLength': 7,
            'ordering': false,
            'bPaginate': false,
            'scrollY': 'calc(100vh - 440px)',
            'aLengthMenu': [[7, 10, 25, 50, -1], [7, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Document Type ID', mData: 'DocumentTypeID', bVisible: false },
                { sTitle: 'Document Type Name', sClass: 'text-left', mData: 'DocumentTypeName', bSortable: false },
                { mData: 'StackingOrderGroupDetails', sClass: 'text-left', sTitle: 'Group Name', bSortable: false },
                { sTitle: 'Sequence ID', type: 'date', mData: 'SequenceID', bSortable: false },
                { sTitle: 'StackingOrderDetail ID', mData: 'StackingOrderDetailID', bVisible: false },
                { sTitle: 'StackingOrder ID', mData: 'StackingOrderID', bVisible: false },
                { mData: 'DocFieldList', bVisible: false },
                { mData: 'OrderByFieldID', bVisible: false },
                { mData: 'DocFieldValueId', bVisible: false },
                { mData: 'StackingOrderGroupDetails', bVisible: false },
                { mData: 'isGroup', bVisible: false },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [2],
                    'mRender': function (data) {
                        if (data.length > 0) {
                            return data[0].StackingOrderGroupName;
                        }
                        return '';
                    }
                }
            ]
        };

    }

    ngAfterContentChecked() {
        if (isTruthy(this.dTable) && !this.tableAligned) {
            $('.dataTables_info').parent().removeClass('col-sm-5').addClass('col-md-6 text-left');
            $('.dataTables_paginate').parent().removeClass('col-sm-5').addClass('col-md-6 text-right');
            $('.dataTables_filter').parent().removeClass('col-sm-5').addClass('col-md-6 text-right');
            $('.dataTables_length').parent().removeClass('col-sm-5').addClass('col-sm-6 text-left');
            this.tableAligned = true;
        }
    }

    SaveStackingOrder() {
        this._upsertCustomerService.SaveStackingOrder(this.docStackingOrder.slice(), this.docGroupStackingOrder.slice());
    }

    CloseEditStackingOrderModal() {
        this._upsertCustomerService.GetStackingOrderDetails();
        this.showEditor = false;
    }

    onDragElement(args) {
        if (args.el.dataset.docid === 'false' || args.el.dataset.docid === 'true') {
            const groupElements = document.getElementsByClassName('groupElements');
            let isKeepGoing = true;
            for (let i = 0; i < groupElements.length; i++) {
                if (isKeepGoing) {
                    isKeepGoing = this.CheckGroupCollapsed(groupElements[i]);
                }
            }
            if (args.el.dataset.docid === 'false' || !isKeepGoing) {
                this._notificationService.showError('Drag is Not Allowed,Collapse All Groups');
                const drake = this.dragService.find('nested-bag').drake;
                drake.cancel(true);
            }
        }
    }

    onDropModel(args) {
        const el = args.el;
        const target = args.target;
        const source = args.source;
        const dropElt = args.item;

        if (typeof dropElt !== 'undefined' && typeof dropElt.StackingOrderGroupDetails !== 'undefined' && this.docStackingOrder.length > 0) {
            el.classList.add('DropHighlight');
            if (target.classList.contains('dragScroll2')) {
                if (dropElt.StackingOrderGroupDetails.length === 0) {
                    dropElt.StackingOrderGroupDetails.push({ StackingOrderGroupName: target.dataset.docname, GroupSortField: '' });
                    dropElt.isGroup = true;
                    const isGrouped = this.filterCommonElements(target.dataset.docname, dropElt);
                    if (!isGrouped) {
                        const index = this.docGroupStackingOrder.findIndex(a => a.DocumentTypeID === dropElt.DocumentTypeID);
                        this.docGroupStackingOrder.splice(index, 1);
                        const i = args.targetModel.findIndex(a => a.DocumentTypeID === dropElt.DocumentTypeID);
                        args.targetModel.splice(i, 1);
                        dropElt.StackingOrderGroupDetails = [];
                        dropElt.isGroup = false;
                        const drake = this.dragService.find('nested-bag').drake;
                        drake.cancel(true);
                        setTimeout(() => {
                            this.docStackingOrder.push(dropElt);
                        }, 100);

                    }
                }
            } else if (dropElt.StackingOrderGroupDetails.length > 0 && (dropElt.StackingOrderGroupDetails[0].StackingOrderGroupName) === '') {
                dropElt.StackingOrderGroupDetails[0].StackingOrderGroupName = target.dataset.docName;
            } else if (target.id === 'dragScrollContainer' && source.classList.contains('dragScroll2')) {
                dropElt.StackingOrderGroupDetails = [];
                dropElt.isGroup = false;
                this.filterCommonElements(source.dataset.docname, dropElt);
            }
        }
    }

    onRemoveModel(args) {
        const [el, source] = args;
    }

    CheckGroupCollapsed(elements): boolean {
        if (elements.dataset.docid !== 'true' && elements.dataset.docid !== 'false') {
            return true;
        }
        return elements.dataset.docid === 'true';
    }

    setStackDocSelected(i: number) { }

    SetOrderByField(FieldID: any, _docID: number) {
        const _fieldID = parseInt(FieldID, 10);
        if (_docID > 0 && _fieldID > 0) {
            this._upsertCustomerService.SetTenantOrderByField(_docID, _fieldID);
        }
    }

    SetDocFieldValue(FieldID: any, _docID: number) {
        const _fieldID = parseInt(FieldID, 10);
        if (_docID > 0 && _fieldID > 0) {
            this._upsertCustomerService.SetTenantDocFieldValue(_docID, _fieldID);
        }
    }

    SetDocGroupFieldValue(groupName, stackGroupID, orderbyfield, value) {
        if (isTruthy(orderbyfield)) {
            this._upsertCustomerService.SetDocGroupFieldValue(this.docGroupStackingOrder.slice(), groupName, stackGroupID, value);
        }
    }

    removeOrder(i: any) { this.docStackingOrder.splice(i, 1); }

    removeGroup(groupName: string, i: any) {
        const drpDocs = this.docGroupStackingOrder.filter(x => x.StackingOrderGroupDetails[0].StackingOrderGroupName === groupName);
        drpDocs.forEach(element => {
            const index = this.docGroupStackingOrder.findIndex(dg => dg.DocumentTypeName === element.DocumentTypeName);
            this.docGroupStackingOrder.splice(index, 1);
        });
        this.docStackingOrder.splice(i, 1);
    }

    removeGroupOrder(i: any) { this.docGroupStackingOrder.splice(i, 1); }

    CollapseGroup() {
        this.docStackingOrder.forEach(element => {
            if (element.isGroup) {
                element.DocumentTypeID = true;
            }
        });
    }

    AddGroup() {
        this._addGroupModal.show();
        this.StackingOrderGroupName = '';
    }

    SaveGroupName() {
        if (this.StackingOrderGroupName !== '') {
            let keepGoing = false;
            this.docStackingOrder.forEach(element => {
                if (element.isGroup && element.StackingOrderGroupDetails[0].StackingOrderGroupName.toLowerCase() === this.StackingOrderGroupName.toLowerCase() && (!keepGoing)) {
                    keepGoing = true;
                }
            });
            if (!keepGoing) {
                this.docStackingOrder.push({
                    DocFieldList: [],
                    DocFieldValueId: 0,
                    DocumentTypeID: false,
                    DocumentTypeName: '',
                    OrderByFieldID: 0,
                    SequenceID: 0,
                    StackingOrderDetailID: 0,
                    StackingOrderGroupDetails: [new StackingOrderGroupMasters(this.StackingOrderGroupName)],
                    StackingOrderID: 0,
                    isGroup: true,
                    isContainer: true
                });
            } else {
                this._notificationService.showError('Group Name Already Exists');
            }
            this._addGroupModal.hide();
        } else {
            this._notificationService.showError('Enter Group Name');
        }
    }

    EditStackingOrder() {
        this.docStackingOrder = [];
        this.docGroupStackingOrder = [];
        for (let i = 0; i < this._stackingOrderDetail.length; i++) {
            if (this._stackingOrderDetail[i].StackingOrderGroupDetails.length === 0) {
                this.docStackingOrder.push(this._stackingOrderDetail[i]);
            } else {
                const dupGroup = this.docStackingOrder.filter(el => (el.StackingOrderGroupDetails.length > 0 ? el.StackingOrderGroupDetails[0].StackingOrderGroupName : 'none') === this._stackingOrderDetail[i].StackingOrderGroupDetails[0].StackingOrderGroupName);
                if (dupGroup.length === 0) {
                    const doc = JSON.parse(JSON.stringify(this._stackingOrderDetail[i]));
                    this.docStackingOrder.push(doc);
                    const index = doc.DocFieldList.findIndex(x => x.Name === doc.StackingOrderGroupDetails[0].GroupSortField);
                    this.docStackingOrder[this.docStackingOrder.length - 1].OrderByFieldID = doc.DocFieldList[index].FieldID;
                    this.docStackingOrder[this.docStackingOrder.length - 1].DocumentTypeID = true;
                }
                this.docGroupStackingOrder.push(this._stackingOrderDetail[i]);
            }
        }
        let groupElements = [];
        const docGroupElements = this.docStackingOrder.filter(a => a.StackingOrderGroupDetails.length > 0);
        docGroupElements.forEach(element => {
            groupElements = [];
            const groupName = element.StackingOrderGroupDetails[0].StackingOrderGroupName;
            element.StackingOrderGroupDetails[0].TrimmedStackingOrderGroupName = element.StackingOrderGroupDetails[0].StackingOrderGroupName.replace(/[\s]/g, '');
            const elements = this.docGroupStackingOrder.filter(x => x.StackingOrderGroupDetails[0].StackingOrderGroupName === groupName);
            let displayName = [];
            elements.forEach(el => {
                displayName = [];
                el.DocFieldList.forEach(disp => {
                    displayName.push(disp.Name);
                });
                groupElements.push(displayName);
            });
            const docFieldList = this.findCommonElements(groupElements);
            const data = JSON.stringify(element);
            element.DocFieldList = [];
            const datas = JSON.parse(data);
            docFieldList.forEach(field => {
                element.DocFieldList.push((datas.DocFieldList.filter(y => y.Name === field)[0]));
            });
        });
    }

    findCommonElements(arr) {
        const lookupArray = [];
        const commonElementArray = [];
        for (let arrayIndex = 0; arrayIndex < arr.length; arrayIndex++) {
            for (let childArrayIndex = 0; childArrayIndex < arr[arrayIndex].length; childArrayIndex++) {
                if (lookupArray[arr[arrayIndex][childArrayIndex]]) {
                    lookupArray[arr[arrayIndex][childArrayIndex]]++;
                } else {
                    lookupArray[arr[arrayIndex][childArrayIndex]] = 1;
                }
                if (lookupArray[arr[arrayIndex][childArrayIndex]] === arr.length) {
                    commonElementArray.push(arr[arrayIndex][childArrayIndex]);
                }
            }
        }
        return commonElementArray;
    }

    filterCommonElements(groupName, drpElt): boolean {
        this.docGroupStackingOrder.push(drpElt);
        const elements = this.docGroupStackingOrder.filter(el => (el.StackingOrderGroupDetails.length > 0 ? el.StackingOrderGroupDetails[0].StackingOrderGroupName : 'none') === groupName);
        if (elements.length > 0) {
            const groupElements = [];
            let displayName = [];
            elements.forEach(el => {
                displayName = [];
                el.DocFieldList.forEach(disp => {
                    displayName.push(disp.Name);
                });
                groupElements.push(displayName);
            });
            const docFieldList = this.findCommonElements(groupElements);
            if (docFieldList.length > 0) {
                if (drpElt.isGroup) {
                    const i = this.docStackingOrder.findIndex(el => (el.StackingOrderGroupDetails.length > 0 && !(el.isContainer) ? el.StackingOrderGroupDetails[0].StackingOrderGroupName : 'none') === groupName);
                    this.docStackingOrder.splice(i, 1);
                }
                const index = this.docStackingOrder.findIndex(el => (el.StackingOrderGroupDetails.length > 0 ? el.StackingOrderGroupDetails[0].StackingOrderGroupName : 'none') === groupName);
                this.docStackingOrder[index].DocFieldList = [];
                docFieldList.forEach(el => {
                    const data = elements[0].DocFieldList.filter(e => e.Name === el);

                    this.docStackingOrder[index].DocFieldList.push(data[0]);
                });
                this.docStackingOrder[index].OrderByFieldID = 0;
                return true;
            } else {
                this._notificationService.showError('No Common Fields Available');
                return false;
            }
        }

    }

    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            if (this.StackingOrderMapped) {
                this.promise = this._upsertCustomerService.GetStackingOrderDetails();
            }
        });
    }

    ngOnDestroy() {
        this.dragService.destroy('nested-bag');
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
