export class CustomerWizardStepModel {
    stepID = 0;
    stepOneClass = '';
    stepTwoClass = '';
    stepThreeClass = '';
    stepFourClass = '';
    stepFiveClass = '';
    stepSixClass = '';

    constructor(_stepID: number, _stepOneClass: string, _stepTwoClass: string, _stepThreeClass: string, _stepFourClass: string, _stepFiveClass: string, _stepSixClass: string) {
        this.stepID = _stepID;
        this.stepOneClass = _stepOneClass;
        this.stepTwoClass = _stepTwoClass;
        this.stepThreeClass = _stepThreeClass;
        this.stepFourClass = _stepFourClass;
        this.stepFiveClass = _stepFiveClass;
        this.stepSixClass = _stepSixClass;
    }
}
