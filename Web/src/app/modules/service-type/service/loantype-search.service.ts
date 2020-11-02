export class LoanTypeSearchService {
  LoanTypeSourceObj = [];
  LoanTypeDestObj = [];

  ServiceTypeLoanFiltersearch(search, _sourceobj, AssignedLoanTypes) {
    this.LoanTypeSourceObj = [];
    if (search === undefined || search === '') {
      let _allunassignloan = [];
      if (AssignedLoanTypes.length > 0) {
        _sourceobj.forEach((element) => {
          const index = AssignedLoanTypes.findIndex(
            (x) => x.LoanTypeID === element.LoanTypeID
          );
          if (index === undefined || index === -1) {
            _allunassignloan.push(element);
          }
        });
      } else if (AssignedLoanTypes.length === 0) {
        _allunassignloan = _sourceobj;
      }
      this.LoanTypeSourceObj = _allunassignloan;
    } else {
      const _unassLoans = [];
      _sourceobj.filter(function (loanType) {
        let istru;
        if (loanType.LoanTypeName !== undefined) {
          istru = loanType.LoanTypeName.toLowerCase().includes(search.toLowerCase());
        }
        if (istru === true) {
          _unassLoans.push(loanType);
        }
      });
      AssignedLoanTypes.forEach((element) => {
        const index = _unassLoans.findIndex(
          (x) => x.LoanTypeID === element.LoanTypeID
        );
        if (index !== undefined && index !== -1) {
          _unassLoans.splice(index, 1);
        }
      });
      this.LoanTypeSourceObj = _unassLoans;
    }
    return this.LoanTypeSourceObj;
  }
}
