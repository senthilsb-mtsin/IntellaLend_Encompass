import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { Subject } from 'rxjs';

const jwtHelper = new JwtHelperService();

export class LenderSearchService {
    LenderSourceObj = [];
    LenderDestObj = [];
    CurrentLoanType$ = new Subject<string>();
    loanRetainConfirm$ = new Subject<boolean>();

    ServiceTypeLenderFiltersearch(search, _sourceobj, AssignedLenders) {
        this.LenderSourceObj = [];
        if (search === undefined || search === '') {
            let _allunassignlender = [];
            if (AssignedLenders.length > 0) {
                _sourceobj.forEach((element) => {
                    const index = AssignedLenders.findIndex(
                        (x) => x.CustomerID === element.CustomerID
                    );
                    if (index === undefined || index === -1) {
                        _allunassignlender.push(element);
                    }
                });
            } else if (AssignedLenders.length === 0) {
                _allunassignlender = _sourceobj;
            }
            this.LenderSourceObj = _allunassignlender;
        } else {
            const _unassLenders = [];
            _sourceobj.filter(function (lender) {
                let istru;
                if (lender.CustomerName !== undefined) {
                    istru = lender.CustomerName.toLowerCase().includes(search.toLowerCase());
                }
                if (istru === true) {
                    _unassLenders.push(lender);
                }
            });
            AssignedLenders.forEach((element) => {
                const index = _unassLenders.findIndex(
                    (x) => x.CustomerID === element.CustomerID
                );
                if (index !== undefined && index !== -1) {
                    _unassLenders.splice(index, 1);
                }
            });
            this.LenderSourceObj = _unassLenders;
        }
        return this.LenderSourceObj;
    }
}
