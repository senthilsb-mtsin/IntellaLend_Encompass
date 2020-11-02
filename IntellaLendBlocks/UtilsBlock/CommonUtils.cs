using IntellaLend.Constance;
using IntellaLend.Model;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MTSEntBlocks.UtilsBlock
{
    public static class CommonUtils
    {
        public static string footerText = string.Empty;
        public static string ReplaceFileSpecialChars(string fileName, string replace = "")
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), replace));
        }

        public static string UnicodeToAscii(string fileName)
        {

            byte[] asciiArry = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(fileName));

            char[] asciiChars = new char[Encoding.ASCII.GetCharCount(asciiArry, 0, asciiArry.Length)];

            Encoding.ASCII.GetChars(asciiArry, 0, asciiArry.Length, asciiChars, 0);

            return new string(asciiChars);

        }

        public static Dictionary<string, string> ExtractDataFromString(string inputString, string regexPattern)
        {
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            Regex pattern = new Regex(regexPattern);
            Match match = pattern.Match(inputString);
            foreach (var group in pattern.GetGroupNames())
            {
                if (group != "0")
                    returnDic[group] = match.Groups[group].Value;
            }
            return returnDic;
        }

        public static Dictionary<string, string> ExtractDataFromString(string inputString, string[] regexPattern)
        {
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            foreach (string pattern in regexPattern)
            {
                Regex regPattern = new Regex(pattern);
                Match match = regPattern.Match(inputString);
                if (regPattern.Match(inputString).Success)
                {
                    foreach (string group in regPattern.GetGroupNames())
                    {
                        if (group != "0")
                            returnDic[group] = match.Groups[group].Value;
                    }
                    break;
                }

            }

            return returnDic;
        }


        public static string EnDecrypt(string input, bool decrypt = false)
        {
            string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ984023";

            if (decrypt)
            {
                Dictionary<string, uint> _index = null;
                Dictionary<string, Dictionary<string, uint>> _indexes = new Dictionary<string, Dictionary<string, uint>>(2, StringComparer.InvariantCulture);

                if (_index == null)
                {
                    Dictionary<string, uint> cidx;

                    string indexKey = "I" + _alphabet;

                    if (!_indexes.TryGetValue(indexKey, out cidx))
                    {
                        lock (_indexes)
                        {
                            if (!_indexes.TryGetValue(indexKey, out cidx))
                            {
                                cidx = new Dictionary<string, uint>(_alphabet.Length, StringComparer.InvariantCulture);
                                for (int i = 0; i < _alphabet.Length; i++)
                                {
                                    cidx[_alphabet.Substring(i, 1)] = (uint)i;
                                }
                                _indexes.Add(indexKey, cidx);
                            }
                        }
                    }

                    _index = cidx;
                }

                MemoryStream ms = new MemoryStream(Math.Max((int)Math.Ceiling(input.Length * 5 / 8.0), 1));

                for (int i = 0; i < input.Length; i += 8)
                {
                    int chars = Math.Min(input.Length - i, 8);

                    ulong val = 0;

                    int bytes = (int)Math.Floor(chars * (5 / 8.0));

                    for (int charOffset = 0; charOffset < chars; charOffset++)
                    {
                        uint cbyte;
                        if (!_index.TryGetValue(input.Substring(i + charOffset, 1), out cbyte))
                        {
                            throw new ArgumentException(string.Format("Invalid character {0} valid characters are: {1}", input.Substring(i + charOffset, 1), _alphabet));
                        }

                        val |= (((ulong)cbyte) << ((((bytes + 1) * 8) - (charOffset * 5)) - 5));
                    }

                    byte[] buff = BitConverter.GetBytes(val);
                    Array.Reverse(buff);
                    ms.Write(buff, buff.Length - (bytes + 1), bytes);
                }

                return System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray());
            }
            else
            {

                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(input);

                StringBuilder result = new StringBuilder(Math.Max((int)Math.Ceiling(data.Length * 8 / 5.0), 1));

                byte[] emptyBuff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                byte[] buff = new byte[8];

                for (int i = 0; i < data.Length; i += 5)
                {
                    int bytes = Math.Min(data.Length - i, 5);

                    Array.Copy(emptyBuff, buff, emptyBuff.Length);
                    Array.Copy(data, i, buff, buff.Length - (bytes + 1), bytes);
                    Array.Reverse(buff);
                    ulong val = BitConverter.ToUInt64(buff, 0);

                    for (int bitOffset = ((bytes + 1) * 8) - 5; bitOffset > 3; bitOffset -= 5)
                    {
                        result.Append(_alphabet[(int)((val >> bitOffset) & 0x1f)]);
                    }
                }


                return result.ToString();
            }
        }

        public static byte[] CreatePdfBytes(List<byte[]> documentsImageByteList, string pageSize = "A4")
        {
            if (documentsImageByteList.Count > 0)
            {

                iTextSharp.text.Rectangle rec = PageSize.GetRectangle(pageSize);

                using (MemoryStream ms = new MemoryStream())
                {
                    Document myDocument = new Document(rec, 0, 0, 0, 0);
                    PdfWriter writer = PdfWriter.GetInstance(myDocument, ms);
                    myDocument.Open();
                    PdfContentByte cb = writer.DirectContent;
                    foreach (var imageBytes in documentsImageByteList)
                    {

                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes);
                        img.SetAbsolutePosition(0, 0);
                        img.ScaleAbsolute(rec);
                        cb.AddImage(img);
                        myDocument.NewPage();
                    }
                    myDocument.Close();
                    writer.Dispose();
                    myDocument.Dispose();
                    byte[] pdfBytes = ms.ToArray();
                    return pdfBytes;
                }
            }
            else
            {
                return new byte[0];
            }
        }

        public static void CreatePdf(List<byte[]> documentsImageByteList, string filePath, string pageSize = "A4")
        {
            if (documentsImageByteList.Count > 0)
            {
                iTextSharp.text.Rectangle rec = PageSize.GetRectangle(pageSize);

                using (FileStream ms = new FileStream(filePath, FileMode.Create))
                {
                    Document myDocument = new Document(rec, 0, 0, 0, 0);
                    PdfWriter writer = PdfWriter.GetInstance(myDocument, ms);
                    myDocument.Open();
                    foreach (var imageBytes in documentsImageByteList)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageBytes);
                        img.SetAbsolutePosition(0, 0);
                        img.ScaleAbsolute(rec);
                        myDocument.Add(img);
                        myDocument.NewPage();
                    }
                    myDocument.Close();
                    writer.Close();
                    writer.Dispose();
                    myDocument.Dispose();
                }
            }
        }

        public static void AppendToPDF(string source1, string source2, int[] pageOrder = null)
        {

            string tempFileName = Path.Combine(Path.GetDirectoryName(source1), Path.GetFileNameWithoutExtension(source1) + "_temp" + Path.GetExtension(source1));

            using (var source1reader = new PdfReader(source1))
            {
                using (var source2reader = new PdfReader(source2))
                {
                    using (FileStream fs = new FileStream(tempFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (Document doc = new Document(source1reader.GetPageSizeWithRotation(1)))
                        {
                            using (PdfCopy copy = new PdfCopy(doc, fs))
                            {
                                doc.Open();
                                copy.SetLinearPageMode();

                                for (int i = 1; i <= source1reader.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(source1reader, i));
                                }

                                if (pageOrder != null)
                                {
                                    foreach (int pageNo in pageOrder)
                                    {
                                        copy.AddPage(copy.GetImportedPage(source2reader, pageNo));
                                    }
                                }
                                else
                                {
                                    for (int i = 1; i <= source2reader.NumberOfPages; i++)
                                    {
                                        copy.AddPage(copy.GetImportedPage(source2reader, i));
                                    }
                                }

                                doc.Close();
                            }
                        }
                    }
                }

            }

            File.Delete(source1);
            File.Move(tempFileName, source1);
        }

        public static byte[] AppendToPDF(byte[] source1, string source2, int[] pageOrder = null)
        {

            //   string tempFileName = Path.Combine(Path.GetDirectoryName(source1), Path.GetFileNameWithoutExtension(source1) + "_temp" + Path.GetExtension(source1));
            byte[] _pdfBytes = new byte[0];
            using (var source1reader = new PdfReader(source1))
            {
                using (var source2reader = new PdfReader(source2))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Document doc = new Document(source1reader.GetPageSizeWithRotation(1)))
                        {
                            using (PdfCopy copy = new PdfCopy(doc, ms))
                            {
                                doc.Open();
                                copy.SetLinearPageMode();

                                for (int i = 1; i <= source1reader.NumberOfPages; i++)
                                {
                                    copy.AddPage(copy.GetImportedPage(source1reader, i));
                                }

                                if (pageOrder != null)
                                {
                                    foreach (int pageNo in pageOrder)
                                    {
                                        copy.AddPage(copy.GetImportedPage(source2reader, pageNo));
                                    }
                                }
                                else
                                {
                                    for (int i = 1; i <= source2reader.NumberOfPages; i++)
                                    {
                                        copy.AddPage(copy.GetImportedPage(source2reader, i));
                                    }
                                }

                                doc.Close();
                            }
                        }
                        _pdfBytes = ms.ToArray();
                    }
                }

            }

            return _pdfBytes;
        }


        public static int GetPDFPageCount(string PdfFileName)
        {
            int totalPage = 0;
            using (var reader = new PdfReader(PdfFileName))
            {
                totalPage = reader.NumberOfPages;
            }
            return totalPage;
        }

        public static int GetPDFPageCount(byte[] _pdfBytes)
        {
            int totalPage = 0;
            using (var reader = new PdfReader(_pdfBytes))
            {
                totalPage = reader.NumberOfPages;
            }
            return totalPage;
        }

        public static byte[] ReOrderPDFPages(string orginalFileName, string newFileName, int[] pageOrder, Dictionary<Int32, string> pgLevelLS)
        {
            //bool overWriteFile = false;
            //if(orginalFileName== newFileName)
            //{
            //    overWriteFile = true;
            //    newFileName = Path.Combine(Path.GetDirectoryName(orginalFileName), Path.GetFileNameWithoutExtension(orginalFileName) + "_temp" + Path.GetExtension(orginalFileName));
            //}
            byte[] _pdfBytes = null;
            using (var reader = new PdfReader(orginalFileName))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document doc = new Document(reader.GetPageSizeWithRotation(1)))
                    {
                        using (PdfCopy copy = new PdfCopy(doc, ms))
                        {
                            doc.Open();
                            copy.SetLinearPageMode();

                            foreach (int pageNo in pageOrder)
                            {
                                Console.WriteLine($"PageNO : {pageNo.ToString()}");
                                try
                                {
                                    if (pgLevelLS.ContainsKey(pageNo))
                                    {
                                        //  Console.WriteLine($"Rotated Page : {pageNo.ToString()}");
                                        PdfDictionary pageDict = reader.GetPageN(pageNo);
                                        int rotation = reader.GetPageRotation(pageNo);
                                        pageDict = reader.GetPageN(pageNo);
                                        int rotationChange = EphesoftRotationConstants.Direction[pgLevelLS[pageNo]];
                                        pageDict.Put(PdfName.ROTATE, new PdfNumber(rotation + rotationChange));
                                    }
                                    copy.AddPage(copy.GetImportedPage(reader, pageNo));
                                }
                                catch (Exception ex)
                                {
                                    Exception e = new Exception($"PageNO : {pageNo.ToString()}", ex);
                                    throw ex;
                                }

                            }

                            doc.Close();
                        }
                    }

                    _pdfBytes = ms.ToArray();
                }
            }

            return _pdfBytes;

            //if(overWriteFile)
            //{
            //    File.Delete(orginalFileName);
            //    File.Move(newFileName, orginalFileName);
            //}

        }

        public static byte[] ReOrderPDFPages(byte[] _oldPDFBytes, string newFileName, int[] pageOrder, Dictionary<Int32, string> pgLevelLS)
        {
            byte[] _pdfBytes = null;
            using (var reader = new PdfReader(_oldPDFBytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document doc = new Document(reader.GetPageSizeWithRotation(1)))
                    {
                        using (PdfCopy copy = new PdfCopy(doc, ms))
                        {
                            doc.Open();
                            copy.SetLinearPageMode();

                            foreach (int pageNo in pageOrder)
                            {
                                try
                                {
                                    if (pgLevelLS.ContainsKey(pageNo))
                                    {
                                        PdfDictionary pageDict = reader.GetPageN(pageNo);
                                        int rotationChange = EphesoftRotationConstants.Direction[pgLevelLS[pageNo]];
                                        pageDict.Put(PdfName.ROTATE, new PdfNumber(rotationChange));
                                    }

                                    copy.AddPage(copy.GetImportedPage(reader, pageNo));
                                }
                                catch (Exception ex)
                                {
                                    Exception e = new Exception($"PageNO : {pageNo.ToString()}", ex);
                                    throw e;
                                }
                            }

                            doc.Close();
                        }
                    }

                    _pdfBytes = ms.ToArray();
                }
            }

            return _pdfBytes;
        }

        public static byte[] CreateConfiguredPDF(byte[] originalPDF, LoanJobExport jobDetails, List<TOCDetails> tocDetails)
        {
            byte[] batchPDF = null;
            if (jobDetails.TableOfContent)
            {
                batchPDF = CreateTocPDF("Index", originalPDF, tocDetails);
            }
            if (jobDetails.PasswordProtected)
            {
                if (jobDetails.TableOfContent)
                {
                    batchPDF = CreatePasswordProtectedPDF(batchPDF, jobDetails);
                }
                else
                {
                    batchPDF = CreatePasswordProtectedPDF(originalPDF, jobDetails);
                }

            }
            //byte[] imgPdf = null;
            return batchPDF;

        }

        public static byte[] CreateTocPDF(string Title, byte[] originalImage, List<TOCDetails> _tocDetails)
        {
            byte[] pdfArray = null;
            //byte[] img = System.IO.File.ReadAllBytes(@"D:\Project\6Docs.pdf");
            MemoryStream PDFData = new MemoryStream(originalImage);
            var ms = new MemoryStream();
            // Document document = new Document(PageSize.LETTER, 50, 50, 80, 50);
            // Document document = new Document(PageSize.A4);

            TOCEvent eventTo = new TOCEvent();
            using (var reader = new PdfReader(originalImage))
            {
                using (ms)
                {
                    //document = new Document(reader.GetPageSizeWithRotation(1));
                    // Rectangle size = reader.GetPageSizeWithRotation(1);
                    Document document = new Document(PageSize.A4);
                    var PDFWriter = PdfWriter.GetInstance(document, ms);
                    PDFWriter.PageEvent = eventTo;
                    document.Open();
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        //Adding New Page and Also Responsible for the Page Redirection in OverRiding TOC class Method.
                        document.NewPage();
                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = PDFWriter.GetImportedPage(reader, i);
                        string docName = _tocDetails.Find(a => a.StartingPage == i) == null ? string.Empty : _tocDetails.Find(a => a.StartingPage == i).Type;
                        var contentByte = PDFWriter.DirectContent;
                        int pageNum = _tocDetails.Find(a => a.StartingPage == i) == null ? 0 : _tocDetails.Find(a => a.StartingPage == i).StartingPage;
                        String title = string.Empty;

                        // Customized To Set the Page Number in Starting Page
                        if (string.IsNullOrEmpty(docName))
                        {
                            title = docName + "|" + i;
                        }
                        else
                        {
                            int j = i + 1;
                            title = docName + "|" + j;
                        }
                        Chunk c = new Chunk(title, new Font());
                        c.SetGenericTag(title);

                        if (i == pageNum)
                        {
                            document.Add(new Paragraph(c));
                        }
                        contentByte.AddTemplate(importedPage, 0, 0);
                    }
                    document.NewPage();
                    //document.Close();
                    var headerFont = FontFactory.GetFont("Arial", 20, BaseColor.BLACK);
                    Paragraph para = new Paragraph("Table Of Contents", new Font(headerFont));
                    para.Alignment = Element.ALIGN_CENTER;
                    document.Add(para);
                    Chunk dottedLine = new Chunk(new DottedLineSeparator());
                    List<PageIndex> entries = eventTo.getTOC();
                    Paragraph p;
                    pdfArray = CreateStartPage(entries, originalImage, _tocDetails);
                    document.Close();

                }
            }
            return pdfArray;
        }



        public static byte[] CreateStartPage(List<PageIndex> entries, byte[] originalImage, List<TOCDetails> _tocDetails)
        {
            MemoryStream PDFData = new MemoryStream(originalImage);
            var ms = new MemoryStream();
            TOCEvent eventTo = new TOCEvent();
            using (var reader = new PdfReader(originalImage))
            {
                using (ms)
                {
                    //document = new Document(reader.GetPageSizeWithRotation(1));
                    // Rectangle size = reader.GetPageSizeWithRotation(1);
                    Document document = new Document(reader.GetPageSizeWithRotation(1));
                    var PDFWriter = PdfWriter.GetInstance(document, ms);
                    PDFWriter.PageEvent = eventTo;
                    document.Open();

                    document.NewPage();
                    //document.Close();
                    var headerFont = FontFactory.GetFont("Arial", 20, BaseColor.BLACK);
                    Paragraph para = new Paragraph("Table Of Contents", new Font(headerFont));
                    para.Alignment = Element.ALIGN_CENTER;
                    document.Add(para);
                    Chunk dottedLine = new Chunk(new DottedLineSeparator());
                    //List<PageIndex> entries = eventTo.getTOC();
                    Paragraph p;
                    for (int i = 0; i < 1; i++)
                    {
                        foreach (PageIndex pageIndex in entries)
                        {
                            Chunk chunk = new Chunk(pageIndex.Text);
                            chunk.SetAction(PdfAction.GotoLocalPage(pageIndex.Name, false));
                            p = new Paragraph(chunk);
                            p.Add(dottedLine);
                            chunk = new Chunk(pageIndex.Page.ToString());
                            chunk.SetAction(PdfAction.GotoLocalPage(pageIndex.Name, false));
                            p.Add(chunk);
                            document.Add(p);
                        }
                    }
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                        document.NewPage();
                        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        var importedPage = PDFWriter.GetImportedPage(reader, i);
                        int pageNum = _tocDetails.Find(a => a.StartingPage == i) == null ? 0 : _tocDetails.Find(a => a.StartingPage == i).StartingPage;
                        // This docName not Used for Initialize  Title ,used only for validation  purpose
                        string docName = _tocDetails.Find(a => a.StartingPage == i) == null ? string.Empty : _tocDetails.Find(a => a.StartingPage == i).Type;
                        var contentByte = PDFWriter.DirectContent;
                        String title = string.Empty;
                        if (string.IsNullOrEmpty(docName))
                        {
                            title = "Title" + "|" + i + "|true";
                        }
                        else
                        {
                            int j = i + 1;
                            title = "Title" + "|" + j + "|true";
                        }
                        var redListTextFont = FontFactory.GetFont("Arial", 1, BaseColor.WHITE);
                        Chunk c = new Chunk(title, new Font(redListTextFont));
                        c.SetGenericTag(title);
                        //Exceptional Case to Overide Particular Documents

                        if (i == pageNum)
                        {
                            document.Add(new Paragraph(c));
                        }
                        contentByte.AddTemplate(importedPage, 0, -1);
                    }
                    document.Close();
                }
            }
            byte[] pdfBytes;
            pdfBytes = ms.ToArray();
            ms.Close();
            return pdfBytes;
        }

        public static byte[] CreateCoverLetter(StringBuilder sb, LoanJobExport _jobDetails)
        {

            StringReader sr = new StringReader(sb.ToString());
            //byte[] bytes =null;
            byte[] protectedPdf = null;
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                pdfDoc.Close();

                protectedPdf = memoryStream.ToArray();
                if (_jobDetails.PasswordProtected)
                {
                    //protectedPdf = null;
                    protectedPdf = CreatePasswordProtectedPDF(protectedPdf, _jobDetails);
                }

                memoryStream.Close();
                // File.WriteAllBytes(@"D:\MTS\123.pdf", bytes);
            }
            return protectedPdf;
        }

        public static byte[] CreatePasswordProtectedPDF(byte[] pdfBytes, LoanJobExport jobDetails)
        {
            byte[] protectedPDF = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream input = new MemoryStream(pdfBytes))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        PdfReader reader = new PdfReader(input);
                        PdfEncryptor.Encrypt(reader, output, true, jobDetails.Password, "secret", PdfWriter.ALLOW_SCREENREADERS);
                        protectedPDF = output.ToArray();
                        // File.WriteAllBytes(OutputFile, img);
                    }
                }

            }
            return protectedPDF;
        }

        public class TOCEvent : PdfPageEventHelper
        {
            protected int counter = 0;
            protected List<PageIndex> toc = new List<PageIndex>();

            public override void OnGenericTag(PdfWriter writer, Document document, Rectangle rect, string text)
            {
                string[] temp = text.Split('|');
                string name = "dest" + (temp[1]);
                int page = Convert.ToInt16(temp[1]);
                if (temp.Length == 3)
                    temp[0] = " ";

                toc.Add(new PageIndex() { Text = temp[0], Name = name, Page = page });
                writer.DirectContent.LocalDestination(name, new PdfDestination(PdfDestination.FITH, rect.GetTop(0)));
            }

            public List<PageIndex> getTOC()
            {
                return toc;
            }

        }


        public static byte[] GetDocPDFFromLoan(byte[] originalPDF, List<int> _pageNum)
        {
            using (PdfReader originalPDFReader = new PdfReader(originalPDF))
            {
                using (MemoryStream msCopy = new MemoryStream())
                {
                    using (Document docCopy = new Document())
                    {
                        using (PdfCopy copy = new PdfCopy(docCopy, msCopy))
                        {
                            docCopy.Open();

                            foreach (var pages in _pageNum)
                                copy.AddPage(copy.GetImportedPage(originalPDFReader, pages + 1));

                            docCopy.Close();
                        }
                    }

                    return msCopy.ToArray();
                }
            }
        }


        public static ReArrangePDF ReArrrange(Dictionary<string, List<int>> internalDictionary, byte[] OriginalLoanPDF)
        {
            byte[] pdfBytes = null;
            ReArrangePDF _reArrange = new ReArrangePDF();
            List<TOCDetails> _toc = new List<TOCDetails>();
            PdfReader originalPDFReader = new PdfReader(OriginalLoanPDF);
            List<int> _pageNum = new List<int>();
            using (MemoryStream msCopy = new MemoryStream())
            {
                using (Document docCopy = new Document())
                {
                    using (PdfCopy copy = new PdfCopy(docCopy, msCopy))
                    {
                        docCopy.Open();

                        int i = 0;
                        foreach (var item in internalDictionary)
                        {
                            _toc.Add(new TOCDetails
                            {
                                StartingPage = (Convert.ToInt32(i) + 1),
                                Type = item.Key
                            });

                            foreach (var pages in item.Value)
                            {
                                copy.AddPage(copy.GetImportedPage(originalPDFReader, pages + 1));
                                i++;
                            }
                        }
                        docCopy.Close();
                    }
                }

                pdfBytes = msCopy.ToArray();
                _reArrange._tocDetails = _toc;
                _reArrange.TocPDF = pdfBytes;
            }

            return _reArrange;
        }

        public static byte[] CreateHeaderFooterPDF(byte[] imageBytes, string _footerText)
        {
            var ms = new MemoryStream();
            PdfReader reader = new PdfReader(imageBytes);
            footerText = _footerText;
            Rectangle newSize = PageSize.A4;
            Document doc = new Document(newSize, 0, 0, 0, 20);
            using (ms)
            {
                PdfWriter PDFWriter = PdfWriter.GetInstance(doc, ms);
                // calling PDFFooter class to Include in document
                PDFWriter.PageEvent = new PDFFooter();
                doc.Open();
                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    Rectangle originalSize = reader.GetPageSize(i);
                    if (originalSize.Height >= newSize.Height && originalSize.Width >= newSize.Width)
                    {
                        float originalHeight = originalSize.Height;
                        float originalWidth = originalSize.Width;
                        float newHeight = newSize.Height;
                        float newWidth = newSize.Width;
                        float scaleHeight = (newHeight / originalHeight);
                        float scaleWidth = newWidth / originalWidth;
                        var contentByte = PDFWriter.DirectContent;
                        var importedPage = PDFWriter.GetImportedPage(reader, i);
                        contentByte.AddTemplate(importedPage, scaleWidth, 0, 0, scaleHeight, 0, 20);
                    }
                    else
                    {
                        var contentByte = PDFWriter.DirectContent;
                        var importedPage = PDFWriter.GetImportedPage(reader, i);
                        contentByte.AddTemplate(importedPage, 0, 20);
                    }

                    doc.NewPage();
                }
                doc.Close();
            }
            byte[] _pdfBytes = ms.ToArray();
            return _pdfBytes;
        }

        public static byte[] GetPdfBytes(List<int> pages, byte[] OriginalLoanPDF)
        {
            byte[] pdfBytes = null;
            PdfReader originalPDFReader = new PdfReader(OriginalLoanPDF);
            using (MemoryStream msCopy = new MemoryStream())
            {
                using (Document docCopy = new Document())
                {
                    using (PdfCopy copy = new PdfCopy(docCopy, msCopy))
                    {
                        docCopy.Open();

                        foreach (var page in pages)
                        {
                            copy.AddPage(copy.GetImportedPage(originalPDFReader, page + 1));
                        }
                        docCopy.Close();
                    }
                }
                pdfBytes = msCopy.ToArray();
            }
            return pdfBytes;
        }
        public class PDFFooter : PdfPageEventHelper
        {
            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                //base.OnOpenDocument(writer, document);
                //PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                //tabFot.SpacingAfter = 10F;
                //PdfPCell cell;
                //tabFot.TotalWidth = 300F;
                //cell = new PdfPCell(new Phrase("Header"));
                //tabFot.AddCell(cell);
                //tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
            }

            // write on start of each page
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                //base.OnStartPage(writer, document);
            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                //Paragraph para = new Paragraph("Footer", new Font());
                //para.Alignment = Element.ALIGN_CENTER;
                //document.Add(para);

                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = document.PageSize.Width;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cell = new PdfPCell();
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                footerTbl.AddCell(cell);
                var fontFormat = FontFactory.GetFont("Calibri", 8, BaseColor.LIGHT_GRAY);
                PdfPCell footer = new PdfPCell(new Paragraph(footerText, fontFormat));
                footer.Border = Rectangle.NO_BORDER;
                footer.HorizontalAlignment = Element.ALIGN_CENTER;
                footerTbl.AddCell(footer);
                footerTbl.WriteSelectedRows(0, -1, 0, (document.BottomMargin), writer.DirectContent);
            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
            }
        }
    }



    public class PageIndex
    {
        public string Text { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }
}
