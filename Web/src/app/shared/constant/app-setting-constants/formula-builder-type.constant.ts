export class FormulaBuilderTypesConstant {
    static FormulaTypes = [
        { Name: 'general', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'generalRule', Description: 'General', Icon: 'settings_applications', Active: false, IconColor: 'txt-info', AddUpdateField: 'generalDocumentTypes', AddUpdateField2: '', FunctionName: 'setEvalDocs', SaveFieldName: 'docField', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', SaveDocTypeName: 'generalDocumentTypes', DisabledFieldName: '' },
        { Name: 'if', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'conditionalRule', Description: 'If', Icon: 'device_hub', Active: false, IconColor: 'txt-indigo', AddUpdateField: '', AddUpdateField2: '', AddUpdateField3: '', FunctionName: 'setIfEvalDocs', SaveFieldName: '', SaveDocTypeName: '', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', DisabledFieldName: '' },
        { Name: 'in', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'inRule', Description: 'Any', Icon: 'gamepad', Active: false, IconColor: 'txt-warm', AddUpdateField: 'InDocumentTypes', AddUpdateField2: '', FunctionName: 'setComparsionEvalDocs', SaveFieldName: 'InDocField', SaveFieldName2: '', SaveFieldName3: '', SaveDocTypeName: 'InDocumentTypes', SaveFieldName4: '', DisabledFieldName: 'InDisValField' },
        { Name: 'datediff', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'datediffRule', Description: 'Datediff', Icon: 'date_range', Active: false, IconColor: 'txt-red', AddUpdateField: 'fromDateDocTypes', AddUpdateField2: 'ToDateDocumentTypes', FunctionName: 'setDateDiffEvalDocs', SaveFieldName2: 'toDate', SaveFieldName3: '', SaveFieldName4: '', SaveFieldName: 'fromDate', SaveDocTypeName: '', DisabledFieldName: '' },
        { Name: 'manual', Rule_Type: 1, Evaluate: false, ShowVersioning: false, RuleJsonName: 'manualGroup', Description: 'Manual', Icon: 'perm_identity', Active: false, IconColor: 'txt-themeRed', AddUpdateField: '', AddUpdateField2: '', FunctionName: '', SaveFieldName: '', SaveDocTypeName: '', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', DisabledFieldName: '' },
        { Name: 'empty', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'isEmptyRule', Description: 'Empty', Icon: 'label_outline', Active: false, IconColor: 'txt-green', AddUpdateField: 'EmptyDocTypes', AddUpdateField2: '', FunctionName: 'setEvalDocs', SaveFieldName: 'EmptyDocFieldTypes', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', SaveDocTypeName: 'EmptyDocTypes', DisabledFieldName: '' },
        { Name: 'compareall', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'compareAllRule', Description: 'CompareAll', Icon: 'compare_arrows', Active: false, IconColor: 'txt-orange', AddUpdateField: 'ComapreAllDocumentTypes', AddUpdateField2: '', FunctionName: 'setComparsionEvalDocs', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', SaveFieldName: 'CompareAllDocField', SaveDocTypeName: 'ComapreAllDocumentTypes', DisabledFieldName: 'compareallDisValField' },
        { Name: 'isnotempty', Rule_Type: 0, Evaluate: true, ShowVersioning: true, RuleJsonName: 'isNotEmptyRule', Description: 'IsNotEmpty', Icon: 'not_interested', Active: false, IconColor: 'txt-grey', AddUpdateField: 'NotEmptyDocTypes', AddUpdateField2: '', FunctionName: 'setEvalDocs', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName: 'NotEmptyDocFieldTypes', SaveFieldName4: '', SaveDocTypeName: 'NotEmptyDocTypes', DisabledFieldName: '' },
        { Name: 'checkall', Rule_Type: 0, Evaluate: false, ShowVersioning: true, RuleJsonName: 'docCheckAll', Description: 'CheckAll', Icon: 'done_all', Active: false, IconColor: '', AddUpdateField: 'CheckAllDocTypes', AddUpdateField2: '', FunctionName: '', SaveFieldName: 'CheckAllDocFieldTypes', SaveFieldName2: '', SaveFieldName3: '', SaveDocTypeName: '', SaveFieldName4: '', DisabledFieldName: '' },
        { Name: 'isexist', Rule_Type: 0, Evaluate: false, ShowVersioning: false, RuleJsonName: 'docIsExist', Description: 'IsExist', Icon: 'open_in_browser', Active: false, IconColor: '', AddUpdateField: 'IsExistDocTypes', AddUpdateField2: '', FunctionName: '', SaveFieldName: '', SaveFieldName2: '', SaveFieldName3: '', SaveDocTypeName: '', SaveFieldName4: '', DisabledFieldName: '' },
        { Name: 'losrule', Rule_Type: 0, Evaluate: false, ShowVersioning: false, RuleJsonName: 'losRule', Description: 'LOSRule', Icon: 'play_for_work', Active: false, IconColor: '', AddUpdateField: 'losDocumentTypes', AddUpdateField2: '', FunctionName: 'setEvalDocs', SaveFieldName: 'losValuesField', SaveDocTypeName: 'losLookUpDocumentTypes', SaveFieldName2: '', SaveFieldName3: '', SaveFieldName4: '', DisabledFieldName: '' },
        { Name: 'datatablerule', Rule_Type: 0, Evaluate: false, ShowVersioning: true, RuleJsonName: 'datatableRule', Description: 'DataTable', Icon: 'border_all', Active: false, IconColor: 'txt-info', AddUpdateField: 'generalDocumentTypes', AddUpdateField2: '', FunctionName: '', SaveFieldName: 'docField', SaveFieldName2: 'tableName', SaveFieldName3: 'columnName', SaveFieldName4: '', SaveDocTypeName: '', DisabledFieldName: '' },
        { Name: 'groupby', Rule_Type: 0, Evaluate: false, ShowVersioning: false, RuleJsonName: 'groupby', Description: 'GroupBy', Icon: 'group_work', Active: false, IconColor: 'txt-info', AddUpdateField: 'generalDocumentTypes', AddUpdateField2: '', FunctionName: '', SaveFieldName: 'docField', SaveDocTypeName: '', SaveFieldName2: 'groupByField', SaveFieldName3: 'orderByField', SaveFieldName4: 'groupField', DisabledFieldName: '' }
    ];

    static GeneralRuleOperators = [
        { id: '+', value: '+' },
        { id: '-', value: '-' },
        { id: '*', value: '*' },
        { id: '/', value: '/' },
        { id: '=', value: '=' },
        { id: '!=', value: '!=' },
        { id: '>', value: '>' },
        { id: '<', value: '<' },
        { id: '>=', value: '>=' },
        { id: '<=', value: '<=' },
        { id: '&&', value: 'AND' },
        { id: '||', value: 'OR' }
    ];
    static Operators = ['+', '-', '*', '/', '&', '||', '|', '=', '>', '<', '>=', '<='];
}
