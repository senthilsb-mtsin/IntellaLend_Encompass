import { Pipe, PipeTransform } from '@angular/core';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Pipe({
  name: 'docmastersearch'
})
export class DocMasterSearchPipe implements PipeTransform {
  transform(docTypes: any, search: any): any {
    if (search === undefined) { return docTypes; }
    if (search === '') { return docTypes; }
    return docTypes.filter(function (docType) {
      if (isTruthy(docType.Name)) {
        return docType.Name.toLowerCase().includes(search.toLowerCase());
      }
      if (isTruthy(docType.DocumentTypeName)) {
        return docType.DocumentTypeName.toLowerCase().includes(search.toLowerCase());
      }
      if (isTruthy(docType.DisplayName)) {
        return docType.DisplayName.toLowerCase().includes(search.toLowerCase());
      }
    });

  }

}
