import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserDataAccess } from '../../user/user.data';
import { Subject, Subscription } from 'rxjs';
import { Location } from '@angular/common';
import { ReverificationdetailsModel } from '../models/reverification-details.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ReverificationStepModel } from '../models/reverification-step.model';
import { ReverificationDataAccess } from '../reverification.data';
import { NotificationService } from '@mts-notification';
import { Router } from '@angular/router';
import { FileUploaderOptions } from 'ng2-file-upload';
import { AppSettings } from '@mts-app-setting';
import { environment } from 'src/environments/environment';
import { ReverificationMappingModel } from '../models/reverification-mapping.model';
import { ReverificationValidateModel } from '../models/reverification-validate.model';
import { LoanTemplateField } from '../../loan/models/loan-template-fields.model';
import { TrimspacePipe } from '@mts-pipe';

const jwtHelper = new JwtHelperService();

@Injectable()

export class ReverificationService {
    setNextStep = new Subject<ReverificationStepModel>();
    setReverificationTableData = new Subject<any>();
    ReverificationData = new Subject<any>();
    ManagerReverificationData = new Subject<any>();
    assignedDocuments = new Subject<number[]>();
    TemplateMaster = new Subject<any>();
    AddReverificationSteps: any = {
        Reverification: 1,
        AssignDocTypes: 2
    };
    _mappingTemplateValues: ReverificationMappingModel = new ReverificationMappingModel();
    mentionDropOptions$ = new Subject<{ mentionDocTypes: any, mentionDocFields: any }>();
    isprevious = new Subject<any>();
    _addReverifyData = new Subject<any>();
    _editReverifyData = new Subject<any>();
    AllDocTypes = new Subject<any[]>();
    AssignedDocTypes = new Subject<any[]>();
    _mappingTemplate = new Subject<ReverificationMappingModel>();
    Fields = new Subject<any>();
    AssignedDocFieldTypes = new Subject<any[]>();
    FilterAllDocTypes = new Subject<any[]>();
    setNextStepForLoadDocs = new Subject<boolean>();
    LoanTypeID: any;
    isSetMapping = new Subject<boolean>();
    setNextStepForDocs = false;
    constructor(private _reverificationData: ReverificationDataAccess,
        private location: Location,
        private _route: Router,
        private _notifyservice: NotificationService,
        private trim: TrimspacePipe
    ) {
    }
    private _ReverificationData: ReverificationdetailsModel = new ReverificationdetailsModel();

    private _tempMasters: any[] = [];
    private _stepID: Subscription;
    private _isPrevious = false;
    private _assignedDocuments: any[] = [];
    private assignDocfields: any[] = [];
    private _allDocuments: any[] = [];
    private MappingID = 0;
    private ReverificationID = 0;
    private TemplateID = 0;
    private _fields: LoanTemplateField = new LoanTemplateField();
    private _reverifiData: any = [];
    private _reverifyList: any = [];
    private DocTypeNames: any = [];
    private DocFieldNames: any = [];
    private editReverifyID = 0;
    SetRowData(inputData: ReverificationdetailsModel) {
        this._ReverificationData = inputData;
        this.ReverificationData.next(inputData);

    }
    GetRowdata() {
        this.ReverificationData.next(this._ReverificationData);
    }
    CloseReverification() {
        this.location.back();
    }

