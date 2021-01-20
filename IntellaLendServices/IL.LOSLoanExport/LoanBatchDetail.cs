using System;
using System.Collections.Generic;

namespace IL.LOSLoanExport
{

    public class ExportDocuments
    {
        public string DocumentID { get; set; }
        public string DocumentIdentifier { get; set; }
        public string BatchInstanceIdentifier { get; set; }
        public Int64 ILDocumentID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentDesc { get; set; }
        public string Version { get; set; }
        public string Confidence { get; set; }
        public bool Reviewed { get; set; }
        public string DocumentExtractionAccuracy { get; set; }
        public string DocumentFile { get; set; }
        public bool Obsolete { get; set; }
        public List<string> Pages { get; set; }
        public List<JSONField> DataFields { get; set; }
        public List<LoanTableData> DataTables { get; set; }
    }

    public class JSONField
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }

    public class LoanTableData
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
        public List<RowData> Rows { get; set; }
    }

    public class RowData
    {
        public List<JSONField> CellValues { get; set; }
    }


    public class ManualkAnswerJson
    {
        public Int64 RuleID { get; set; }
        public string Notes { get; set; }
        public List<object> Answer { get; set; }
    }
    public class AnswersJson
    {
        public string Notes { get; set; }
        public List<object> Answer { get; set; }
    }
    public class RuleResult
    {
        public Int64 RuleID { get; set; }
        public string RuleType { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public List<string> Options { get; set; }
        public List<string> SelectedAnswer { get; set; }
        public List<RuleDocs> DocTypes { get; set; }
        public string Notes { get; set; }
        public bool Result { get; set; }
        public string Expression { get; set; }
        public string EvaluatedExpression { get; set; }
        public string ErrorMessage { get; set; }
        public string RDocTypes { get; set; }
    }

    public class AutoRuleResult
    {
        public Int64 RuleID { get; set; }
        public string RuleType { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public bool Result { get; set; }
        public string Expression { get; set; }
        public string EvaluatedExpression { get; set; }
        public string ErrorMessage { get; set; }
        public List<RuleDocs> DocTypes { get; set; }
        public string RDocTypes { get; set; }
    }

    public class RuleDocs
    {
        public string DocumentType { get; set; }
        public string DocumentDesc { get; set; }
    }
    public class CheckboxOptions
    {
        public string checkboxoptions { get; set; }
    }

    public class RadioBoxOptions
    {
        public string radiooptions { get; set; }
    }

    public class RuleOptions
    {
        public List<ManualGroup> manualGroup { get; set; }
    }

    public class ManualGroup
    {
        public List<CheckboxOptions> CheckBoxChoices { get; set; }
        public List<RadioBoxOptions> raidoboxoptions { get; set; }
    }
}
