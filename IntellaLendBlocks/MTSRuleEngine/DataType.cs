using System;

namespace MTSRuleEngine
{
    public static class DataType
    {
        public static object Find(string a)
        {

            DateTime d = DateTime.MinValue;
            DateTime.TryParse(a, out d);

            if (d != DateTime.MinValue)
                return d;
            
            Int64 i = 0;
            Int64.TryParse(a, out i);

            if (i.ToString().ToLower().Equals(a.Trim().ToLower()))
                return i;

            decimal de = 0;
            decimal.TryParse(a, out de);

            if (de.ToString().ToLower().Equals(a.Trim().ToLower()))
                return de;

            double dou = 0;
            double.TryParse(a, out dou);

            if (dou.ToString().ToLower().Equals(a.Trim().ToLower()))
                return dou;

            bool b = false;
            bool.TryParse(a, out b);

            if (b.ToString().ToLower().Equals(a.Trim().ToLower()))
                return b;

            return a;
        }
    }
}
