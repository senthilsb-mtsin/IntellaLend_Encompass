import { Component, OnInit, OnDestroy, QueryList, ViewChildren, ElementRef, ViewChild } from '@angular/core';
import { CommonService } from 'src/app/shared/common/common.service';
import { Subscription } from 'rxjs';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { NotificationService } from '@mts-notification';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DocumentSearchService } from '../../service/document-search.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DragulaService } from '@mts-dragula';

@Component({
    selector: 'mts-edit-stacking-order',
    styleUrls: ['edit-stacking-order.page.css'],
    templateUrl: 'edit-stacking-order.page.html',
    providers: [DocumentSearchService],
})
export class EditStackingOrderComponent implements OnInit, OnDestroy {
    stackingOrderList: any[] = [];
    sysEditStackingOrderName = '';
    assignedDocSearchval = '';
    StackingOrderGroupName = '';
    allDocSearchval = '';
    UnAssignedDocTypes: any[] = [];
    FilterUnAssignedDocs: any[] = [];
    docSysStackingOrder: any[] = [];
    docGroupStackingOrder: any[] = [];
    curentStackingOrderID = 0;
    stackOrderType = 'createEdit';
    promise: Subscription;
    @ViewChildren('allDocs') _allDocChildrens: QueryList<ElementRef>;
    @ViewChildren('stackDocs') _stackDocChildrens: QueryList<ElementRef>;
    @ViewChild('addGroupModal') _addGroupModal: ModalDirective;
    constructor(
        private _commonService: CommonService,
        private _notificationService: NotificationService,
        private _addLoanTypeService: AddLoanTypeService,
        private _documentSearch: DocumentSearchService,
        private dragService: DragulaService
    ) { }
    private docName = '';
    private CommonFields: any[] = [];
    private temp: any[] = [];
    private isDocument = false;

    private _subscription: Subscription[] = [];
    private _stackGrp: any[] = [];
    private isKeepGoing = true;

