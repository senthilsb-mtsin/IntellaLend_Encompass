
export class SynchronizeConstant {
  static Staged = 0;
  static Process = 1;
  static Completed = 2;
  static Failed = -1;
  static DefaultVal = 99;

  static AllSync = 0;
  static ChecklistSync = 1;
  static RetrySync = 3;

  static SYNCHRONIZE_DESCRIPTION = {
    '0': 'Synchronize Waiting',
    '1': 'Synchronize Processing',
    '2': 'Completed',
    '-1': 'Synchronize Failed',

  };
  static SYNCHRONIZE_COLOR = {
    '0': 'label-primary',
    '1': 'label label-info',
    '2': 'label label-success',
    '-1': 'label-info',
  };

  static SYNCHRONIZE_ICON = {
    '0': 'label label-primary',
    '1': 'label-info',
    '2': 'label label-success',
    '-1': 'label label-danger',
  };

}
