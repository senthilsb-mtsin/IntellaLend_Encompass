export class ExportWizardStepModel {
  stepID = 0;
  stepOneClass = '';
  stepTwoClass = '';
  stepThreeClass = '';
  constructor(_stepID: number, _stepOneClass: string, _stepTwoClass: string = '', _stepThreeClass: string = '') {
    this.stepID = _stepID;
    this.stepOneClass = _stepOneClass;
    this.stepTwoClass = _stepTwoClass;
    this.stepThreeClass = _stepThreeClass;
  }
}
