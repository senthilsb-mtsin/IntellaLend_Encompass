export class AssignLoanTypesRequestModel {
    constructor(public ReviewTypeID: number,
        public LoanTypeIDs: number[]) {
    }
}
