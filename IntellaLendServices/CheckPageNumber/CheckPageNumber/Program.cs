using IntellaLend.Model;
using MTSXMLParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPageNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Batch _batch = ParseXml.GetParsedDataByFile(@"D:\BI338B_batch.xml");

            List<List<string>> testDi = new List<List<string>>();

            List<string> test = _batch.Documents[0].Pages.GroupBy(x => x)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key).ToList<string>();

            string testt = string.Join(",", test);

        }
    }
}
