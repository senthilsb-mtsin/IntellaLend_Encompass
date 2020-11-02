import { Injectable } from '@angular/core';
import { AddLoanTypeService } from './add-loantype.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { Subject } from 'rxjs';
import { LoanDataAccess } from '../loantype.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotificationService } from '@mts-notification';
import { SaveChecklistItem, SaveRuleMasters } from '../models/save-checklist-item-request.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { FormulaBuilderTypesConstant } from '@mts-app-setting';
import { SessionHelper } from '@mts-app-session';
import { JsonPipe } from '@angular/common';
import { RemoveDuplicateValues } from '../pipes/RemoveDuplicateValue.pipe';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

const jwtHelper = new JwtHelperService();

@Injectable()
export class RuleBuilderService {
    RuleFormGroup = new Subject<FormGroup>();
    LoanTypeDocuments = new Subject<{
        Documents: { id: number, text: string }[],
        Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
        DataTables: { DocID: number, TableName: string, ColumnName: string }[]
    }>();
    EvalResult = new Subject<{
        TestResult: boolean,
        TestResultExp: string,
        ErrorMsg: string
    }>();

    constructor(
        private _addLoanTypeService: AddLoanTypeService,
        private _commonRuleBuilderService: CommonRuleBuilderService,
        private _fb: FormBuilder,
        private _loanTypeData: LoanDataAccess,
        private _notificationService: NotificationService,
        private _jsonPipe: JsonPipe,
        private _removeDuplicatePipe: RemoveDuplicateValues) {

    }

    private _loanTypeID = this._addLoanTypeService.getCurrentLoanTypeID();
    private _rulesFrmGrp: FormGroup;
    private _loanTypeDocuments: { id: number, text: string }[] = [];
    private _loanTypeDocumentFields: { DocID: number, FieldID: number, Name: string, DocName: string }[] = [];
    private _loanTypeDocumentDataTable: { DocID: number, TableName: string, ColumnName: string }[] = [];

    replaceFormula(ruleFormationValues: string, str: string) {
        return ruleFormationValues && ruleFormationValues.replace('[Rule]', str);
    }

    replaceDateDiffFormula(ruleFormationValues: string, str: string, Validate: string) {
        return ruleFormationValues && ruleFormationValues.replace('[Rule]', str).replace('[Validate]', Validate);
    }

    replaceIfFormula(ruleFormationValues, str: string, trueString: any, falseString: any) {
        let result = ruleFormationValues && ruleFormationValues.replace('[condition]', str);
        result = result && result.replace('[true]', trueString);
        result = result && result.replace('[false]', falseString);
        return result;
    }

    docFieldsInitChange(control: any, currtDocFields: any[], vals: any, field: string, index: number) {
        const tempGeneralEditDocFieldMasters = [];
        control.get(field).setValue('');
        let newVals = '';
        let newValNotEmpty = false;
        if (typeof (vals) !== 'string') {
            if (vals.currentTarget.value !== '' && vals.currentTarget.value !== 'Select') {
                newValNotEmpty = true;
                newVals = vals.target.selectedOptions[0].innerText.replace(/[\s]/g, '');
            }
        } else {
            if (vals !== '' && vals !== 'Select') {
                newValNotEmpty = true;
                newVals = vals.replace(/[\s]/g, '');
            }
        }

        if (newValNotEmpty) {
            for (let i = 0; i < this._loanTypeDocumentFields.slice().length; i++) {
                const trmmiedSpaceDocName = this._loanTypeDocumentFields.slice()[i].DocName.replace(/[\s]/g, '');
                if (newVals === trmmiedSpaceDocName) {
                    tempGeneralEditDocFieldMasters.push(this._loanTypeDocumentFields.slice()[i].Name);
                }
            }
            currtDocFields[index] = tempGeneralEditDocFieldMasters;
        } else if (vals === '' || vals === 'Select') {
            control.get(field).setValue('');
            currtDocFields[index] = tempGeneralEditDocFieldMasters;
        }
        if (tempGeneralEditDocFieldMasters.length === 0) {
            this._notificationService.showError('Fields Unavailable');
        }
    }

