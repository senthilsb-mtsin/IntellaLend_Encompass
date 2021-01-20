import { DatePipe } from '@angular/common';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { AppSettings, FormulaBuilderTypesConstant } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subscription } from 'rxjs';
import { ChecklistItemRowData } from 'src/app/modules/loantype/models/checklist-items-table.model';
import { RuleCheckValidation } from 'src/app/modules/loantype/pipes';
import { AddLoanTypeService } from 'src/app/modules/loantype/service/add-loantype.service';
import { RuleBuilderService } from 'src/app/modules/loantype/service/rule-builder.service';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
    selector: 'mts-datediff-formula-builder',
    styleUrls: ['datediff-formula-builder.page.css'],
    templateUrl: 'datediff-formula-builder.page.html',
    providers: [RuleCheckValidation, DatePipe]
})
export class DateDiffFormulaBuilderComponent implements OnInit, OnDestroy {
    rulesFrmGrp: FormGroup;
    docFieldMasters: any[] = [];
    genDocTypes: any[] = [];
    RuleOperator: any[] = FormulaBuilderTypesConstant.GeneralRuleOperators.slice();

    FieldErrorMsg = 'The value entered needs to match exactly with the selected Field value';
    isErrMsgs = false;
    ErrorMsg = '';
    currtDocFields: any[] = [];

    LosDocumentFields: any[] = [];
    FannieMaeDocName: string = AppSettings.RuleFannieMaeDocName;

    rowData: ChecklistItemRowData = new ChecklistItemRowData();
    waitingForData = true;
    dOptions: any = {
        singleDatePicker: true,
        showDropdowns: false,
        opens: 'left',
        alwaysShowCalendars: false
    };
    constructor(
        private _ruleBuilderService: RuleBuilderService,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _checkrule: RuleCheckValidation,
        private _fb: FormBuilder,
        private _datePipe: DatePipe
    ) {
        this._ruleBuilderService.setRuleFormGroup('datediff');
    }

    private _subscriptions: Subscription[] = [];
    private operator: any[] = FormulaBuilderTypesConstant.Operators.slice();

    get formData() { return this.rulesFrmGrp.get('datediffRule') as FormArray; }

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
                this.waitingForData = !this.waitingForData;

                if (this.formData.length === 0) {
                    this.addRules();
                }

