import { Injectable } from '@angular/core';
import { LoanSearchTableModel } from '../../loansearch/models/loan-search-table.model';

export class LoanService {

    private _loanPageInfo: LoanSearchTableModel;

    setLoanPageInfo(loanData: LoanSearchTableModel) {
        this._loanPageInfo = loanData;
    }

}