    ngOnInit() {
        this._subscription.push(this._addLoanTypeService.SysStackingOrderDetailData.subscribe((res: { StackingOrder: any[], StackingOrderGroup: any[], UnAssignedDocTypes: any[] }) => {
            this.docSysStackingOrder = res.StackingOrder;
            this.docGroupStackingOrder = res.StackingOrderGroup;
            this.UnAssignedDocTypes = res.UnAssignedDocTypes;
            this.FilterUnAssignedDocs = res.UnAssignedDocTypes;
        }));
        this._subscription.push(this.dragService.dropModel('nested-bag').subscribe((value) => {
            this.onDropModel(value.item, value, false);
        }));
        this._subscription.push(this.dragService.drag('nested-bag').subscribe((value) => {
            this.onDragElement(value);
        }));
        this._subscription.push(this.dragService.dragend().subscribe(() => {
            this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
        }));
        this._subscription.push(this._commonService.SystemStackingOrderMaster.subscribe((res: any[]) => {
            this._stackGrp = res;
        }));
        this.stackOrderType = this._addLoanTypeService.getStackType();
        this._addLoanTypeService.getStackUnDocTypes(this.stackOrderType);
        this._stackGrp = this._commonService.GetSysStackingOrderGroup();
        this.sysEditStackingOrderName = this._addLoanTypeService.getStackingOrder().Description;
        this.curentStackingOrderID = this._addLoanTypeService.getStackingOrder().StackingOrderID;

        if (this.stackOrderType === 'createEdit') {
            this.promise = this._addLoanTypeService.getStackAssignDocTypes();
             this._subscription.push(this._addLoanTypeService.stackAssignedDocs.subscribe((res: any[]) => {
                res.forEach(element => {
                    element.StackingOrderID = this.curentStackingOrderID;
                });
                this.docSysStackingOrder = res;
                this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder, this.docGroupStackingOrder);
               // this.FilterUnAssignedDocs = res;
            }));
            this._subscription.push(this._addLoanTypeService.StackUnAssignedDocs.subscribe((res: any[]) => {
                res.forEach(element => {
                    element.StackingOrderID = this.curentStackingOrderID;
                });
                this.UnAssignedDocTypes = res;
                this.FilterUnAssignedDocs = res;
            }));
        } else {
            this.promise = this._addLoanTypeService.getSystemStackingOrderDetails();
        }
    }

    InsertRemovedData(drpEltModel, value) {
        setTimeout(() => {
            value.targetModel.splice(value.targetIndex, 1);
            this.docSysStackingOrder.push(drpEltModel);
            this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
        }, 100);
    }

    CheckGroupCollapsed(elements): boolean {
        if (elements.dataset.docid === 'true') {
            return true;
        } else if (elements.dataset.docid === 'false') {
            return false;
        } else {
            return true;
        }
    }

    SysStackingOrderRedundancyCheck(vals: string) {
        this._addLoanTypeService.checkDuplicateStackingOrderGrp(vals);
    }

    StackingClose() {
        this._addLoanTypeService.stackingOrderBack.next('');
    }

    UnassignedDocFilterSearch(search: string) {
        if (isTruthy(search)) {
            this.UnAssignedDocTypes = this._documentSearch.DocFiltersearch(search, this.FilterUnAssignedDocs, this.docGroupStackingOrder, this.docSysStackingOrder);
        } else {
            this.UnAssignedDocTypes = this.FilterUnAssignedDocs.slice();
        }
    }

    AddSysStackingOrder(docID) {
        const index = this.UnAssignedDocTypes.findIndex(l => l.DocumentTypeID === docID);
        if (index !== undefined) {
            this.docSysStackingOrder.push(this.UnAssignedDocTypes[index]);
            if (this.allDocSearchval !== null) {
                const assignDocValue = this.allDocSearchval;
                this.allDocSearchval = '';
                this.UnAssignedDocTypes.splice(index, 1);
                this.allDocSearchval = assignDocValue;
            } else {
                this.UnAssignedDocTypes.splice(index, 1);
            }
            this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
        }
    }

    setDocSelected(index) {
        const clist = this._allDocChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._allDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._allDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    MoveFromAllDoc() {
        for (let i = 0; i < this._allDocChildrens['_results'].length; i++) {
            const clist = this._allDocChildrens['_results'][i].nativeElement.classList.toString();
            const docID = parseInt(this._allDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);
            const index = this.UnAssignedDocTypes.findIndex(l => l.DocumentTypeID === docID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this.docSysStackingOrder.push(this.UnAssignedDocTypes[index]);
                this.UnAssignedDocTypes.splice(index, 1);
            }
        }
        this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
    }

    MoveToAllDoc() {
        for (let i = 0; i < this._stackDocChildrens['_results'].length; i++) {
            const clist = this._stackDocChildrens['_results'][i].nativeElement.classList.toString();
            const docID = parseInt(this._stackDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);
            const index = this.docSysStackingOrder.findIndex(l => l.DocumentTypeID === docID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this.UnAssignedDocTypes.push(this.docSysStackingOrder[index]);
                this.docSysStackingOrder.splice(index, 1);
            }
        }
        this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
    }

    CollapseGroup() {
        this.docSysStackingOrder.forEach(element => {
            if (element.isGroup) {
                element.DocumentTypeID = true;
            }
        });
    }

    setStackDocSelected(index) {
        const clist = this._stackDocChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._stackDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._stackDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    RemoveGroup(dName: string) {
        const docindex = this.docSysStackingOrder.findIndex(l => l.StackingOrderGroupName === dName);
        const documents = this.docGroupStackingOrder.filter(l => l.StackingOrderGroupName === dName);
        if (documents.length > 0) {
            documents.forEach(ele => {
                const index = this.docGroupStackingOrder.findIndex(l => l.DocumentTypeID === ele.DocumentTypeID);
                this.docGroupStackingOrder[index].StackingOrderGroupName = '';
                this.UnAssignedDocTypes.push(this.docGroupStackingOrder[index]);
                this.docGroupStackingOrder.splice(index, 1);
            });
        }
        this.docSysStackingOrder.splice(docindex, 1);
        this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
    }

    SetDocFieldValue(FieldID: any, DocID: number, type: string) {
        const field = parseInt(FieldID, 10);
        if (DocID > 0 && field > 0) {
            const req = { DocumentTypeID: DocID, FieldID: field };
            this._addLoanTypeService.setDocFieldValue(req, type);
        }
    }

    RemoveGroupSysStackingOrder(docID) {
        const index = this.docGroupStackingOrder.findIndex(l => l.DocumentTypeID === docID);
        this.docName = this.docGroupStackingOrder[index].StackingOrderGroupName;
        if (index !== undefined) {
            this.docGroupStackingOrder[index].StackingOrderGroupName = '';
            this.UnAssignedDocTypes.push(this.docGroupStackingOrder[index]);
            if (this.assignedDocSearchval !== null) {
                const assignDocValue = this.assignedDocSearchval;
                this.assignedDocSearchval = '';
                this.docGroupStackingOrder.splice(index, 1);
                this.assignedDocSearchval = assignDocValue;
            } else {
                this.docGroupStackingOrder.splice(index, 1);
            }
            this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
        }

        this.findcommonFields();
    }

    findcommonFields() {
        const documents = this.docGroupStackingOrder.filter(x => x.StackingOrderGroupName === this.docName);
        const stack = this.docSysStackingOrder.filter(x => x.StackingOrderGroupName === this.docName);
        const values = [];
        this.CommonFields = [];

        if (documents.length > 0) {
            documents.forEach(element => {
                element.DocFieldList.forEach(el => {
                    this.CommonFields.push(el.DisplayName);
                });
            });
            let j = 0;
            documents[0].DocFieldList.forEach(element => {
                j = 0;
                this.CommonFields.forEach(ele => {
                    if (element.DisplayName === ele) {
                        j++;
                    }
                });

                if (j === documents.length) {
                    values.push(element);
                }
            });
            stack[0].DocFieldList = [];

            values.forEach(element => {
                stack[0].DocFieldList.push(element);
            });
        } else {
            stack[0].DocFieldList = [];
        }

    }

    RemoveSysStackingOrder(docID: number) {
        const index = this.docSysStackingOrder.findIndex(l => l.DocumentTypeID === docID);
        if (index !== undefined) {
            this.UnAssignedDocTypes.push(this.docSysStackingOrder[index]);
            if (this.assignedDocSearchval !== null) {
                const assignDocValue = this.assignedDocSearchval;
                this.assignedDocSearchval = '';
                this.docSysStackingOrder.splice(index, 1);
                this.assignedDocSearchval = assignDocValue;
            } else {
                this.docSysStackingOrder.splice(index, 1);
            }
            this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
        }
    }

    SaveGroupName() {
        if (this.StackingOrderGroupName !== '') {
            let keepGoing = false;
            this.docSysStackingOrder.forEach(element => {
                if (element.isGroup && element.StackingOrderGroupName.toLowerCase() === this.StackingOrderGroupName.toLowerCase() && (!keepGoing)) {
                    keepGoing = true;
                }
            });

            if (!keepGoing) {
                this.docSysStackingOrder.push(
                    {
                        DocumentTypeID: false,
                        DocumentTypeName: this.StackingOrderGroupName,
                        SequenceID: 0,
                        StackingOrderDetailID: 0,
                        StackingOrderID: 0,
                        DocFieldList: [],
                        isGroup: true,
                        isContainer: true,
                        StackingOrderGroupName: this.StackingOrderGroupName,
                    }
                );
                this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
                this.StackingOrderGroupName = '';
                this._addGroupModal.hide();
                this.temp = [];
            } else {
                this._notificationService.showError('Group Name Already Exists');
            }
        } else {
            this._notificationService.showError('Enter Group Name');
        }
    }

    SetDocGroupFieldValue(value, target, stackGroupID) {
        if (target.selectedOptions[0].text !== '') {
            const groupName = target.parentElement.dataset.docname;
            const docs = this.docGroupStackingOrder.filter(x => x.StackingOrderGroupName === groupName);
            docs.forEach(el => {
                el.StackingOrderFieldName = target.selectedOptions[0].text;
            });
            const groupdetails = { ID: stackGroupID, Name: groupName, StackingOrderFieldName: target.selectedOptions[0].text };
            const inputs = { TableSchema: 'IL', StackingOrderDetails: docs, StackOrder: groupdetails, };
            this._addLoanTypeService.setDocGrpFieldValue(inputs);
        }
    }

    ngOnDestroy() {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
        this.dragService.destroy('nested-bag');
    }

    private onDropModel(args, value, isDropped) {
        const el = value.el;
        const target = value.target;
        const source = value.source;
        const drpEltModel = args;

        if (target.getAttribute('DivType') === 'Assigned') {
            el.classList.add('DropHighlight');
        }
        this.docName = '';
        this.docName = target.dataset.docname;
        if (typeof drpEltModel !== 'undefined' && drpEltModel.isComponentName === 'SystemStackingOrder') {
            if (target.classList.contains('dragScroll2') && isDropped === false) {
                if (this.docSysStackingOrder.length > 0) {
                    drpEltModel.StackingOrderGroupName = this.docName;
                }
                if (drpEltModel.StackingOrderGroupName === this.docName) {
                    if (isTruthy(this.docSysStackingOrder) && this.docSysStackingOrder.length > 0) {
                        const documents = this.docSysStackingOrder.filter(x => x.StackingOrderGroupName === this.docName && x.isContainer === true);
                        let i = 0;
                        if (documents.length > 0) {
                            this.temp = documents[0].DocFieldList;
                        }
                        if (this.temp.length > 0) {
                            documents[0].DocFieldList = [];

                            drpEltModel.DocFieldList.forEach(element => {
                                this.temp.forEach(ele => {
                                    if (element.DisplayName === ele.DisplayName) {
                                        documents[0].DocFieldList.push(element);
                                        i++;
                                    }
                                });
                            });

                            if (i === 0) {
                                drpEltModel.StackingOrderGroupName = '';
                                if (typeof source.attributes.divtype.value !== 'undefined' && source.attributes.divtype.value === 'Assigned' && target.classList.contains('dragScroll2')) {
                                    const drake = this.dragService.find('nested-bag').drake;
                                    drake.cancel(true);
                                    this.InsertRemovedData(drpEltModel, value);
                                } else {
                                    setTimeout(() => {
                                        this.UnAssignedDocTypes.push(value.targetModel[value.targetIndex]);
                                        value.targetModel.splice(value.targetIndex, 1);
                                    }, 300);
                                }

                                documents[0].DocFieldList = this.temp;
                                this._notificationService.showError('No Common Fields Are There');
                            } else {
                                this.docGroupStackingOrder.push(drpEltModel);
                                this._addLoanTypeService.setDocSysStackingOrder(this.docSysStackingOrder.slice(), this.docGroupStackingOrder.slice());
                            }
                            isDropped = true;
                        }
                        if (isTruthy(documents) && this.temp.length === 0) {
                            if (drpEltModel.DocFieldList.length > 0) {
                                this.isDocument = false;
                                if (documents.length > 0) {
                                    documents[0].DocFieldList = [];
                                    drpEltModel.DocFieldList.forEach(ele => {
                                        documents[0].DocFieldList.push(ele);
                                    });
                                    this.temp = documents[0].DocFieldList;
                                    isDropped = true;
                                }
                            } else {
                                if (typeof source.attributes.divtype.value !== 'undefined' && source.attributes.divtype.value === 'Assigned' && target.classList.contains('dragScroll2')) {
                                    const drake = this.dragService.find('nested-bag').drake;
                                    drake.cancel(true);
                                    this.InsertRemovedData(drpEltModel, value);
                                } else {
                                    setTimeout(() => {
                                        this.UnAssignedDocTypes.push(value.targetModel[value.targetIndex]);
                                        value.targetModel.splice(value.targetIndex, 1);
                                        drpEltModel.StackingOrderGroupName = '';
                                    }, 100);
                                }
                                this._notificationService.showError('No Fields Are There');
                                isDropped = true;
                            }
                        }
                    } else {
                        drpEltModel.StackingOrderGroupName = '';
                        isDropped = true;
                    }
                }
            } else {
                if ((drpEltModel !== undefined && (drpEltModel.StackingOrderGroupName !== undefined && drpEltModel.StackingOrderGroupName !== '')) && (drpEltModel.isContainer === undefined || !(drpEltModel.isContainer))) {
                    this.docName = drpEltModel.StackingOrderGroupName;
                    const index = this.docGroupStackingOrder.findIndex(l => l.DocumentTypeID === drpEltModel.DocumentTypeID);
                    this.docGroupStackingOrder.splice(index, 1);
                    this.findcommonFields();
                    drpEltModel.StackingOrderGroupName = '';
                    drpEltModel.isGroup = false;
                }
                isDropped = true;
            }
        }
    }

    private onDragElement(args) {
        if (args.el.dataset.docid === 'false' || args.el.dataset.docid === 'true') {
            const groupElements = document.getElementsByClassName('groupElements');
            this.isKeepGoing = true;
            for (let i = 0; i < groupElements.length; i++) {
                if (this.isKeepGoing) {
                    this.isKeepGoing = this.CheckGroupCollapsed(groupElements[i]);
                }
            }
            if (args.el.dataset.docid === 'false' || !this.isKeepGoing) {
                this._notificationService.showError('Drag is Not Allowed,Collapse All Groups');
                const drake = this.dragService.find('nested-bag').drake;
                drake.cancel(true);
            }
        }
    }
}
