import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'loanFilter'
})
export class LoanFilterPipe implements PipeTransform {

  transform(loans: any, search: any): any {
    if (search === undefined) { return loans; }

    if (search === '') { return loans; }

    return loans.filter(function (loan) {
      if (loan.LoanNumber !== undefined) {
        return loan.LoanNumber.toLowerCase().includes(search.toLowerCase());
      }
    });

  }

}
