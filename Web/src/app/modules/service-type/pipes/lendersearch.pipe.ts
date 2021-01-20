import { Pipe, PipeTransform } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Pipe({
    name: 'lendersearch'
})
export class LenderSearchPipe implements PipeTransform {
    transform(loanTypes: any, search: any): any {
        if (search === undefined) { return loanTypes; }
        if (search === '') { return loanTypes; }
        return loanTypes.filter(function (loanType) {
          if (isTruthy(loanType.CustomerName)) {
            return loanType.CustomerName.toLowerCase().includes(search.toLowerCase());
          }
        //   if (isTruthy(loanType.DocumentTypeName)) {
        //     return loanType.DocumentTypeName.toLowerCase().includes(search.toLowerCase());
        //   }
        //   if (isTruthy(loanType.DisplayName)) {
        //     return loanType.DisplayName.toLowerCase().includes(search.toLowerCase());
        //   }
        });

      }

}
