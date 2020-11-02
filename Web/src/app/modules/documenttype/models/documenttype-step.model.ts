export class AddDocumentTypeStepModel {
    stepID = 0;
    stepOneClass = '';
    stepTwoClass = '';
  constructor(_stepID: number, _stepOneClass: string, _stepTwoClass: string) {
        this.stepID = _stepID;
        this.stepOneClass = _stepOneClass;
        this.stepTwoClass = _stepTwoClass;
    }
}
