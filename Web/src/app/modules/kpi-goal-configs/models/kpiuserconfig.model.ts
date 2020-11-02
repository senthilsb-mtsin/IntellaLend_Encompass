export class KpiUserGroupConfigModel {
    GroupID = 0;
    RoleID = 0;
    UserName: any;
    PeriodFrom: any;
    PeriodTo: any;
    Goal: any;
    ConfigType: any = 0;
    IsValid: any = false;
    AuditKpiGoalConfig: AuditKpiGoalConfig[];
    KPIGoalConfig: KPIGoalConfig[];
}
export class AuditKpiGoalConfig {
    UserGroupID = 0;
    RoleID: number;
    PeriodFrom: any;
    PeriodTo: any;
    Goal: number;
}

export class KPIGoalConfig {
    ID: Number;
    UserID: Number;
    PeriodFrom: any;
    PeriodTo: any;
    Goal: number;
}
