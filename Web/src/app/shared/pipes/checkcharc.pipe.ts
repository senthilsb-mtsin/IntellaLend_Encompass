import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
    name: 'charcpipe'
})

export class CharcCheckPipe implements PipeTransform {

    transform(value: any): any {
        if (value === undefined) {
            return false;
        }

        if (value === '') { return false; }

        const exp = new RegExp('^[a-zA-Z]');

        return exp.test(value);
    }

}
