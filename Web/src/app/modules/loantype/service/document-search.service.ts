export class DocumentSearchService {
  DocSourceObj = [];
  DocDestObj = [];

  DocFiltersearch(search, _sourceobj, docStackGrouping, docstacking) {
    this.DocSourceObj = [];
    if (search === undefined || search === '') {
      const _allunassigndoc = [];
      if (docStackGrouping.length > 0) {
        _sourceobj.forEach((element) => {
          const index = docStackGrouping.findIndex(
            (x) => x.DocumentTypeID === element.DocumentTypeID
          );
          if (index === undefined || index === -1) {
            _allunassigndoc.push(element);
          }
        });
      }
      if (docstacking.length > 0) {
        _sourceobj.forEach((element) => {
          const index = docstacking.findIndex(
            (x) => x.DocumentTypeID === element.DocumentTypeID
          );
          if (index === undefined || index === -1) {
            _allunassigndoc.push(element);
          }
        });
      }
      this.DocSourceObj = _allunassigndoc;
    } else {
      const _unassDocs = [];
      _sourceobj.filter(function (docType) {
        let istru;
        if (docType.DocumentTypeName !== undefined) {
          istru = docType.DocumentTypeName.toLowerCase().includes(
            search.toLowerCase()
          );
        }
        if (istru === true) {
          _unassDocs.push(docType);
        }
      });

      docStackGrouping.forEach((element) => {
        const index = _unassDocs.findIndex(
          (x) => x.DocumentTypeID === element.DocumentTypeID
        );
        if (index !== undefined && index !== -1) {
          _unassDocs.splice(index, 1);
        }
      });
      docstacking.forEach((element) => {
        const index = _unassDocs.findIndex(
          (x) => x.DocumentTypeID === element.DocumentTypeID
        );
        if (index !== undefined && index !== -1) {
          _unassDocs.splice(index, 1);
        }
      });
      this.DocSourceObj = _unassDocs;
    }
    return this.DocSourceObj;
  }

  LoanFiltersearch(search, _sourceobj, AssignedLoanTypes) {
    this.DocSourceObj = [];
    if (search === undefined || search === '') {
      let _allunassigndoc = [];

      if (AssignedLoanTypes.length > 0) {
        _sourceobj.forEach((element) => {
          const index = AssignedLoanTypes.findIndex(
            (x) => x.LoanTypeID === element.LoanTypeID
          );
          if (index === undefined || index === -1) {
            _allunassigndoc.push(element);
          }
        });
      } else if (AssignedLoanTypes.length === 0) {
        _allunassigndoc = _sourceobj;
      }
      this.DocSourceObj = _allunassigndoc;
    } else {
      const _unassDocs = [];
      _sourceobj.filter(function (loanType) {
        let istru;
        if (loanType.LoanTypeName !== undefined) {
          istru = loanType.LoanTypeName.toLowerCase().includes(
            search.toLowerCase()
          );
        }
        if (istru === true) {
          _unassDocs.push(loanType);
        }
      });
      AssignedLoanTypes.forEach((element) => {
        const index = _unassDocs.findIndex(
          (x) => x.LoanTypeID === element.LoanTypeID
        );
        if (index !== undefined && index !== -1) {
          _unassDocs.splice(index, 1);
        }
      });
      this.DocSourceObj = _unassDocs;
    }
    return this.DocSourceObj;
  }

  LoanTypeDocFiltersearch(search, _sourceobj, AssignedDocTypes) {
    this.DocSourceObj = [];
    if (search === undefined || search === '') {
      let _allunassigndoc = [];
      if (AssignedDocTypes.length > 0) {
        _sourceobj.forEach((element) => {
          const index = AssignedDocTypes.findIndex(
            (x) => x.DocumentTypeID === element.DocumentTypeID
          );
          if (index === undefined || index === -1) {
            _allunassigndoc.push(element);
          }
        });
      } else if (AssignedDocTypes.length === 0) {
        _allunassigndoc = _sourceobj;
      }
      this.DocSourceObj = _allunassigndoc;
    } else {
      const _unassDocs = [];
      _sourceobj.filter(function (docType) {
        let istru;
        if (docType.Name !== undefined) {
          istru = docType.Name.toLowerCase().includes(search.toLowerCase());
        }
        if (istru === true) {
          _unassDocs.push(docType);
        }
      });
      AssignedDocTypes.forEach((element) => {
        const index = _unassDocs.findIndex(
          (x) => x.DocumentTypeID === element.DocumentTypeID
        );
        if (index !== undefined && index !== -1) {
          _unassDocs.splice(index, 1);
        }
      });
      this.DocSourceObj = _unassDocs;
    }
    return this.DocSourceObj;
  }
}
