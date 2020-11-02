import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'checkrule'
})

export class RuleCheckValidation implements PipeTransform {
    transform(values: string, operatorArr: any[]): any {
        let lastChar = '';
        let charBeforeComma = '';
        charBeforeComma = values.substring(0, values.indexOf(','));
        if (values !== null) {
            if (charBeforeComma !== '') {
                lastChar = charBeforeComma.charAt(charBeforeComma.length - 1);
            } else {
                lastChar = values.charAt(values.length - 1);
            }
            for (let i = 0; i < operatorArr.length; i++) {
                if (operatorArr[i] === lastChar) {
                    return false;
                }
            }
        }
        return true;
    }
}
