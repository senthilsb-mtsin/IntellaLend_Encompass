using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace StackingOrderPDFGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IntellaLendImportDataAccess dataAccess = new IntellaLendImportDataAccess("T1");

                Console.WriteLine("Enter LoanIDs : ");
                string loanIDs = Console.ReadLine();
                foreach (var item in loanIDs.Split(','))
                {
                    Console.WriteLine($"Processing LoanID : {item} ....");
                    Int64 EUploadID = 0;
                    Int64 LoanID = Convert.ToInt64(item);
                    string existingPDF = string.Empty;
                    Batch batchObj = dataAccess.GetBatchDetails(LoanID);
                    GenerateLoanPdfByStackingOrder(batchObj, dataAccess);
                    existingPDF = GetOldPDFFileName(batchObj, dataAccess);
                    EUploadID = InitializeEncompassUpload(dataAccess, batchObj);

                    if (!string.IsNullOrEmpty(existingPDF) && dataAccess.CheckCustomerConfigKey() && File.Exists(existingPDF))
                        File.Delete(existingPDF);

                    dataAccess.UpdateEUploadStatus(EUploadID);

                    Console.WriteLine($"Completed LoanID : {item}");
                }

                Console.WriteLine($"All Loans Completed...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }


        }

        private static Int64 InitializeEncompassUpload(IntellaLendImportDataAccess dataAccess, Batch batchObj, bool isMissingDocument = false)
        {
            string docsToUpload = string.Empty;
            if (isMissingDocument)
            {
                object docs = batchObj.Documents.Select(s => new { DocumentName = s.Type, Version = s.VersionNumber }).ToList();
                docsToUpload = JsonConvert.SerializeObject(docs);
            }

            return dataAccess.InitializeEncompassUpload(batchObj.LoanID, isMissingDocument, docsToUpload);
        }

        private static string GetOldPDFFileName(Batch batchObj, IntellaLendImportDataAccess dataAccess)
        {
            string IntellaLendLoanUploadPath = ConfigurationManager.AppSettings["LoanUploadPath"].ToString();
            string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, batchObj.Schema);
            string loanFile = batchObj.Schema + "_" + batchObj.LoanID.ToString() + ".don";
            var fileSerach = Directory.GetFiles(Path.Combine(tenantFolder, "Output"), loanFile, SearchOption.AllDirectories).ToArray();
            if (fileSerach.Length > 0)
            {
                return fileSerach[0];
            }

            return string.Empty;
        }

        private static void GetPageNumberOrder(List<Documents> _listDocs, bool isMissingDocument, ref int pageSequence, ref List<Int32> pageNoList)
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
        private static int ExtractPageNo(string spageNo)
        {
            string extPattern = @"PG(?<PageNO>\d+)";
            int pageNo = 0;
            Int32.TryParse(CommonUtils.ExtractDataFromString(spageNo, extPattern)["PageNO"], out pageNo);
            return pageNo;
        }

        private static void GenerateLoanPdfByStackingOrder(Batch batchObj, IntellaLendImportDataAccess dataAccess, bool isMissingDocument = false)
        {

            List<int> pageNoList = new List<int>();

            Loan loan = dataAccess.GetLoanInfo(batchObj.LoanID);

            if (loan != null)
            {
                string existingPDF = GetOldPDFFileName(batchObj, dataAccess);

                if (!string.IsNullOrEmpty(existingPDF))
                {
                    //Get Stacking Order
                    int pageSequence = 0;
                    Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                    List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();
                    foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                    {
                        //List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(batchObj.LoanID, item.DocumentTypeID);
                        var docList = batchObj.Documents.Where(X => X.DocumentTypeID == item.DocumentTypeID).OrderBy(sd => sd.VersionNumber).ToList();
                        GetPageNumberOrder(docList, isMissingDocument, ref pageSequence, ref pageNoList);

                    }

                    //
                    bool incAllDocs = dataAccess.GetIncLoantypeDocs();
                    if (incAllDocs)
                    {
                        List<Documents> listDoc = batchObj.Documents.Where(l => !stackingOrderDetailList.Any(ls => ls.DocumentTypeID == l.DocumentTypeID)).OrderBy(d => d.DocumentTypeID).ThenByDescending(d => d.VersionNumber).ToList();
                        GetPageNumberOrder(listDoc, isMissingDocument, ref pageSequence, ref pageNoList);
                    }
                    var isDeletOrgFile = dataAccess.CheckCustomerConfigKey();

                    Dictionary<Int32, string> pgLevelLS = new Dictionary<Int32, string>();
                    foreach (Documents doc in batchObj.Documents)
                    {
                        if (doc.PageLevelFields != null)
                        {
                            List<PageLevelFields> pgLs = doc.PageLevelFields.Where(p => p.IsRotated == true).ToList();

                            foreach (PageLevelFields pg in pgLs)
                                pgLevelLS[Convert.ToInt32(pg.PageNo)] = pg.Direction.ToUpper();

                        }
                    }


                    string newPDFPath = string.Empty;

                    byte[] _pdfBytes = new byte[0];

                    if (pageNoList.Count > 0)
                    {
                        byte[] _oldPDFBytes = File.ReadAllBytes(existingPDF);

                        _pdfBytes = CommonUtils.ReOrderPDFPages(_oldPDFBytes, newPDFPath, pageNoList.ToArray(), pgLevelLS);

                        if (dataAccess.CheckCustomerConfigKey())
                            File.Delete(existingPDF);

                        new ImageMinIOWrapper(batchObj.Schema).InsertLoanPDF(batchObj.LoanID, _pdfBytes);

                        dataAccess.InsertLoanPdf(batchObj, newPDFPath);
                    }
                }
            }
        }

    }
}
