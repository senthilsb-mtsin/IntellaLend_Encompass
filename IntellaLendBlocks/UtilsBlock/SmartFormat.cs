using SmartFormat;
using System.Collections.Generic;

namespace MTSEntBlocks.UtilsBlock
{
    public static class SmartFormat
    {
        public static string Format(string Template, Dictionary<string, object> Field)
        {
            return Smart.Format(Template, Field);
        }
    }
}
