using IntellaLend.BoxWrapper;
using IntellaLend.Model;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanPDFGenerator
{
    class Program
    {
        //private static int GhostScriptConvertion(string lockFileName, string outputPath)
        //{
        //    String ars = $"-dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=\"{outputPath}\" -sPAPERSIZE=a4 \"{lockFileName}\" -c quit";
        //    Process proc = new Process();
        //    proc.StartInfo.FileName = GhostScriptPath;
        //    proc.StartInfo.Arguments = ars;
        //    proc.StartInfo.CreateNoWindow = true;
        //    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //    proc.Start();
        //    proc.WaitForExit(20 * 60000);
        //    bool isRunning = !proc.HasExited;
        //    if (isRunning)
        //        proc.Kill();
        //    return isRunning ? -1 : proc.ExitCode;
        //}

        static void Main(string[] args)
        {

            try
            {
                // string orginalPDF = @"D:\UHS\PDF Testing\4254742511 Muela.pdf";
                // string destPDF = @"D:\UHS\PDF Testing\test14.pdf";

                // //string orginalPDF = @"D:\UHS\PDF Testing\test13.pdf";
                // //string destPDF = @"D:\UHS\PDF Testing\gsPDF.pdf";

                // Int32[] pgs = { 1, 2, 3, 4, 5, 6, 7, 8 };

                //// CommonUtils.CheckImageAndCompress(orginalPDF, destPDF);

                // byte[] bytes = CommonUtils.ReOrderPDFPages(orginalPDF, destPDF, pgs);

                // File.WriteAllBytes(destPDF, bytes);





                // //WriteFile(orginalPDF, destPDF);

                Console.WriteLine("Enter PDF Failed Loans Path :");
                string pdfFailedPath = Console.ReadLine();

                Console.WriteLine("Enter Box Original PDF Path :");
                string originalPDFPath = Console.ReadLine();

                string[] _boxFiles = File.ReadAllLines(pdfFailedPath);
                Dictionary<string, List<string>> _loans = new Dictionary<string, List<string>>();

                foreach (string item in _boxFiles)
                {
                    List<string> _Ls = new List<string>();
                    string[] _boxItem = item.Split(',');

                    if (_loans.ContainsKey(_boxItem[0]))
                    {
                        _loans[_boxItem[0]].Add(_boxItem[1]);
                    }
                    else
                    {
                        _Ls.Add(_boxItem[1]);
                        _loans.Add(_boxItem[0], _Ls);
                    }
                }

                DownloadFromBox(_loans, originalPDFPath);
                //string[] _boxOriginalFiles = Directory.GetFiles(originalPDFPath);

                ////Console.WriteLine("Enter Loan PDF BasePath");
                ////string LoanPDFPath = Console.ReadLine();

                ////Console.WriteLine("Enter Audit Loan Detail ID:");
                ////string AuditLoanDetailID = Console.ReadLine();

                ////Console.WriteLine("Replace PDF Y/N");
                ////bool isreplace = Console.ReadLine().ToLower() == "y";

                //foreach (string _boxFile in _boxOriginalFiles)
                //{
                //    Int64 loanID = Convert.ToInt64(Path.GetFileName(_boxFile).Split('.')[0]);
                //    try
                //    {
                //        CreatePDF(loanID, _boxFile);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write($"Loan ID : {loanID.ToString()}");
                //        Console.Write(ex.Message);
                //        Console.Write(ex.StackTrace);
                //    }

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void WriteFile(string orginalPDF, string destPDF)
        {
            Int32[] pgs = { 1, 2, 3, 4, 5, 6, 7, 8 };


            //byte[] bytes = CommonUtils.ReOrderPDFPages(orginalPDF, destPDF, pgs);

           // File.WriteAllBytes(destPDF, bytes);
        }

        private static void DownloadFromBox(Dictionary<string, List<string>> _loans, string _boxOriginalPath)
        {
            BoxAPIWrapper boxwrap = new BoxAPIWrapper("T1", 1);

            foreach (string _loanID in _loans.Keys)
            {
                try
                {
                    string pdfPath = Path.Combine(_boxOriginalPath, _loanID + ".pdf");
                    PDFMerger merger = null;
                    if (_loans[_loanID].Count == 1)
                    {
                        File.WriteAllBytes(pdfPath, boxwrap.DownloadFile(_loans[_loanID][0]));
                    }
                    else
                    {
                        merger = new PDFMerger(pdfPath);
                        merger.OpenDocument();
                        foreach (string item in _loans[_loanID])
                        {
                            merger.AppendPDF(boxwrap.DownloadFile(item));
                        }
                        merger.SaveDocument();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"LoanID : {_loanID}, Message : {ex.Message}");
                }

            }
        }

        private static void CreateAndReplacePDF(Int64 loanId, string LoanPDFPath, string tenantSchema = "T1")
        {
            List<byte[]> documentsImageByteList = new List<byte[]>();
            LoanDataAccess dataAccess = new LoanDataAccess(tenantSchema);
            Console.WriteLine("Fetching Loan Deatils.......");
            Loan loan = dataAccess.GetLoandetails(loanId);
            if (loan != null)
            {
                Console.WriteLine("Fetching Stacking order.......");
                Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();

                foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                {
                    Console.WriteLine("Fetching Loan Images.......");
                    List<LoanCustomImage> loanImageDetails = dataAccess.GetLoanImages(loanId, item.DocumentTypeID);

                    foreach (LoanCustomImage imgdetail in loanImageDetails)
                    {
                        documentsImageByteList.Add(imgdetail.Image);
                    }
                }

                if (documentsImageByteList.Count > 0)
                {
                    Console.WriteLine("Creating PDF.......");
                    byte[] pdfBytes = CommonUtils.CreatePdfBytes(documentsImageByteList);

                    string directorypath = Path.Combine(LoanPDFPath, tenantSchema, DateTime.Now.ToString("yyyyMMdd"));
                    string pdfpath = Path.Combine(directorypath, loanId.ToString() + ".pdf");

                    if (!Directory.Exists(directorypath))
                        Directory.CreateDirectory(directorypath);

                    if (!Directory.Exists("LoanPDF"))
                        Directory.CreateDirectory("LoanPDF");

                    dataAccess.InsertLoanPdf(loanId, pdfpath);

                    File.WriteAllBytes(pdfpath, pdfBytes);

                    Console.WriteLine("New Loan PDF Path:" + pdfpath);

                }
                else
                {
                    throw new Exception("Images not found for Loan ID");
                }
            }
            else
            {
                throw new Exception("Loan ID not found");
            }
        }

        private static void CreatePDF(Int64 loanId, string _originalPDFPath, string tenantSchema = "t1")
        {
            List<byte[]> documentsImageByteList = new List<byte[]>();
            LoanDataAccess dataAccess = new LoanDataAccess(tenantSchema);
            Console.WriteLine("Fetching Loan Deatils.......");
            Loan loan = dataAccess.GetLoandetails(loanId);
            //Int64 AuditLoanDetailID = 
            List<int> pageNoList = new List<int>();
            if (loan != null)
            {
                AuditLoanDetail _loanDetail = dataAccess.GetLoanObject(loanId);
                Batch batchObj = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

                Console.WriteLine("Fetching Stacking order.......");
                Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                Console.WriteLine($"stackingOrderId : {stackingOrderId.ToString()}");
                List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();
                Console.WriteLine($"stackingOrderCount : {stackingOrderDetailList.Count.ToString()}");
                int pageSequence = 0;
                foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                {
                    //List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(batchObj.LoanID, item.DocumentTypeID);

                    var docList = batchObj.Documents.Where(X => X.DocumentTypeID == item.DocumentTypeID).OrderBy(sd => sd.VersionNumber).ToList();
                    //  Console.WriteLine($"{(docList.Count == 0).ToString()}");
                    GetPageNumberOrder(docList, false, ref pageSequence, ref pageNoList);
                }

                List<Documents> listDoc = batchObj.Documents.Where(l => !stackingOrderDetailList.Any(ls => ls.DocumentTypeID == l.DocumentTypeID)).OrderBy(d => d.DocumentTypeID).ThenByDescending(d => d.VersionNumber).ToList();
                if (listDoc.Count > 0)
                    GetPageNumberOrder(listDoc, false, ref pageSequence, ref pageNoList);

                Console.WriteLine(string.Join(",", pageNoList));

                //byte[] _pdfBytes = CommonUtils.ReOrderPDFPages(_originalPDFPath, "", pageNoList.ToArray());
                //File.WriteAllBytes(@"LoanPDF\" + loanId.ToString() + ".pdf", _pdfBytes);

                Console.WriteLine("New Loan PDF Path:" + System.IO.Directory.GetCurrentDirectory() + @"LoanPDF\" + loanId.ToString() + ".pdf");

                //foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                //{
                //    Console.WriteLine("Fetching Loan Images.......");
                //    List<LoanCustomImage> loanImageDetails = dataAccess.GetLoanImages(loanId, item.DocumentTypeID);

                //    foreach (LoanCustomImage imgdetail in loanImageDetails)
                //    {
                //        documentsImageByteList.Add(imgdetail.Image);
                //    }
                //}

                //if (documentsImageByteList.Count > 0)
                //{
                //    Console.WriteLine("Creating PDF.......");
                //    //byte[] pdfBytes = CommonUtils.CreatePdfBytes(documentsImageByteList);
                //    LoanPDF existingPDF= dataAccess.GetExistingLoanPdfDetails(loanId);

                //    if (existingPDF != null)
                //    {
                //        Console.WriteLine("Old Loan Path:" + existingPDF.LoanPDFPath);
                //    }
                //    if (!Directory.Exists("LoanPDF"))
                //        Directory.CreateDirectory("LoanPDF");

                //  //  File.WriteAllBytes(@"LoanPDF\"+ loanId.ToString() + ".pdf", pdfBytes);
                //    CommonUtils.CreatePdf(documentsImageByteList, @"LoanPDF\"+ loanId.ToString() + ".pdf");

                //    byte[] _pdfBytes = CommonUtils.ReOrderPDFPages(@"LoanPDF\BoxFile.pdf", "", pageNoList.ToArray());
                //    File.WriteAllBytes(@"LoanPDF\" + loanId.ToString() + ".pdf", _pdfBytes);

                //    Console.WriteLine("New Loan PDF Path:" + System.IO.Directory.GetCurrentDirectory() + @"LoanPDF\" + loanId.ToString() + ".pdf");

                //}
                //else
                //{
                //    throw new Exception("Images not found for Loan ID");
                //}
            }
            else
            {
                throw new Exception("Loan ID not found");
            }
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

    }
}