    setRuleFormGroup(ruleType: string) {
        this._rulesFrmGrp = this._fb.group({
            mainOperator: ruleType,
            generalRule: this._fb.array([]),
            conditionalRule: this._fb.array([this.initConditionalRules()]),
            inRule: this._fb.array([]),
            datediffRule: this._fb.array([]),
            manualGroup: this._fb.array([this.initManualGroups()]),
            isEmptyRule: this._fb.array([]),
            compareAllRule: this._fb.array([]),
            isNotEmptyRule: this._fb.array([]),
            docCheckAll: this._fb.array([]),
            docIsExist: this._fb.array([]),
            losRule: this._fb.array([]),
            datatableRule: this._fb.array([]),
            groupby: this._fb.array([])
        });
    }

    getRuleFormGroup() {
        return this._rulesFrmGrp;
    }

    setComparsionEvalDocs(_rule: FormArray, _docTypeProperty: string, _fieldProperty: string, _disabledField: string = '') {
        let _evalFields = [];
        const regex = /\[(.+?)\]/g;
        const arr = [];
        let matchValues;
        while (matchValues = regex.exec(_rule.controls[0].get(_disabledField).value)) {
            arr.push(matchValues[1]);
        }
        for (let i = 0; i < arr.length; i++) {
            _evalFields.push({ DocName: arr[i].split('.')[0], Fieldname: arr[i].split('.')[1], ComparisionFields: '[' + arr[i].split('.')[0] + '.' + arr[i].split('.')[1] + ']', originalField: '' });
        }
        if (_rule.controls[0].get(_docTypeProperty).value !== '' && _rule.controls[0].get(_fieldProperty).value !== '') {
            const dcsValues = [{ DocName: _rule.controls[0].get(_docTypeProperty).value, Fieldname: _rule.controls[0].get(_fieldProperty).value, ComparisionFields: '[' + _rule.controls[0].get(_docTypeProperty).value + '.' + _rule.controls[0].get(_fieldProperty).value + ']', originalField: '' }];
            Array.prototype.push.apply(dcsValues, _evalFields);
            _evalFields = this._removeDuplicatePipe.transform(dcsValues, 'ComparisionFields');
        } else {
            _evalFields = this._removeDuplicatePipe.transform(_evalFields, 'ComparisionFields');
        }
        return _evalFields;
    }

    setEvalDocs(_rule: FormArray, _docTypeProperty: string, _fieldProperty: string, _disabledField: string = '') {
        const _evalFields = [];
        for (let z = 0; z < _rule.length; z++) {
            if (_rule.controls[z].get(_fieldProperty).value !== '' && _rule.controls[z].get(_docTypeProperty).value !== '') {
                _evalFields.push({ DocName: _rule.controls[z].get(_docTypeProperty).value, Fieldname: _rule.controls[z].get(_fieldProperty).value, originalField: '', ComparisionFields: '[' + _rule.controls[z].get(_docTypeProperty).value + '.' + _rule.controls[z].get(_fieldProperty).value + ']' });
            }
        }
        return _evalFields;
    }

    setIfEvalDocs(_rule: FormArray, _docTypeProperty: string, _fieldProperty: string, _disabledField: string = '') {
        const _evalFields = [];
        const _formMetaData = [{
            jObjectName: 'ConditionalExtraFields',
            formFields: {
                docField: 'ifdocField',
                docTypeField: 'IfDocumentTypes'
            }
        }, {
            jObjectName: 'TrueConditionalExtraFields',
            formFields: {
                docField: 'trueDocField',
                docTypeField: 'TrueDocumentTypes'
            }
        }, {
            jObjectName: 'FalseConditionalExtraFields',
            formFields: {
                docField: 'falseDocField',
                docTypeField: 'FalseDocumentTypes'
            }
        }];

        for (let index = 0; index < 3; index++) {
            const rulType = _formMetaData[index];
            const _form = _rule.controls[0].get(rulType.jObjectName) as FormArray;
            for (let i = 0; i < _form.controls.length; i++) {
                if (_form.controls[i].get(rulType.formFields.docTypeField).value !== '' && _form.controls[i].get(rulType.formFields.docField).value) {
                    _evalFields.push({ DocName: _form.controls[i].get(rulType.formFields.docTypeField).value, Fieldname: _form.controls[i].get(rulType.formFields.docField).value, ComparisionFields: '[' + _form.controls[i].get(rulType.formFields.docTypeField).value + '.' + _form.controls[i].get(rulType.formFields.docField).value + ']', originalField: '' });
                }
            }
        }
        return _evalFields;
    }

