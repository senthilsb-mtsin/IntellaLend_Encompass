import { Component, OnInit, OnDestroy, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { DragulaService } from '@mts-dragula';
import { Subscription, Subject } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { LenderSearchService } from '../../service/lender-search.service';
import { element } from 'protractor';

@Component({
    selector: 'mts-assign-lender',
    styleUrls: ['assign-lender.component.css'],
    templateUrl: 'assign-lender.component.html',
    // providers: [LoanTypeSearchService]
})
export class AssignLenderComponent implements OnInit, OnDestroy {
    //#region Public Variables
    AllLenders: any[] = [];
    AssignedLenders: any[] = [];
    FilterAllLenders: any[] = [];
    allLenderSearchval = '';
    assignedLenderSearchval = '';

    @ViewChildren('allLoans') _allLoanChildrens: QueryList<ElementRef>;
    @ViewChildren('assignedLoan') _assignLoanChildrens: QueryList<ElementRef>;
    //#endregion Public Variables

    //#region Constructor
    constructor(
        private _addServiceTypeService: AddServiceTypeService,
        private _lenderSearch: LenderSearchService,
        private dragService: DragulaService,
    ) { }
    //#endregion Constructor

    //#region  Private Variables
    private subscriptions: Subscription[] = [];
    //#endregion Private Variables

    //#region  Public Methods
    ngOnInit(): void {
        this.subscriptions.push(this._addServiceTypeService.allLenders$.subscribe((res: any[]) => {
            this.AllLenders = res;
            this.FilterAllLenders = res;
        }));
        this.subscriptions.push(this._addServiceTypeService.allAssignedLenders.subscribe((res: any[]) => {
            this.AssignedLenders = res;
        }));
        this.dragService.dropModel().subscribe((value) => {
            this.onDropModel(value);
        });
        this.resetPageData();
    }

    resetPageData() {
        this._addServiceTypeService.getAssignedLenders();
    }

    MoveAllLenderToAssignedLender() {
        this._addServiceTypeService.IsAdd$.next(true);
        const allLender = [];
        this.AllLenders.forEach(ele => {
            this.AssignedLenders.push(ele);
        });
        this.AllLenders = allLender;
        this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
    }

    MoveAssignedLenderToAllLender() {
        const assignedLender = [];
        this.AssignedLenders.forEach(ele => {
            this.AllLenders.push(ele);
        });
        this.AssignedLenders = assignedLender;
        this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
    }

    MoveFromAllLender() {
        for (let i = 0; i < this._allLoanChildrens['_results'].length; i++) {
            const clist = this._allLoanChildrens['_results'][i].nativeElement.classList.toString();
            const CustomerID = parseInt(this._allLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
            const index = this.AllLenders.findIndex(l => l.CustomerID === CustomerID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this._addServiceTypeService.IsAdd$.next(true);
                this.AssignedLenders.push(this.AllLenders[index]);
                this.AllLenders.splice(index, 1);
                this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
            }
        }
    }

    MoveToAllLender() {
        for (let i = 0; i < this._assignLoanChildrens['_results'].length; i++) {
            const clist = this._assignLoanChildrens['_results'][i].nativeElement.classList.toString();
            const CustomerID = parseInt(this._assignLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
            const index = this.AssignedLenders.findIndex(l => l.CustomerID === CustomerID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this.AllLenders.push(this.AssignedLenders[index]);
                this.AssignedLenders.splice(index, 1);
                this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
            }
        }
    }

    MoveAllLoansToAssignedLoans() {
        if (this._allLoanChildrens['_results'].length > 0) {
            for (let i = 0; i < this._allLoanChildrens['_results'].length; i++) {
                const clist = this._allLoanChildrens['_results'][i].nativeElement.classList.toString();
                if (clist.indexOf('SelectHighlight') > -1) {
                    this._allLoanChildrens['_results'][i].nativeElement.className = clist.replace('SelectHighlight', '');
                } else {
                    this._allLoanChildrens['_results'][i].nativeElement.className = clist + ' ' + 'SelectHighlight';
                }
            }
            for (let i = 0; i < this._allLoanChildrens['_results'].length; i++) {
                const clist = this._allLoanChildrens['_results'][i].nativeElement.classList.toString();
                const CustomerID = parseInt(this._allLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
                const index = this.AllLenders.findIndex(l => l.CustomerID === CustomerID);
                if (clist.indexOf('SelectHighlight') > -1) {
                    this.AssignedLenders.push(this.AllLenders[index]);
                    this.AllLenders.splice(index, 1);
                    this._addServiceTypeService.setLoanTypes(this.AssignedLenders, this.AllLenders);
                }
            }
        } else {
            for (let index = 0; index < this._assignLoanChildrens['_results'].length; index++) {
                const clist = this._assignLoanChildrens['_results'][index].nativeElement.classList.toString();
                if (clist.indexOf('SelectHighlight') > -1) {
                    this._assignLoanChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
                } else {
                    this._assignLoanChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
                }
            }
            for (let i = 0; i < this._assignLoanChildrens['_results'].length; i++) {
                const clist = this._assignLoanChildrens['_results'][i].nativeElement.classList.toString();
                const CustomerID = parseInt(this._assignLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
                const index = this.AssignedLenders.findIndex(l => l.CustomerID === CustomerID);
                if (clist.indexOf('SelectHighlight') > -1) {
                    this.AllLenders.push(this.AssignedLenders[index]);
                    this.AssignedLenders.splice(index, 1);
                    this._addServiceTypeService.setLoanTypes(this.AssignedLenders, this.AllLenders);
                }
            }
        }
    }

    AddLender(CustomerID: number) {
        if (CustomerID > 0) {
            const index = this.AllLenders.findIndex(l => l.CustomerID === CustomerID);
            if (isTruthy(index)) {
                this._addServiceTypeService.IsAdd$.next(true);
                this.AssignedLenders.push(this.AllLenders[index]);
                if (this.allLenderSearchval !== null) {
                    const assignLenderValue = this.allLenderSearchval;
                    this.allLenderSearchval = '';
                    this.AllLenders.splice(index, 1);
                    this.allLenderSearchval = assignLenderValue;
                } else {
                    this.AllLenders.splice(index, 1);
                }
                this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
            }
        }
    }

    UnassignedLenderFilterSearch(search) {
        this.AllLenders = this._lenderSearch.ServiceTypeLenderFiltersearch(
            search,
            this.FilterAllLenders,
            this.AssignedLenders
        );
    }

    setLenderSelected(index: number) {
        const clist = this._allLoanChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._allLoanChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._allLoanChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    setLenderAssignSelected(index: number) {
        const clist = this._assignLoanChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._assignLoanChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._assignLoanChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    RemoveLender(CustomerID: number) {
        const index = this.AssignedLenders.findIndex(l => l.CustomerID === CustomerID);
        if (index !== undefined) {
            this.AllLenders.push(this.AssignedLenders[index]);
            if (this.assignedLenderSearchval !== null) {
                const assignLenderValue = this.assignedLenderSearchval;
                this.assignedLenderSearchval = '';
                this.AssignedLenders.splice(index, 1);
                this.assignedLenderSearchval = assignLenderValue;
            } else {
                this.AssignedLenders.splice(index, 1);
            }
            this._addServiceTypeService.setLenders(this.AssignedLenders, this.AllLenders);
        }
    }

    ngOnDestroy(): void {
        this.dragService.destroy('draglender');
        this.subscriptions.forEach((ele) => {
            ele.unsubscribe();
        });
    }
    //#endregion Public Methods

    //#region Private Methods
    private onDropModel(args) {
        const CustomerID = args.item.CustomerID;
        if (args.target.getAttribute('DivType') === 'Assigned') {
            args.el.classList.add('DropHighlight');
            this.AddLender(CustomerID);
        } else if (args.target.getAttribute('DivType') === 'UnAssigned') {
            this.RemoveLender(CustomerID);
        }
    }
    //#endregion Private Methods

}
