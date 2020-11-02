import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'singleoperatorexists',
    pure: true
})

export class SingleOperatorExistforRule implements PipeTransform {

    transform(str: any, delimiter: any) {
        if (!(delimiter instanceof Array)) {
            return str.split(delimiter);
        }
        if (!delimiter || delimiter.length === 0) {
            return [str];
        }
        const hashSet = new Set(delimiter);
        if (hashSet.has('')) {
            return str.split('');
        }
        let lastIndex = 0;
        const result = [];
        let count = 0;
        for (let i = 0; i < str.length; i++) {
            if (hashSet.has(str[i])) {
                result.push(str.substring(lastIndex, i));
                lastIndex = i + 1;
                ++count;
            }
        }
        result.push(str.substring(lastIndex));
        const finalResult = [];
        finalResult.push({ Vals: result }, { operatorsCount: count });
        return finalResult;
    }
}
