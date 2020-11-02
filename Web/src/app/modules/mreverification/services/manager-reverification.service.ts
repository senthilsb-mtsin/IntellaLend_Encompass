import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ManagerReverificationdetailsModel } from '../models/manager-reverification-details.model';
import { Subject, Subscription } from 'rxjs';
import { NotificationService } from '@mts-notification';
import { Location } from '@angular/common';
import { ManagerReverificationDataAccess } from '../mreverification.data';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ReverificationStepModel } from '../../reverification/models/reverification-step.model';
import { environment } from 'src/environments/environment';
import { FileUploaderOptions } from 'ng2-file-upload';
import { AppSettings } from '@mts-app-setting';
import { ReverificationMappingModel } from '../../reverification/models/reverification-mapping.model';
import { Router } from '@angular/router';
import { EditDocumentModel } from '../models/edit-doc-submit.model';
import { RequestTemplateModel } from '../models/request-template.model';

const jwtHelper = new JwtHelperService();

@Injectable()

export class ManagerReverificationService {
    setManagerReverificationTableData = new Subject<any>();
    isSetMapping = new Subject<boolean>();
    setNextStep = new Subject<ReverificationStepModel>();
    AddReverificationSteps: any = {
        Reverification: 1,
        AssignDocTypes: 2
    };
    assignedDocuments = new Subject<number[]>();
    ManagerReverificationData = new Subject<ManagerReverificationdetailsModel>();
    TemplateMaster = new Subject<any>();
    _editReverifyData = new Subject<EditDocumentModel>();
    AllDocTypes = new Subject<any>();
    AssignedDocTypes = new Subject<any>();
    _mappingTemplate = new Subject<ReverificationMappingModel>();
    Fields = new Subject<any>();
    AssignedDocFieldTypes = new Subject<any>();
    FilterAllDocTypes = new Subject<any>();
    isprevious = new Subject<any>();
    _mappingTemplateValues: ReverificationMappingModel = new ReverificationMappingModel();
    setNextStepForDocs = false;

    constructor(private _notificationService: NotificationService,
        private location: Location,
        private _managerReverifydataAccess: ManagerReverificationDataAccess,
        private _route: Router
    ) {

    }
    private _managerReverifiData: any = [];
    private _tempMasters: any[] = [];
    private _stepID: Subscription;

    private _managerReverificationData: ManagerReverificationdetailsModel = new ManagerReverificationdetailsModel();
    private _fields: any = [];
    private _assignedDocuments: any[] = [];
    private _allDocuments: any[] = [];
    private ReverificationID = 0;
    private CustomerID = 0;
    private MappingID = 0;
    private LoanTypeID = 0;
    private TemplateID = 0;
    private DocTypeNames: any[] = [];
    private assignDocfields: any[] = [];

