using IntellaLend.Model;
using MTSEntBlocks.UtilsBlock;
using MTSXMLParsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace batchObjTesting
{
    class Program
    {
        static void Main(string[] args)

        {
            Batch batchObj = ParseXml.GetParsedDataByFile(@"C:\Users\mtsuser\Desktop\BI177_batch.don");

            UpdateLastPageNumber(batchObj);

            //GetPageNumberOrder()

            Console.ReadKey();

        }

        private void GetPageNumberOrder(List<Documents> _listDocs, bool isMissingDocument, ref int pageSequence, ref List<Int32> pageNoList)
        {

            foreach (Documents doc in _listDocs)
            {
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    int pno = 0;

                    if (isMissingDocument && doc.Pages[i].ToUpper().StartsWith("PG"))
                        continue;

                    if (doc.Pages[i].ToUpper().StartsWith("PG"))
                    {
                        pno = ExtractPageNo(doc.Pages[i]);
                    }
                    else
                    {
                        pno = Convert.ToInt32(doc.Pages[i]);
                    }

                    if (!pageNoList.Contains(pno + 1))
                    {
                        pageNoList.Add(pno + 1);
                    }
                    //Update new Sequence Page no
                    doc.Pages[i] = pageSequence.ToString();
                    pageSequence++;
                }
            }
        }

        private static void UpdateLastPageNumber(Batch batchObj)
        {
            List<Int64> pageNoList = new List<Int64>();
            foreach (var document in batchObj.Documents)
                foreach (var page in document.Pages)
                {
                    Int64 pageNo = ExtractPageNo(page);
                    if (!pageNoList.Contains(pageNo))
                    {
                        pageNoList.Add(pageNo);
                    }
                }

            batchObj.LastPageNumber = pageNoList.Count == 0 ? "0" : pageNoList.Max().ToString();
        }

        private static int ExtractPageNo(string spageNo)
        {
            string extPattern = @"PG(?<PageNO>\d+)";
            int pageNo = 0;
            Int32.TryParse(CommonUtils.ExtractDataFromString(spageNo, extPattern)["PageNO"], out pageNo);
            return pageNo;
        }
    }
}
