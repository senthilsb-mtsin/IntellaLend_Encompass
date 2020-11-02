import { Component, OnInit, OnDestroy } from '@angular/core';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-manual-formula-builder',
    styleUrls: ['manual-formula-builder.page.css'],
    templateUrl: 'manual-formula-builder.page.html',
})
export class ManualFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];

    ErrorMsg = '';
    inputArray: any[] = [{ id: '1', value: 'CheckBox' }, { id: '2', value: 'RadioButton' }];
    isCreateBtnEnabled = true;
    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    itemsNotes: any[] = [];
    showMention = true;
    constructor(
        private _ruleBuilderService: RuleBuilderService,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _fb: FormBuilder
    ) {
        this._ruleBuilderService.setRuleFormGroup('manual');
    }

    private _subscriptions: Subscription[] = [];
    private _counterValues = 1;
    private _counter = 1;

    get formData() { return this.rulesFrmGrp.get('manualGroup') as FormArray; }

    get formDataQuestionsTypes() { return this.formData.controls[0].get('QuestionsTypes') as FormArray; }

    get formDataCheckBoxChoices() { return this.formData.controls[0].get('CheckBoxChoices') as FormArray; }

    get formDataRadioChoices() { return this.formData.controls[0].get('RadioChoices') as FormArray; }

    ngOnInit(): void {
        this.rowData = this._commonRuleBuilderService.getEditChecklistItem();
        this.rulesFrmGrp = this._ruleBuilderService.getRuleFormGroup();
        this._subscriptions.push(this._ruleBuilderService.LoanTypeDocuments.subscribe(
            (res: {
                Documents: { id: number, text: string }[],
                Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
                DataTables: { DocID: number, TableName: string, ColumnName: string }[]
            }) => {
                this.genDocTypes = res.Documents;
                this.docFieldMasters = res.Fields;
                this.waitingForData = false;

                if (this.formDataQuestionsTypes.length === 0) {
                    this.addRules();
                }

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.manualGroup) && this.rowData.RuleJsonObject.manualGroup.length > 0) {
                    this.GenerateFormValues();
                    this.isCreateBtnEnabled = false;
                }
            }));

        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let ruleFormationValues = '[Rule]';
            let str = '';
            form.manualGroup.forEach(elts => {
                this._commonRuleBuilderService.ruleBuilderNext.next(elts.QuestionsTypes[0].inputtypes === 0);

                if (elts.QuestionsTypes[0].question !== '') {
                    str += elts.QuestionsTypes[0].question;
                }

                if (elts.QuestionsTypes[0].inputtypes === '1') {
                    if (elts.CheckBoxChoices.length > 1) {
                        elts.CheckBoxChoices.forEach(el => {
                            this._commonRuleBuilderService.ruleBuilderNext.next(!(el.checkboxoptions !== ''));
                        });
                    } else { this._commonRuleBuilderService.ruleBuilderNext.next(true); }
                } else if (elts.QuestionsTypes[0].inputtypes === '2' && elts.RadioChoices.length > 1) {
                    if (elts.RadioChoices.length > 1) {
                        elts.RadioChoices.forEach(eltz => {
                            this._commonRuleBuilderService.ruleBuilderNext.next(!(eltz.radiooptions !== ''));
                        });
                    } else { this._commonRuleBuilderService.ruleBuilderNext.next(true); }
                }
            });
            if (str === '') {
                this._commonRuleBuilderService.ruleBuilderNext.next(true);
            } else {
                ruleFormationValues = this._ruleBuilderService.replaceFormula(ruleFormationValues, str);
            }
            this.rowData.RuleDescription = ruleFormationValues;
            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));
    }

    GenerateFormValues() {
        this.rowData.RuleJsonObject.manualGroup.forEach(element => {
            if (element.QuestionsTypes.length > 0) {
                this.formDataQuestionsTypes.controls[0].get('choicenumber').setValue(element.QuestionsTypes[0]['choicenumber']);
                this.formDataQuestionsTypes.controls[0].get('inputtypes').setValue(element.QuestionsTypes[0]['inputtypes']);
                this.formDataQuestionsTypes.controls[0].get('question').setValue(element.QuestionsTypes[0]['question']);
                this.formDataQuestionsTypes.controls[0].get('isNotesEnabled').setValue(element.QuestionsTypes[0]['isNotesEnabled']);
                if (element.QuestionsTypes[0]['inputtypes'] === '1') {
                    let i = 0;
                    element.CheckBoxChoices.forEach(elt => {
                        this.addCheckBoxElements();
                        if (elt.checkboxoptions.length > 0) {
                            this.formDataCheckBoxChoices.controls[i].get('checkboxoptions').setValue(elt.checkboxoptions);
                        }

                        i++;
                    });
                } else if (element.QuestionsTypes[0]['inputtypes'] === '2') {
                    let j = 0;
                    element.RadioChoices.forEach(elts => {
                        this.adRadioOptionsElements();
                        if (elts.radiooptions.length > 0) {
                            this.formDataRadioChoices.controls[j].get('radiooptions').setValue(elts.radiooptions);
                        }
                        j++;
                    });
                }
            }
        });

    }

    changeManualDocs(even: any) {
        this.showMention = false;
        const notes = this.formDataQuestionsTypes.controls[0].get('question').value;
        const _manualnotes = (notes !== '' && notes !== 'undefined') ? notes.match(/(?=\[).+?(?=\])/g) : [];
        if (_manualnotes !== null && _manualnotes.length !== 0 && _manualnotes.length - 1 === this.genDocTypes.length) { } else {
            if (_manualnotes !== null && _manualnotes.length > 0) {
                const _ReplacedList = [];
                _manualnotes.forEach(ele =>
                    _ReplacedList.push(ele.replace('[', ''))
                );
                const tempArry = [];
                this.itemsNotes = [];
                this.genDocTypes.forEach(ele => {
                    if (_ReplacedList.indexOf(ele.text) === -1) {
                        tempArry.push(ele.text);
                    }
                });
                this.itemsNotes = tempArry;
            } else {
                this.itemsNotes = this.genDocTypes.map(x => x.text).slice();
            }
        }
        this.showMention = true;
    }

    createElements() {
        const manualEditQST = this.formDataQuestionsTypes.controls[0].get('question').value;
        const getSelectedInputTypes = this.formDataQuestionsTypes.controls[0].get('inputtypes').value;

        if (isTruthy(manualEditQST) && getSelectedInputTypes !== '0') {
            let elementsLength = this.formDataQuestionsTypes.controls[0].get('choicenumber').value;
            const elementTypes = this.formDataQuestionsTypes.controls[0].get('inputtypes').value;
            if (elementsLength === '') {
                elementsLength = '1';
            }

            if (elementTypes === '1') {
                this.formDataCheckBoxChoices.clear();
                this.formDataRadioChoices.clear();
                for (let i = 0; i < parseInt(elementsLength, 10); i++) {
                    this.addCheckBoxElements();
                }
            } else if (elementTypes === '2') {
                this.formDataCheckBoxChoices.clear();
                this.formDataRadioChoices.clear();
                for (let i = 0; i < parseInt(elementsLength, 10); i++) {
                    this.adRadioOptionsElements();
                }
            }
        }
    }

    increment() {
        if (this.formDataQuestionsTypes.controls[0].get('choicenumber').value >= 1 || this.formDataQuestionsTypes.controls[0].get('choicenumber').value === '') {
            if (this._counter >= 1 && this._counterValues < 5) {
                if (this.formDataQuestionsTypes.controls[0].get('choicenumber').value < 5) {
                    this._counter = this.formDataQuestionsTypes.controls[0].get('choicenumber').value;
                    this._counterValues = ++this._counter;
                    this.formDataQuestionsTypes.controls[0].get('choicenumber').setValue(this._counterValues);
                } else if (this.formDataQuestionsTypes.controls[0].get('choicenumber').value > 5) {
                    this._counterValues = 5;
                    this._counter = 5;
                    this.formDataQuestionsTypes.controls[0].get('choicenumber').setValue(this._counterValues);
                } else { this._counterValues = ++this._counter; }
            }
        }
    }

    decrement() {
        if (this._counter !== 0 && this._counterValues > 1) {
            if (this.formDataQuestionsTypes.controls[0].get('choicenumber').value > 1) {
                this._counter = this.formDataQuestionsTypes.controls[0].get('choicenumber').value;
                this._counterValues = --this._counter;
                this.formDataQuestionsTypes.controls[0].get('choicenumber').setValue(this._counterValues);
            } else { this._counterValues = --this._counter; }
        } else {
            this._counterValues = 1;
            this._counter = 1;
            this.formDataQuestionsTypes.controls[0].get('choicenumber').setValue(this._counterValues);
        }
    }

    OnManualChange() {
        const manualEditQST = this.formDataQuestionsTypes.controls[0].get('question').value;
        const getSelectedInputTypes = this.formDataQuestionsTypes.controls[0].get('inputtypes').value;
        this.isCreateBtnEnabled = !(isTruthy(manualEditQST) && getSelectedInputTypes !== '0');
    }

    addRules() {
        this.formDataQuestionsTypes.push(this._fb.group({
            question: [''],
            inputtypes: ['1'],
            choicenumber: [{ value: '', disabled: true }],
            isNotesEnabled: ['']
        }));
    }

    addCheckBoxElements() {
        this.formDataCheckBoxChoices.push(this._fb.group({
            checkboxoptions: ['']
        }));
    }

    adRadioOptionsElements() {
        this.formDataRadioChoices.push(this._fb.group({
            radiooptions: ['']
        }));
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
