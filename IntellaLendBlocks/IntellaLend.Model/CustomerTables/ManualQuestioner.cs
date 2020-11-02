using System;

namespace IntellaLend.Model
{
    public class ManualQuestioner
    {
        public Int64 RuleID { get; set; }
        public Int64 CheckListDetailID { get; set; }
        public string Category { get; set; }
        public string CheckListName { get; set; }
        public string Question { get; set; }
        public string OptionJson { get; set; }
        public string AnswerJson { get; set; }
        public Int64 SequenceID { get; set; }
    }
}
