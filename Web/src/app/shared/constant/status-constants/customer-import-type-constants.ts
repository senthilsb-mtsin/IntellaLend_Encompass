import { AppSettings } from '@mts-app-setting';

export class CustomerImportTypeConstants {
    static CustomerImport = 0;
    static CustomerMapping = 1;

    static StatusColor = {
        '0': 'label-info',
        '1' : 'label-primary',
        '2' : 'label-warning',
        '3' : 'label-success',
        '-1' : 'label-danger'
    };

    static TypeDropdown = [
        CustomerImportTypeConstants.CustomerImport,
        CustomerImportTypeConstants.CustomerMapping,
    ];
    static Type = {
        '0': AppSettings.AuthorityLabelSingular + ' Import',
        '1': AppSettings.AuthorityLabelSingular + ' Mapping'
    };
}