    SetManagerReverifyRowData(inputData) {
        this._managerReverificationData = inputData;
        this.ManagerReverificationData.next(this._managerReverificationData);
    }
    GetRowdata() {
        this.ManagerReverificationData.next(this._managerReverificationData);
    }
    CloseReverification() {
        this.location.back();
    }
    GetManagerReverification(_reqBody: any) {
        return this._managerReverifydataAccess.GetCustomerReverificationData(_reqBody).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            this._managerReverifiData = data;
            this.setManagerReverificationTableData.next(data);
        });
    }

    GetManagerReverificationTemplate() {
        this._tempMasters = [];
        this._tempMasters.push({ TemplateID: 0, TemplateName: '--Select Template--' });
        return this._managerReverifydataAccess.GetManagerReverificationTemplate().subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data) && data.length > 0) {
                data.forEach(element => {
                    this._tempMasters.push({ TemplateID: element.TemplateID, TemplateName: element.TemplateName });
                });
                this.TemplateMaster.next(this._tempMasters.slice());
            }
        });
    }
    EditSetNextStep() {
        const _req = { TableSchema: AppSettings.TenantSchema, CustomerID: this.CustomerID, LoanTypeID: this.LoanTypeID, ReverificationID: this.ReverificationID };
        this.GetLoanDocuments(_req);

    }
    SetStepID(stepid: any) {
        this._stepID = stepid;
        this.SetPrevious();
    }
    EditReverificationSubmit(_reqBody: any, _templateID: any, _loantypeID: any) {
        this.CustomerID = _reqBody.CustomerID;
        if (_reqBody.ReverificationName !== '' && _reqBody.LoanTypeID !== 0 && _reqBody.TemplateID !== 0 && _reqBody.MappingID !== 0 && _reqBody.ReverificationID !== 0) {
            return this._managerReverifydataAccess.UpdateReverification(_reqBody).subscribe(res => {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (isTruthy(data) && data.Success) {
                    this._notificationService.showSuccess('Re-Verification Updated Successfully');
                    this._editReverifyData.next(data);
                    this.MappingID = data.MappingID;
                    this.ReverificationID = data.ReverificationID;
                    this.TemplateID = _templateID;
                    const _req = { TableSchema: AppSettings.TenantSchema, CustomerID: this.CustomerID, LoanTypeID: _loantypeID, ReverificationID: data.ReverificationID };
                    this.GetLoanDocuments(_req);

                } else {
                    this._notificationService.showError('Re-Verification Update Failed');

                }
            });
        } else {
            this._notificationService.showError('LoanType and Re-Verification Required');

        }
    }
    SaveManagerReverifyDocMapping(_req: any) {
        return this._managerReverifydataAccess.SaveManagerReverifyDocMapping(_req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data)) {
                this.GetMappedTemplate();
                this._notificationService.showSuccess('Re-Verification Mapping Updated Successfully');

            } else {
                this._notificationService.showError('Re-Verification Mapping Update Failed');

            }
        });
    }
    CheckReverificationAvailableForEdit(_req: any, _loanTypeID: any, _templateID: any) {
        const input = { ReverificationName: _req.ReverificationName, TableSchema: AppSettings.TenantSchema };
        return this._managerReverifydataAccess.CheckReverificationAvailableForEdit(JSON.stringify(input)).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data.length === 1) {
                    if (data[0].ReverificationID === _req.ReverificationID) {
                        this.EditReverificationSubmit(_req, _loanTypeID, _templateID);
                    } else {
                        this._notificationService.showError('Reverification Name Already Exists');

                    }

                } else if (data.length === 0) {
                    this.EditReverificationSubmit(_req, _loanTypeID, _templateID);
                } else if (data.length > 1) {
                    this._notificationService.showError('Reverification Name Already Exists');

                } else {
                    const req = { LoanTypeID: _loanTypeID, ReverificationID: this.ReverificationID };
                    this.GetLoanDocuments(req);
                }
            }
        });

    }
    GetMappedTemplate() {
        if (this.MappingID !== 0 && this.ReverificationID !== 0 && this.TemplateID !== 0) {
            const req = { TableSchema: AppSettings.TenantSchema, MappingID: this.MappingID, TemplateID: this.TemplateID, ReverificationID: this.ReverificationID };
            return this._managerReverifydataAccess.GetManagerMappedTemplate(req).subscribe(res => {
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
            this._notificationService.showError('Failed');
        }
    }

    SetPrevious() {
        this.setNextStepForDocs = true;
        if (this._stepID === this.AddReverificationSteps.AssignDocTypes) {
            this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.Reverification, 'active', ''));
        }
    }
    getReverificationList() {
        return this._managerReverifiData.slice();
    }
    GetLoanDocuments(_req: any) {
        return this._managerReverifydataAccess.GetLoanDocuments(_req).subscribe(
            res => {
                if (isTruthy(res)) {
                    const result = jwtHelper.decodeToken(res.Data)['data'];
                    this._allDocuments = result.AllDocTypes;
                    this._assignedDocuments = result.AssignedDocTypes;
                    this.AssignedDocFieldTypes.next(result.AssignedDocFields);
                    this.assignDocfields = result.AssignedDocFields;
                    this.isprevious.next(true);
                    this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, 'active complete', 'active'));

                }
            });
    }
    SaveTemplate(req: RequestTemplateModel) {
        return this._managerReverifydataAccess.UpdateReverificationTemplateFields(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data)) {
                this._notificationService.showSuccess('Re-Verification Template Updated Successfully');
                this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, '', 'active complete'));
                this._route.navigate(['view/mreverification']);

            } else {
                this._notificationService.showError('Failed');
                this.setNextStep.next(new ReverificationStepModel(this.AddReverificationSteps.AssignDocTypes, '', 'active complete'));
                this._route.navigate(['view/mreverification']);
            }

        });
    }
    GetMappingTemplateValues() {
        return this._mappingTemplateValues;

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
    getFieldsForTemplate() {
        this.Fields.next(this._fields);
        return this._fields;
    }
    getAssignedDocFields() {
        this.AssignedDocFieldTypes.next(this.assignDocfields);
        return this.assignDocfields.slice();
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

    getAllDocTypes() {
        return this._allDocuments.slice();
    }

}
