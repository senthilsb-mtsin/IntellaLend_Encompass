import { Pipe, PipeTransform, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
@Pipe({
    name: 'documentsearch'
})
export class DocumentsearchPipe implements PipeTransform {
    transform(value: any, searchText: any[]): any {
        if (searchText[0] === undefined && searchText[1] === '') { return value; }
        if (searchText[1] !== '') {
            if (searchText[0] === undefined || searchText[0] === '') {
                return value.filter(function (srval) {
                    if (searchText[1] !== undefined && (searchText[1] === '401' || searchText[1] === '403' || searchText[1] === '404' || searchText[1] === '407')) {
                        return (srval.IDCStatus.toLowerCase()) === (searchText[1].toLowerCase());
                    }
                    if (searchText[1] === 'Obsolete') {
                        return ((srval.DocumentLevel.toLowerCase()) === 'in loan' && srval.Obsolete === true);
                    } else if (searchText[1] === 'In Loan') {
                        return ((srval.DocumentLevel.toLowerCase()) === 'in loan' && srval.Obsolete === false);
 } else if (searchText[1] !== undefined) {
                        return (srval.DocumentLevel.toLowerCase()) === (searchText[1].toLowerCase());
 }
                });
            } else if (searchText[0] !== '') {
                return value.filter(function (srval) {
                    if (searchText[0] !== undefined && (searchText[1] === '401' || searchText[1] === '403' || searchText[1] === '404' || searchText[1] === '407')) {
                        return ((srval.IDCStatus.toLowerCase()) === (searchText[1].toLowerCase()) && srval.DocName.toLowerCase().includes(searchText[0].toLowerCase()));
                    } else {
                        return (srval.DocumentLevel.toLowerCase()) === (searchText[1].toLowerCase()) && srval.DocName.toLowerCase().includes(searchText[0].toLowerCase());
                    }
                });
            }
        }

        if (searchText[1] === '' && (searchText[0] === undefined || searchText[0] === '')) { return value; }
        return value.filter(function (srval) {
            if (searchText[1] === '' && searchText[0] !== '' && searchText[0] !== undefined) {
                return srval.DocName.toLowerCase().includes(searchText[0].toLowerCase());
            }
        });
    }

}

@NgModule({
    declarations: [DocumentsearchPipe], // <---
    imports: [CommonModule],
    exports: [DocumentsearchPipe] // <---
})
export class DocumentsearchPipeModule { }
