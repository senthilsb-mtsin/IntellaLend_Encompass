import { Pipe, PipeTransform, NgModule } from '@angular/core';
@Pipe({ name: 'fieldsearch' })
export class FieldsearchPipe implements PipeTransform {
    transform(value: any, searchText: any): any {
        if (searchText === undefined || searchText === '' || searchText.length < 2) {
            return value;
        } else {
            return value.filter(function (srval) {
                if (srval.controls.FieldDisplayName.value.toLowerCase().includes(searchText.toLowerCase())) {
                    return srval;
                }
            });
        }
    }
}

@Pipe({
    name: 'replaceallchar'
})
export class ReplaceAllSpecialCharPipe implements PipeTransform {
    transform(str: any): any {
        return str.replace(/[^\w]/gi, '');
    }
}

@NgModule({
    imports: [],
    declarations: [FieldsearchPipe, ReplaceAllSpecialCharPipe],
    exports: [FieldsearchPipe, ReplaceAllSpecialCharPipe]
})
export class FieldsearchPipeModule { }
