import { Pipe, PipeTransform } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Pipe({ name: 'mappingSearch' })
export class MappingSearchPipe implements PipeTransform {

    transform(loanTypes: any[], search: any) {
        if (search === undefined) { return loanTypes; }
        if (search === '') { return loanTypes; }
        return loanTypes.filter(function (loanType) {
            if (isTruthy(loanType.LoanTypeName)) {
                return loanType.LoanTypeName.toLowerCase().includes(search.toLowerCase());
            }
            if (isTruthy(loanType.LoanTypeName)) {
                return loanType.LoanTypeName.toLowerCase().includes(search.toLowerCase());
            }
        });
    }

}
