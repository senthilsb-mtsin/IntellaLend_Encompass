import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
    name: 'checkListItemNamePipe'
})

export class CheckListItemNamePipe implements PipeTransform {
    transform(values: any[], checklistname: any, checklistgroupId: any): any {

        if (values !== null && checklistname !== null && checklistgroupId !== null) {
            for (let i = 0; i < values.length; i++) {
                if (values[i].id === checklistgroupId && values[i].name.toLowerCase() === checklistname.toLowerCase()) {
                    return false;
                }
            }
            return true;
        }
    }
}
