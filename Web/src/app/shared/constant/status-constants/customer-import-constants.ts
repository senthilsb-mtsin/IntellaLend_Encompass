export class CustomerImportStatusConstants {
    static Processing = 1;
    static PartiallyCompleted = 2;
    static Completed = 3;
    static Error = -1;

    static Description = {
        '0' : 'Staged',
        '1' : 'Processing',
        '2' : 'Partially Completed',
        '3' : 'Completed',
        '-1' : 'Error'
    };

    static StatusColor = {
        '0': 'label-info',
        '1' : 'label-primary',
        '2' : 'label-warning',
        '3' : 'label-success',
        '-1' : 'label-danger'
    };

    static StatusDropdown = [
        CustomerImportStatusConstants.Processing,
        CustomerImportStatusConstants.PartiallyCompleted,
        CustomerImportStatusConstants.Completed,
        CustomerImportStatusConstants.Error
    ];
}