    setDateDiffEvalDocs(_rule: FormArray, _docTypeProperty: string, _fieldProperty: string, _disabledField: string = '') {
        const evalDateDiffDocTypes = [];
        const evalDateDiffToDocTypes = [];
        let distinctEditDateDiffEvalDocTypes = [];
        if (_rule.controls[0].get('fromDateDocTypes').value !== '' && _rule.controls[0].get('ToDateDocumentTypes').value !== '') {
            evalDateDiffDocTypes.push({ DocName: _rule.controls[0].get('fromDateDocTypes').value, Fieldname: _rule.controls[0].get('fromDate').value, originalField: '', ComparisionFields: '[' + _rule.controls[0].get('fromDateDocTypes').value + '.' + _rule.controls[0].get('fromDate').value + ']' });
            evalDateDiffToDocTypes.push({ DocName: _rule.controls[0].get('ToDateDocumentTypes').value, Fieldname: _rule.controls[0].get('toDate').value, originalField: '', ComparisionFields: '[' + _rule.controls[0].get('ToDateDocumentTypes').value + '.' + _rule.controls[0].get('toDate').value + ']' });
            Array.prototype.push.apply(evalDateDiffDocTypes, evalDateDiffToDocTypes);
            distinctEditDateDiffEvalDocTypes = this._removeDuplicatePipe.transform(evalDateDiffDocTypes, 'ComparisionFields');
        } else if (_rule.controls[0].get('fromDateDocTypes').value !== '') {
            evalDateDiffDocTypes.push({ DocName: _rule.controls[0].get('fromDateDocTypes').value, Fieldname: _rule.controls[0].get('fromDate').value, originalField: '', ComparisionFields: '[' + _rule.controls[0].get('fromDateDocTypes').value + '.' + _rule.controls[0].get('fromDate').value + ']' });
            distinctEditDateDiffEvalDocTypes = this._removeDuplicatePipe.transform(evalDateDiffDocTypes, 'ComparisionFields');
        } else if (_rule.controls[0].get('ToDateDocumentTypes').value !== '') {
            evalDateDiffToDocTypes.push({ DocName: _rule.controls[0].get('ToDateDocumentTypes').value, Fieldname: _rule.controls[0].get('toDate').value, originalField: '', ComparisionFields: '[' + _rule.controls[0].get('ToDateDocumentTypes').value + '.' + _rule.controls[0].get('toDate').value + ']' });
            distinctEditDateDiffEvalDocTypes = this._removeDuplicatePipe.transform(evalDateDiffToDocTypes, 'ComparisionFields');
        }
        return distinctEditDateDiffEvalDocTypes;
    }

    getSysLoanTypeDocuments() {
        if (this._loanTypeDocuments.length > 0) {
            this.LoanTypeDocuments.next(
                {
                    Documents: this._loanTypeDocuments.slice(),
                    Fields: this._loanTypeDocumentFields.slice(),
                    DataTables: this._loanTypeDocumentDataTable.slice()
                }
            );
        } else {
            this.getSysLoanTypeDocs();
        }
    }

    EvaluateRules(req: { RuleFormula: string, CheckListItemValues: any }) {
        this._loanTypeData.EvaluateRules(req).subscribe(res => {
            if (res !== null) {
                const Result = jwtHelper.decodeToken(res.Data)['data'];
                if (Result !== null) {
                    this.EvalResult.next({
                        TestResult: Result.Result,
                        TestResultExp: Result.EvalExp,
                        ErrorMsg: Result.Message
                    });
                } else {
                    this._notificationService.showError('Error Contact Administrator');
                }
            }
        });
    }

