export class LoanTypeWizardStepModel {
    stepID = 0;
    stepOneClass = '';
    stepTwoClass = '';
    stepThreeClass = '';
    stepFourClass = '';

    constructor(_stepID: number, _stepOneClass: string, _stepTwoClass: string, _stepThreeClass: string, _stepFourClass: string) {
        this.stepID = _stepID;
        this.stepOneClass = _stepOneClass;
        this.stepTwoClass = _stepTwoClass;
        this.stepThreeClass = _stepThreeClass;
        this.stepFourClass = _stepFourClass;
    }
}
