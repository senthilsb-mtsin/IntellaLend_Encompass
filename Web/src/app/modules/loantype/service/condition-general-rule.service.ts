import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { Subject } from 'rxjs';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { LoanDataAccess } from '../loantype.data';
import { AssignDocumentsRuleRowData, ConditionObject } from '../models/assign-docs-rule.model';
import { AssignDocumentTypeRequestModel } from '../models/assign-document-types-request.model';
import { AddLoanTypeService } from './add-loantype.service';
import { LoanTypeService } from './loantype.service';

const jwtHelper = new JwtHelperService();

@Injectable()
export class ConditionGeneralRuleService {

    _currentDocID: any = 0;
    RuleFormGroup = new Subject<FormGroup>();
    LoanTypeDocuments = new Subject<{
        Documents: { id: number, text: string }[],
        Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
        DataTables: { DocID: number, TableName: string, ColumnName: string }[]
    }>();
AssignDocSaved = new Subject<boolean>();
InitializeForm = new Subject<boolean>();
ConditionObjcet = new Subject<ConditionObject>();
ruleSave = new Subject<boolean>();
// GeneralRuleclose = new Subject<boolean>();
GeneralRuleModalShow = new Subject<boolean>();
ruleFormula = new Subject<string>();
ruleDesc: string;
_loanType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean };
ConditionValues: FormGroup;
      _docmappingDetails: [];
constructor( private _formBuilder: FormBuilder, private _notificationService: NotificationService,
    private _loanTypeData: LoanDataAccess, private _commonRuleBuilderService: CommonRuleBuilderService,
    private _loantypeService: LoanTypeService

    ) {

}
private _rulesFormGrp: FormGroup;
private RowData: AssignDocumentsRuleRowData;
private _loanTypeDocumentFields: { DocID: number, FieldID: number, Name: string, DocName: string }[] = [];
private _loanTypeDocuments: { id: number, text: string }[] = [];
private _loanTypeDocumentDataTable: { DocID: number, TableName: string, ColumnName: string }[] = [];
    setRuleFormGroup() {
        this._rulesFormGrp = this._formBuilder.group({
            formula: '',
            schema: this._formBuilder.array([]),
        });
    }

    getRuleFormGroup() {
        this.ConditionValues = this._rulesFormGrp;
        return this._rulesFormGrp;

    }
    SaveDocMapping(req: AssignDocumentTypeRequestModel) {
        this._loanTypeData.SaveDocMapping(req).subscribe(res => {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
              this.AssignDocSaved.next(true);
          } else {
            this.AssignDocSaved.next(false);

          }
        });
      }
    GetCustLoanDocMapping(_docID: number, AssignedDocTypes: any) {
         for (let i = 0; i < AssignedDocTypes.length ; i++) {
                if (AssignedDocTypes[i].DocumentTypeID === _docID ) {
                    if (isTruthy(AssignedDocTypes[i].Condition)) {
                        this.ConditionObjcet.next(JSON.parse(AssignedDocTypes[i].Condition));
                        this.GeneralRuleModalShow.next(true);
                        this.ruleSave.next(false);
                        break;
                    } else {
                        this.InitializeForm.next(true);
                        this.GeneralRuleModalShow.next(true);
                        this.ruleSave.next(true);

                    }

                }
            }

            }

    setDocumentTypeID(_docId: number) {
        this._currentDocID = _docId;
    }
    getCurrentDoctypeID() {
        return this._currentDocID;
    }

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
}