    GetLoanDocuments(_req: any) {
        return this._reverificationData.GetLoanDocuments(_req).subscribe(
            res => {
                if (isTruthy(res)) {
                    const result = jwtHelper.decodeToken(res.Data)['data'];
                    this._allDocuments = result.AllDocTypes;
                    this._assignedDocuments = result.AssignedDocTypes;
                    this.assignDocfields = result.AssignedDocFields;
                    this.isprevious.next(true);
                    this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, 'active complete', 'active'));

                }
            });
    }

    GetReverificationTemplate() {
        this._tempMasters = [];
        return this._reverificationData.GetReverificationTemplate().subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data) && data.length > 0) {
                data.forEach(element => {
                    this._tempMasters.push({ TemplateID: element.TemplateID, TemplateName: element.TemplateName });
                });
                this.TemplateMaster.next(this._tempMasters.slice());
            }
        });
    }
    SaveReverifyDocMapping(_req: any) {
        return this._reverificationData.SaveReverifyDocMapping(_req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data)) {
                this.GetMappedTemplate();
                this._notifyservice.showSuccess('Re-Verification Mapping Updated Successfully');

            } else {
                this._notifyservice.showError('Re-Verification Mapping Update Failed');

            }
        });
    }
    SaveTemplate(_req: any) {
        return this._reverificationData.UpdateReverificationTemplate(_req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data)) {
                this._notifyservice.showSuccess('Re-Verification Template Updated Successfully');
                this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, '', 'active complete'));
                this._route.navigate(['view/reverification']);

            } else {
                this._notifyservice.showError('Failed');
                this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, '', 'active complete'));
                this._route.navigate(['view/reverification']);
            }

        });
    }
    GetMappedTemplate() {
        if (this.MappingID !== 0 && this.ReverificationID !== 0 && this.TemplateID !== 0) {
            const req = { MappingID: this.MappingID, TemplateID: this.TemplateID, ReverificationID: this.ReverificationID };
            return this._reverificationData.GetMappedTemplate(req).subscribe(res => {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (isTruthy(data)) {
                    if (isTruthy(data.TemplateFieldValue)) {
                        this._fields = JSON.parse(data.TemplateFieldValue);
                    } else {
                        const tempFields = data.TemplateFields.split(',');
                        tempFields.forEach(element => {
                            this._fields[element.trim()] = '';
                        });
                    }
                    this.Fields.next(this._fields);
                    this._mappingTemplate.next(data);
                    this._mappingTemplateValues = data;
                    this.isSetMapping.next(true);
                }
            });
        } else {
            this._notifyservice.showError('Failed');
        }
    }
    GetMappingTemplateValues() {
        return this._mappingTemplateValues;

    }
    GetReverification() {
        return this._reverificationData.GetReverificationData().subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._reverifiData = data;
            this.setReverificationTableData.next(data);
        });
    }
    AddReverificationSubmit(_reqBody: any) {
        if (_reqBody.ReverificationName !== '' && _reqBody.LoanTypeID !== 0 && _reqBody.TemplateID !== 0) {
            if (!this.IsValidate(_reqBody)) {
                return this._reverificationData.AddReverificationSubmit(_reqBody).subscribe(res => {
                    const data = jwtHelper.decodeToken(res.Data)['data'];
                    if (isTruthy(data) && data.Success) {
                        this._addReverifyData.next(data);
                        const _req = { LoanTypeID: _reqBody.LoanTypeID, ReverificationID: data.ReverificationID };
                        this.MappingID = data.MappingID;
                        this.ReverificationID = data.ReverificationID;
                        this.TemplateID = _reqBody.TemplateID;
                        this.GetLoanDocuments(_req);
                        this._notifyservice.showSuccess('Re-Verification Created Successfully');
                    } else {
                        this._notifyservice.showError('Failed');
                    }
                });
            } else {
                this._notifyservice.showError('Already Exists');
            }
        } else {
            this._notifyservice.showError('LoanType and Re-Verification Required');
        }
    }
    EditReverificationSubmit(_reqBody: any, _loanTypeID: any, _templateID: any) {
        if (_reqBody.ReverificationName !== '' && _reqBody.LoanTypeID !== 0 && _reqBody.TemplateID !== 0 && _reqBody.MappingID !== 0 && _reqBody.ReverificationID !== 0) {
            if (!this.IsValidate(_reqBody)) {
                return this._reverificationData.EditReverificationSubmit(_reqBody).subscribe(res => {
                    const data = jwtHelper.decodeToken(res.Data)['data'];
                    if (isTruthy(data) && data.Success) {
                        this._notifyservice.showSuccess('Re-Verification Updated Successfully');
                        this._editReverifyData.next(data);
                        this.MappingID = data.MappingID;
                        this.ReverificationID = data.ReverificationID;
                        this.editReverifyID = data.ReverificationID;
                        this.TemplateID = _templateID;
                        this.LoanTypeID = _loanTypeID;
                        const _req = { LoanTypeID: _loanTypeID, ReverificationID: data.ReverificationID };
                        this.GetLoanDocuments(_req);

                    } else {
                        this._notifyservice.showError('Re-Verification Update Failed');

                    }
                });
            } else {
                this._notifyservice.showError('Already Exists');
            }
        } else {
            this._notifyservice.showError('LoanType and Re-Verification Required');
        }

    }
    IsValidate(_req: ReverificationValidateModel): boolean {
        if (isTruthy(this._reverifiData)) { return false; }
        if (this._reverifiData.length === 0) { return false; }
        const _reverifyData = this._reverifiData.filter(x => x.LoanTypeID === _req.LoanTypeID && x.ReverificationName.toLowerCase() === _req.ReverificationName.toLowerCase());

        if (_reverifyData.length === 0) { return false; }

        return true;
    }
    SetStepID(stepid: any) {
        this._stepID = stepid;
        this.SetPrevious();
    }

    SetPrevious() {
        this.setNextStepForLoadDocs.next(true);
        this.setNextStepForDocs = true;
        if (this._stepID === this.AddReverificationSteps.AssignDocTypes) {
            this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.Reverification, 'active', ''));
        }
    }
    NextStep() {
        const _req = { LoanTypeID: this.LoanTypeID, ReverificationID: this.ReverificationID };
        this.GetLoanDocuments(_req);

    }
    getAssignedDocTypes() {
        const _docIDs = [];
        this._assignedDocuments.forEach(element => {
            _docIDs.push(element.DocumentTypeID);
        });
        this.assignedDocuments.next(_docIDs);
        return this._assignedDocuments.slice();
    }
    getAssignedDocTypeNames() {
        this.DocTypeNames = [];
        this._assignedDocuments.forEach(element => {
            this.DocTypeNames.push(element.Name);
        });
        return this.DocTypeNames.slice();
    }
    getAssignDocFieldNames() {
        this.DocFieldNames = [];
        this.assignDocfields.forEach(elt => {
            if (this.trim.transform(elt.DocID)) {
                this.DocFieldNames.push(elt.DocFieldName);
            }
        });
        return this.DocFieldNames.slice();
    }
    EditSetNextStep() {
        const _req = { LoanTypeID: this.LoanTypeID, ReverificationID: this.ReverificationID };
        this.GetLoanDocuments(_req);

    }
    CheckreverificationAvailable(_req: any) {
        const input = { ReverificationName: _req.ReverificationName };
        return this._reverificationData.CheckReverificationExistForEdit(JSON.stringify(input)).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data.length === 0) {
                    this.AddReverificationSubmit(_req);
                } else {
                    this._notifyservice.showError('Reverification Name Already Exists');
                }
            }
        });
    }
    getFieldsForTemplate() {
        this.Fields.next(this._fields);
        return this._fields;
    }
    CheckReverificationExistForEdit(_req: any, _loantypeID: any, _templateID: any) {
        const input = { ReverificationName: _req.ReverificationName };
        return this._reverificationData.CheckReverificationExistForEdit(JSON.stringify(input)).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data.length === 1) {
                    if (data[0].ReverificationID === _req.ReverificationID) {
                        this.EditReverificationSubmit(_req, _loantypeID, _templateID);
                    } else {
                        this._notifyservice.showError('Reverification Name Already Exists');

                    }

                } else if (data.length === 0) {
                    this.EditReverificationSubmit(_req, _loantypeID, _templateID);
                } else if (data.length > 1) {
                    this._notifyservice.showError('Reverification Name Already Exists');

                } else {
                    const req = { LoanTypeID: _loantypeID, ReverificationID: this.ReverificationID };
                    this.GetLoanDocuments(req);
                }
            }
        });
    }
    setDocTypes(_assignedDocs: any[], _allDocs: any[]) {
        const _docIDs = [];
        _assignedDocs.forEach(element => {
            _docIDs.push(element.DocumentTypeID);
        });
        this._assignedDocuments = _assignedDocs.slice();
        this._allDocuments = _allDocs.slice();
        this.assignedDocuments.next(_docIDs);
    }

    getAssignedDocFields() {
        this.AssignedDocFieldTypes.next(this.assignDocfields);
        return this.assignDocfields.slice();
    }
    getAllDocTypes() {
        return this._allDocuments.slice();
    }
    getReverificationList() {
        return this._reverifiData.slice();
    }
}
