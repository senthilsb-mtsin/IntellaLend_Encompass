using System;

namespace IntellaLend.Model
{
    public class RetensionPolicy
    {
        public Int32 Day { get; set; }
        public Int32 Month { get; set; }
        public Int32 Year { get; set; }
        public Int64 TotalDays { get; set; }
    }
}
