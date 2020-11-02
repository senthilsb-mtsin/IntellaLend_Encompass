import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dupeditchecklistname',
    pure: true
})

export class CheckDuplicateName implements PipeTransform {
    transform(values: any[], oldName: any, newName: any) {
        if (values.length > 0 && oldName !== null && oldName !== undefined && oldName !== '' && newName !== null && newName !== undefined && newName !== '') {
            if (oldName.toLowerCase() === newName.toLowerCase()) {
                return false;
            } else if (oldName.toLowerCase() !== newName.toLowerCase()) {
                for (let i = 0; i < values.length; i++) {
                    if (values[i].toLowerCase() === newName.toLowerCase()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
