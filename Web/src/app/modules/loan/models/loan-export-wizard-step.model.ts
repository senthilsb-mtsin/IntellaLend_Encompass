export class LoanExportWizardStepModel {
    stepID: number;
    stepOneClass: string;
    stepTwoClass: string;
    constructor(stepID: number, stepOneClass: string, stepTwoClass: string) {
        this.stepID = stepID;
        this.stepOneClass = stepOneClass;
        this.stepTwoClass = stepTwoClass;
    }
}
