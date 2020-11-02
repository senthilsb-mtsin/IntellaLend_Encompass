import { ConfigRequestModel } from './config-request.model';
export class UpdateAuditModel extends ConfigRequestModel {
  AuditConfig: AuditConfigdata;

  constructor(TableSchema: string, AuditConfig: AuditConfigdata) {
    super(TableSchema);
    this.AuditConfig = AuditConfig;
  }
}
export class AuditConfigdata {
  Description: string;
  AuditDescriptionID: number;
  Active: boolean;
  ConcatenateFields?: any;
}
