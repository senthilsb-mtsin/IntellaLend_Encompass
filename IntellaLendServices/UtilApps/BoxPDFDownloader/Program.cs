using IntellaLend.BoxWrapper;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxPDFDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter PDF Failed Loans Path :");
            string pdfFailedPath = "LoanToBeDownloaded.txt";

            ///Console.WriteLine("Enter Box Original PDF Path :");
            string originalPDFPath = @"OriginalBoxPDF";

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
        }

        private static void DownloadFromBox(Dictionary<string, List<string>> _loans, string _boxOriginalPath)
        {
            BoxAPIWrapper boxwrap = new BoxAPIWrapper("T1", 1);

            foreach (string _loanID in _loans.Keys)
            {
                
                try
                {
                    Console.WriteLine($"LoanID : {_loanID}. Starting To Download");
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
                    Console.WriteLine($"LoanID : {_loanID}. Downloaded Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"LoanID : {_loanID}, Message : {ex.Message}");
                }

            }
        }

    }
}
