import { Component, OnInit, OnDestroy, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { LoanTypeSearchService } from '../../service/loantype-search.service';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { DragulaService } from '@mts-dragula';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-assign-loan-type',
    styleUrls: ['assign-loan-type.component.css'],
    templateUrl: 'assign-loan-type.component.html',
    providers: [LoanTypeSearchService],
})
export class AssignLoanTypesComponent implements OnInit, OnDestroy {
    //#region Public Variables
    AllLoanTypes: any[] = [];
    AssignedLoanTypes: any[] = [];
    FilterAllLoanTypes: any[] = [];
    allLoanSearchval = '';
    assignedLoanSearchval = '';

    @ViewChildren('allLoans') _allLoanChildrens: QueryList<ElementRef>;
    @ViewChildren('assignedLoan') _assignLoanChildrens: QueryList<ElementRef>;
    //#endregion Public Variables

    //#region Constructor
    constructor(
        private _addServiceTypeService: AddServiceTypeService,
        private _loantypeSearch: LoanTypeSearchService,
        private dragService: DragulaService,
    ) { }
    //#endregion Constructor

    //#region  Private Variables
    private subscriptions: Subscription[] = [];
    //#endregion Private Variables

    //#region  Public Methods
    ngOnInit(): void {
        this.resetPageData();
        this.dragService.dropModel().subscribe((value) => {
            this.onDropModel(value);
        });
    }

    resetPageData() {
        this.AllLoanTypes = this._addServiceTypeService.getAllLoanTypes();
        this.AssignedLoanTypes = this._addServiceTypeService.getAssignedLoanTypes();
        this.FilterAllLoanTypes = this._addServiceTypeService.getAllLoanTypes();
    }

    MoveFromAllLoan() {
        for (let i = 0; i < this._allLoanChildrens['_results'].length; i++) {
            const clist = this._allLoanChildrens['_results'][i].nativeElement.classList.toString();
            const loanTypeID = parseInt(this._allLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
            const index = this.AllLoanTypes.findIndex(l => l.LoanTypeID === loanTypeID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this.AssignedLoanTypes.push(this.AllLoanTypes[index]);
                this.AllLoanTypes.splice(index, 1);
                this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
            }
        }
    }

    MoveToAllLoan() {
        for (let i = 0; i < this._assignLoanChildrens['_results'].length; i++) {
            const clist = this._assignLoanChildrens['_results'][i].nativeElement.classList.toString();
            const loanTypeID = parseInt(this._assignLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
            const index = this.AssignedLoanTypes.findIndex(l => l.LoanTypeID === loanTypeID);
            if (clist.indexOf('SelectHighlight') > -1) {
                this.AllLoanTypes.push(this.AssignedLoanTypes[index]);
                this.AssignedLoanTypes.splice(index, 1);
                this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
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
                const loanID = parseInt(this._allLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
                const index = this.AllLoanTypes.findIndex(l => l.LoanTypeID === loanID);
                if (clist.indexOf('SelectHighlight') > -1) {
                    this.AssignedLoanTypes.push(this.AllLoanTypes[index]);
                    this.AllLoanTypes.splice(index, 1);
                    this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
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
                const loanTypeID = parseInt(this._assignLoanChildrens['_results'][i].nativeElement.attributes['data-loanid'].value, 10);
                const index = this.AssignedLoanTypes.findIndex(l => l.LoanTypeID === loanTypeID);
                if (clist.indexOf('SelectHighlight') > -1) {
                    this.AllLoanTypes.push(this.AssignedLoanTypes[index]);
                    this.AssignedLoanTypes.splice(index, 1);
                    this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
                }
            }
        }
    }

    AddLoanType(loanID: number) {
        if (loanID > 0) {
            const index = this.AllLoanTypes.findIndex(l => l.LoanTypeID === loanID);
            if (isTruthy(index)) {
                this.AssignedLoanTypes.push(this.AllLoanTypes[index]);
                if (this.allLoanSearchval !== null) {
                    const assignLoanValue = this.allLoanSearchval;
                    this.allLoanSearchval = '';
                    this.AllLoanTypes.splice(index, 1);
                    this.allLoanSearchval = assignLoanValue;
                } else {
                    this.AllLoanTypes.splice(index, 1);
                }
                this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
            }
        }
    }

    UnassignedLoanFilterSearch(search) {
        this.AllLoanTypes = this._loantypeSearch.ServiceTypeLoanFiltersearch(
            search,
            this.FilterAllLoanTypes,
            this.AssignedLoanTypes
        );
    }

    setLoanSelected(index: number) {
        const clist = this._allLoanChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._allLoanChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._allLoanChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    setLoanAssignSelected(index: number) {
        const clist = this._assignLoanChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
            this._assignLoanChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
            this._assignLoanChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
    }

    RemoveLoanType(loanID: number) {
        const index = this.AssignedLoanTypes.findIndex(l => l.LoanTypeID === loanID);
        if (index !== undefined) {
            this.AllLoanTypes.push(this.AssignedLoanTypes[index]);
            if (this.assignedLoanSearchval !== null) {
                const assignLoanValue = this.assignedLoanSearchval;
                this.assignedLoanSearchval = '';
                this.AssignedLoanTypes.splice(index, 1);
                this.assignedLoanSearchval = assignLoanValue;
            } else {
                this.AssignedLoanTypes.splice(index, 1);
            }
            this._addServiceTypeService.setLoanTypes(this.AssignedLoanTypes, this.AllLoanTypes);
        }
    }

    ngOnDestroy(): void {
        this.dragService.destroy('dragloantype');
        this.subscriptions.forEach((element) => {
            element.unsubscribe();
        });
    }
    //#endregion Public Methods

    //#region Private Methods
    private onDropModel(args) {
        const loanId = args.item.LoanTypeID;
        if (args.target.getAttribute('DivType') === 'Assigned') {
            args.el.classList.add('DropHighlight');
            this.AddLoanType(loanId);
        } else if (args.target.getAttribute('DivType') === 'UnAssigned') {
            this.RemoveLoanType(loanId);
        }
    }
    //#endregion Private Methods

}