    SaveChecklistItem() {
        const rowData = this._commonRuleBuilderService.getEditChecklistItem();
        const ruleType = FormulaBuilderTypesConstant.FormulaTypes.slice().filter(x => x.Name === rowData.RuleJsonObject.mainOperator)[0];
        const checklistdetailsmasters = new SaveChecklistItem();
        checklistdetailsmasters.CheckListID = rowData.ChecklistGroupId;
        checklistdetailsmasters.Name = rowData.CheckListName;
        checklistdetailsmasters.Description = rowData.CheckListDescription;
        checklistdetailsmasters.Category = rowData.Category;
        checklistdetailsmasters.UserID = SessionHelper.UserDetails.UserID;
        checklistdetailsmasters.Active = rowData.ChecklistActive;
        checklistdetailsmasters.LOSFieldToEvalRule = rowData.LOSFieldToEvalRule;
        checklistdetailsmasters.LOSValueToEvalRule = rowData.LOSValueToEvalRule;
        checklistdetailsmasters.LosIsMatched = rowData.LosMatched ? 1 : 0;
        checklistdetailsmasters.RuleType = ruleType.Rule_Type;
        checklistdetailsmasters.Rule_Type = ruleType.Rule_Type;
        const rulemasters = new SaveRuleMasters();
        rulemasters.DocVersion = rowData.DocVersion;
        rulemasters.RuleDescription = rowData.RuleDescription;
        rulemasters.Active = true;

        if (rowData.CheckListDetailID > 0 && rowData.RuleID > 0) {
            checklistdetailsmasters.CheckListDetailID = rowData.CheckListDetailID;
            rulemasters.RuleID = rowData.RuleID;
        }

        const jsonData = JSON.parse(this._jsonPipe.transform(this._rulesFrmGrp.getRawValue()));
        if (isTruthy(jsonData[ruleType.RuleJsonName]) && jsonData[ruleType.RuleJsonName].length > 0) {
            const distinctGenDocIds = this.GetDocsMapped(jsonData, ruleType);
            FormulaBuilderTypesConstant.FormulaTypes.slice().forEach(element => {
                if (element.RuleJsonName !== ruleType.RuleJsonName) {
                    jsonData[element.RuleJsonName] = [];
                }
            });
            rulemasters.RuleJson = this._jsonPipe.transform(jsonData);
            const distGenDocArr = [];
            distinctGenDocIds.forEach(elt => {
                distGenDocArr.push(elt.DocIds);
            });
            rulemasters.ActiveDocumentType = distGenDocArr.join(',');
            rulemasters.DocumentType = distGenDocArr.join(',');

            return this.SaveChecklistItemObject({ TableSchema: this._commonRuleBuilderService.getTenantSchema(), CheckListDetailMaster: checklistdetailsmasters, RuleMasters: rulemasters, LoanTypeID: this._commonRuleBuilderService.getCurrentLoanTypeID() });
        } else {
            this._commonRuleBuilderService.EnableRuleBuilder.next(false);
            this._notificationService.showError('Error Contact Administrator');
        }
    }

    private GetDocsMapped(jsonData: any, ruleType: any) {
        const genDocIDS = [];
        const genDocIDS2 = [];
        let distinctGenDocIds = [];
        const ifObjects = [
            { Type: 'ConditionalExtraFields', DocumentType: 'IfDocumentTypes', DocField: 'ifdocField' },
            { Type: 'TrueConditionalExtraFields', DocumentType: 'TrueDocumentTypes', DocField: 'trueDocField' },
            { Type: 'FalseConditionalExtraFields', DocumentType: 'FalseDocumentTypes', DocField: 'falseDocField' }
        ];
        if (ruleType.RuleJsonName === 'conditionalRule') {
            ifObjects.forEach(element => {
                jsonData[ruleType.RuleJsonName][0][element.Type].forEach(ele => {
                    if (isTruthy(ele[element.DocField])) {
                        ele[element.DocField] = [{ id: ele[element.DocField], text: ele[element.DocField] }];
                    }
                });
                for (let i = 0; i < jsonData[ruleType.RuleJsonName][0][element.Type].length; i++) {
                    for (let j = 0; j < this._loanTypeDocuments.length; j++) {
                        if (jsonData[ruleType.RuleJsonName][0][element.Type][i][element.DocumentType] === this._loanTypeDocuments[j].text) {
                            genDocIDS.push({ DocIds: this._loanTypeDocuments[j].id });
                        }
                    }
                }
            });
        } else {
            jsonData[ruleType.RuleJsonName].forEach(element => {
                if (isTruthy(element[ruleType.SaveFieldName])) {
                    element[ruleType.SaveFieldName] = [{ id: element[ruleType.SaveFieldName], text: element[ruleType.SaveFieldName] }];
                }
                if (isTruthy(element[ruleType.SaveFieldName2])) {
                    element[ruleType.SaveFieldName2] = [{ id: element[ruleType.SaveFieldName2], text: element[ruleType.SaveFieldName2] }];
                }
                if (isTruthy(element[ruleType.SaveFieldName3])) {
                    element[ruleType.SaveFieldName3] = [{ id: element[ruleType.SaveFieldName3], text: element[ruleType.SaveFieldName3] }];
                }
                if (isTruthy(element[ruleType.SaveFieldName4])) {
                    element[ruleType.SaveFieldName4] = [{ id: element[ruleType.SaveFieldName4], text: element[ruleType.SaveFieldName4] }];
                }
            });
            for (let i = 0; i < jsonData[ruleType.RuleJsonName].length; i++) {
                for (let j = 0; j < this._loanTypeDocuments.length; j++) {
                    if (isTruthy(ruleType.AddUpdateField) && jsonData[ruleType.RuleJsonName][i][ruleType.AddUpdateField] === this._loanTypeDocuments[j].text) {
                        genDocIDS.push({ DocIds: this._loanTypeDocuments[j].id });
                    }
                    if (isTruthy(ruleType.AddUpdateField2) && jsonData[ruleType.RuleJsonName][i][ruleType.AddUpdateField2] === this._loanTypeDocuments[j].text) {
                        genDocIDS2.push({ DocIds: this._loanTypeDocuments[j].id });
                    }
                }
            }
        }
        Array.prototype.push.apply(genDocIDS, genDocIDS2);
        distinctGenDocIds = this._removeDuplicatePipe.transform(genDocIDS, 'DocIds');

        return distinctGenDocIds;
    }

