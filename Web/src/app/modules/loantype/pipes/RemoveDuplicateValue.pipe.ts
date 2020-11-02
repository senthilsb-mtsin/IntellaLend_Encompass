import { Pipe, PipeTransform } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Pipe({
    name: 'removeduplicate',
    pure: true
})

export class RemoveDuplicateValues implements PipeTransform {
    transform(values: any, prop: any) {
        if (isTruthy(values)) {
            const result = [];
            let filterVal, filters;
            const filterByVal = function (n) {
                if (n[prop] === filterVal) { return true; }
            };
            for (let i = 0; i < values.length; i++) {
                filterVal = values[i][prop];
                filters = result.filter(filterByVal);
                if (filters.length === 0) { result.push(values[i]); }
            }
            return result;
        }
        return values;
    }
}