                if (isTruthy(this.rowData.RuleJsonObject) && isTruthy(this.rowData.RuleJsonObject.datediffRule) && this.rowData.RuleJsonObject.datediffRule.length > 0) {
                    this.GenerateFormValues();
                }
            }));

        this._ruleBuilderService.getSysLoanTypeDocuments();

        this._subscriptions.push(this.rulesFrmGrp.valueChanges.subscribe((form) => {
            let str = '';
            let validate = '';
            let ruleFormationValues = 'datedif([Rule])[Validate]';
            let datediffRuleOperatorCheck = false;

            form.datediffRule.forEach(elements => {
                let fromField = '', toField = '', fromDocType = '', toDocType = '';
                if (elements.fromDateDocTypes !== '' && elements.ToDateDocumentTypes !== '' && (elements.fromDate !== '' || elements.FromDateLosdocField !== '') && elements.toDate !== '' && elements.dateOperator !== '' && elements.resultField !== '') {
                    if (typeof (elements.fromDate) === 'string' && elements.fromDateDocTypes !== AppSettings.RuleFannieMaeDocName) {
                        fromDocType = elements.fromDateDocTypes;
                        fromField = elements.fromDate;
                        toField = elements.toDate !== '' ? elements.toDate : elements.ToDateLosdocField;
                    } else if (Array.isArray(elements.fromDate) && elements.fromDate.length > 0 && elements.fromDateDocTypes !== AppSettings.RuleFannieMaeDocName) {
                        fromDocType = elements.fromDateDocTypes;
                        fromField = elements.fromDate[0].text;
                        toField = elements.toDate[0].text !== '' ? elements.toDate[0].text : elements.ToDateLosdocField[0].text;
                    } else if (typeof (elements.FromDateLosdocField) === 'string' && elements.fromDateDocTypes === AppSettings.RuleFannieMaeDocName) {
                        fromDocType = AppSettings.FannieMaeDocDisplayName;
                        fromField = elements.FromDateLosdocField;
                        toField = elements.toDate !== '' ? elements.toDate : elements.ToDateLosdocField;
                    } else if (Array.isArray(elements.FromDateLosdocField) && elements.FromDateLosdocField.length > 0 && elements.fromDateDocTypes === AppSettings.RuleFannieMaeDocName) {
                        fromDocType = AppSettings.FannieMaeDocDisplayName;
                        fromField = elements.FromDateLosdocField[0].text;
                        toField = elements.toDate[0].text !== '' ? elements.toDate[0].text : elements.ToDateLosdocField[0].text;
                    }
                    str += '[' + fromDocType + '.' + fromField + ']' + ',' + '[' + elements.ToDateDocumentTypes + '.' + toField + ']';
                } else if (elements.datevalueDocField !== '' && elements.ToDateDocumentTypes !== '' && (elements.toDate !== '' || elements.ToDateLosdocField !== '') && elements.dateOperator !== '' && elements.resultField !== '') {
                    if (typeof (elements.toDate) === 'string' && elements.ToDateDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                        toDocType = elements.ToDateDocumentTypes;
                        toField = elements.toDate;
                    } else if (Array.isArray(elements.toDate) && elements.toDate.length > 0 && elements.ToDateDocumentTypes !== AppSettings.RuleFannieMaeDocName) {
                        toDocType = elements.ToDateDocumentTypes;
                        toField = elements.toDate[0].text;
                    } else if (typeof (elements.ToDateLosdocField) === 'string' && elements.ToDateDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                        toDocType = AppSettings.FannieMaeDocDisplayName;
                        toField = elements.ToDateLosdocField;
                    } else if (Array.isArray(elements.ToDateLosdocField) && elements.ToDateLosdocField.length > 0 && elements.ToDateDocumentTypes === AppSettings.RuleFannieMaeDocName) {
                        toDocType = AppSettings.FannieMaeDocDisplayName;
                        toField = elements.ToDateLosdocField[0].text;
                    }

                    str += elements.datevalueDocField + ',' + '[' + toDocType + '.' + toField + ']';
                } else if (elements.toDatevalueDocField !== '' && elements.fromDateDocTypes !== '' && (elements.fromDate !== '' || elements.FromDateLosdocField !== '') && elements.dateOperator !== '' && elements.resultField !== '') {
                    if (typeof (elements.fromDate) === 'string' && elements.fromDateDocTypes !== AppSettings.RuleFannieMaeDocName) {
                        fromDocType = elements.fromDateDocTypes;
                        fromField = elements.fromDate;
                    } else if (Array.isArray(elements.fromDate) && elements.fromDate.length > 0 && elements.fromDateDocTypes !== AppSettings.RuleFannieMaeDocName) {
                        fromDocType = elements.fromDateDocTypes;
                        fromField = elements.fromDate[0].text;
                    } else if (typeof (elements.FromDateLosdocField) === 'string' && elements.fromDateDocTypes === AppSettings.RuleFannieMaeDocName) {
                        fromDocType = AppSettings.FannieMaeDocDisplayName;
                        fromField = elements.FromDateLosdocField;
                    } else if (Array.isArray(elements.FromDateLosdocField) && elements.FromDateLosdocField.length > 0 && elements.fromDateDocTypes === AppSettings.RuleFannieMaeDocName) {
                        fromDocType = AppSettings.FannieMaeDocDisplayName;
                        fromField = elements.FromDateLosdocField[0].text;
                    }

                    str += '[' + fromDocType + '.' + fromField + ']' + ',' + elements.toDatevalueDocField;
                } else if (elements.datevalueDocField !== '' && elements.toDatevalueDocField !== '' && elements.dateOperator !== '' && elements.resultField !== '') {
                    str += elements.datevalueDocField + ',' + elements.toDatevalueDocField;
                } else {
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }

                validate += elements.dateOperator + elements.resultField;
            });
            if (str !== '') {
                datediffRuleOperatorCheck = this._checkrule.transform(str, this.operator);
                ruleFormationValues = this._ruleBuilderService.replaceDateDiffFormula(ruleFormationValues, str, validate);
                if (datediffRuleOperatorCheck) {
                    this.rowData.RuleDescription = ruleFormationValues;
                    this._commonRuleBuilderService.ruleBuilderNext.next(false);
                } else if (!datediffRuleOperatorCheck) {
                    this.ErrorMsg = 'Rule must not end with an Operator, please check the Rule';
                    this._commonRuleBuilderService.ruleBuilderNext.next(true);
                }
            }
            this._commonRuleBuilderService.ruleExpression.next(ruleFormationValues);
        }));
        this._subscriptions.push(this._ruleBuilderService.LosDocumentFields.subscribe((elements: any[]) => {

            this.LosDocumentFields = [];
            elements.forEach((element) => {
                this.LosDocumentFields.push(element);
            });
        }));
    }

    GenerateFormValues() {
        let index = 0;
        this.rowData.RuleJsonObject.datediffRule.forEach(element => {
            if (element.fromDateValue === '' || element.fromDateValue === false) {
                this.formData.controls[index].get('fromDateDocTypes').setValue(element.fromDateDocTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[index], this.currtDocFields, element.fromDateDocTypes, 'fromDate', 0);
                if (isTruthy(element.fromDate)) {
                    this.formData.controls[index].get('fromDate').setValue(element.fromDate[0].id);
                }
                if (isTruthy(element.FromDateLosdocField)) {
                    this.formData.controls[index].get('FromDateLosdocField').setValue(element.FromDateLosdocField);
                }
                this.formData.controls[index].get('dateOperator').setValue(element.dateOperator);
                this.formData.controls[index].get('resultField').setValue(element.resultField);
            }
            if (element.datefieldsCustomValues === '' || element.datefieldsCustomValues === false) {
                this.formData.controls[index].get('ToDateDocumentTypes').setValue(element.ToDateDocumentTypes);
                this._ruleBuilderService.docFieldsInitChange(this.formData.controls[index], this.currtDocFields, element.ToDateDocumentTypes, 'toDate', 1);
                if (isTruthy(element.toDate)) {
                    this.formData.controls[index].get('toDate').setValue(element.toDate[0].id);
                }
                if (isTruthy(element.ToDateLosdocField)) {
                    this.formData.controls[index].get('ToDateLosdocField').setValue(element.ToDateLosdocField);
                }
                this.formData.controls[index].get('dateOperator').setValue(element.dateOperator);
                this.formData.controls[index].get('resultField').setValue(element.resultField);
            }
            if (element.datefieldsCustomValues === true) {
                this.formData.controls[index].get('datefieldsCustomValues').setValue(element.datefieldsCustomValues);
                this.formData.controls[index].get('toDatevalueDocField').setValue(element.toDatevalueDocField);
                this.formData.controls[index].get('dateOperator').setValue(element.dateOperator);
                this.formData.controls[index].get('resultField').setValue(element.resultField);
            }
            if (element.fromDateValue === true) {
                this.formData.controls[index].get('fromDateValue').setValue(element.fromDateValue);
                this.formData.controls[index].get('datevalueDocField').setValue(element.datevalueDocField);
                this.formData.controls[index].get('dateOperator').setValue(element.dateOperator);
                this.formData.controls[index].get('resultField').setValue(element.resultField);
            }
            index++;
        });
    }

    GetEditFromDateValue(vals: any) {
        const fromdatevalue = this._datePipe.transform(vals.start, AppSettings.dateFormat);
        this.formData.controls[0].get('datevalueDocField').setValue(fromdatevalue);
    }

    GetEditToDateValue(vals: any) {
        const todatevalue = this._datePipe.transform(vals.start, AppSettings.dateFormat);
        this.formData.controls[0].get('toDatevalueDocField').setValue(todatevalue);
    }

    DateEditFieldsChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('toDatevalueDocField').setValue('');
            this.formData.controls[index].get('datefieldsCustomValues').setValue(true);
            $('#todatevalueDisplayed_' + index).show();
            $('#todatevalueDisplay_' + index).hide();
            this.formData.controls[index].get('toDate').setValue('');
            this.formData.controls[index].get('ToDateLosdocField').setValue('');
            this.formData.controls[index].get('ToDateDocumentTypes').setValue('');
        } else {
            this.formData.controls[index].get('datefieldsCustomValues').setValue(false);
            $('#todatevalueDisplay_' + index).show();
            $('#todatevalueDisplayed_' + index).hide();
            this.formData.controls[index].get('toDatevalueDocField').setValue('');
        }
    }

    FromEditDateChange(vals: any, index: any) {
        if (vals.currentTarget.checked === true) {
            this.isErrMsgs = true;
            this.formData.controls[index].get('datevalueDocField').setValue('');
            this.formData.controls[index].get('fromDateValue').setValue(true);
            $('#datevalueDisplayed_' + index).show();
            $('#datevalueDisplay_' + index).hide();
            this.formData.controls[index].get('fromDate').setValue('');
            this.formData.controls[index].get('FromDateLosdocField').setValue('');
            this.formData.controls[index].get('fromDateDocTypes').setValue('');
        } else {
            this.formData.controls[index].get('fromDateValue').setValue(false);
            $('#datevalueDisplay_' + index).show();
            $('#datevalueDisplayed_' + index).hide();
            this.formData.controls[index].get('datevalueDocField').setValue('');
        }
    }

    DocTypesChanged(genVals: any, i: any, field: string) {
        const DocName: string = ((typeof (genVals) === 'string') ? genVals : genVals.target.selectedOptions[0].innerText);
        if (DocName !== AppSettings.RuleFannieMaeDocName) {
            this._ruleBuilderService.docFieldsInitChange(this.formData.controls[0], this.currtDocFields, genVals, field, i);
        }
    }

    addRules() {
        this.formData.push(this._fb.group({
            fromDateValue: [''],
            fromDateDocTypes: [''],
            fromDate: [''],
            datevalueDocField: [''],
            datefieldsCustomValues: [''],
            ToDateDocumentTypes: [''],
            toDate: [''],
            toDatevalueDocField: [''],
            dateOperator: [''],
            resultField: [''],
            FromDateLosdocField: [''],
            ToDateLosdocField: ['']
        }));
    }

    OnChangeFieldValue(index: number, LodDocField: string, DocType: string) {
        const SearchValue = this.formData.controls[index].get(LodDocField).value;
        const LosDocumentName = this.formData.controls[index].get(DocType).value;
        let LosDocumentId;
        this.genDocTypes.forEach((a) => {
            if (a.text === LosDocumentName) {
                LosDocumentId = a.id;
            }
        });
        this._ruleBuilderService.GetLosDocFields(LosDocumentId, SearchValue);
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