    private SaveChecklistItemObject(req: { TableSchema: string, CheckListDetailMaster: SaveChecklistItem, RuleMasters: SaveRuleMasters, LoanTypeID: number }) {
        return this._loanTypeData.SaveChecklistItemObject(req).subscribe(res => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result !== null) {
                this._commonRuleBuilderService.RuleAdded.next(true);
                this._notificationService.showSuccess('Checklist Item Created Successfully');
            } else {
                this._notificationService.showError('Error Contact Administrator');
            }
            this._commonRuleBuilderService.EnableRuleBuilder.next(false);
        });
    }

    private getSysLoanTypeDocs() {
        const req = { TableSchema: this._commonRuleBuilderService.getTenantSchema(), customerID: this._commonRuleBuilderService.getCurrentCustomer(), loanTypeID: this._commonRuleBuilderService.getCurrentLoanTypeID(), LoanTypeID: this._commonRuleBuilderService.getCurrentLoanTypeID() };
        this._loanTypeData.GetSysLoanTypeDocuments(req).subscribe((res) => {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            this._loanTypeDocuments = [];
            this._loanTypeDocumentFields = [];
            this._loanTypeDocumentDataTable = [];
            Result.forEach(element => {
                this._loanTypeDocuments.push({ id: element.DocumentTypeID, text: element.Name });
            });
            for (let i = 0; i < Result.length; i++) {
                for (let j = 0; j < Result[i].DocumentFieldMasters.length; j++) {
                    this._loanTypeDocumentFields.push({ DocID: Result[i].DocumentFieldMasters[j].DocumentTypeID, FieldID: Result[i].DocumentFieldMasters[j].FieldID, Name: Result[i].DocumentFieldMasters[j].Name, DocName: Result[i].Name });
                }
                for (let j = 0; j < Result[i].RuleDocumentTables.length; j++) {
                    this._loanTypeDocumentDataTable.push({ DocID: Result[i].RuleDocumentTables[j].DocumentID, TableName: Result[i].RuleDocumentTables[j].TableName, ColumnName: Result[i].RuleDocumentTables[j].TableColumnName });
                }
            }
            this.LoanTypeDocuments.next(
                {
                    Documents: this._loanTypeDocuments.slice(),
                    Fields: this._loanTypeDocumentFields.slice(),
                    DataTables: this._loanTypeDocumentDataTable.slice()
                }
            );
        });
    }

    private initConditionalRules() {
        return this._fb.group({
            ConditionalExtraFields: this._fb.array([]),
            TrueConditionalExtraFields: this._fb.array([]),
            FalseConditionalExtraFields: this._fb.array([])
        });
    }

    private initManualGroups() {
        return this._fb.group({
            QuestionsTypes: this._fb.array([]),
            CheckBoxChoices: this._fb.array([]),
            RadioChoices: this._fb.array([])
        });
    }
}
