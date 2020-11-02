using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace MTSEntBlocks.UtilsBlock
{
    public class PDFMerger
    {
        Document document;
        PdfCopy writer;
        FileStream pdfStrem;
        int pageCount = 0;

        public PDFMerger(string outFileName)
        {
            document = new Document();
            pdfStrem = new FileStream(outFileName, FileMode.Create);
            writer = new PdfCopy(document, pdfStrem);            
        }

        public void OpenDocument()
        {
            document.Open();
        }

        public void SaveDocument()
        {            
            if (pageCount == 0)
            {
                if(pdfStrem!=null)
                    pdfStrem.Close();
            }
            else
            {
                if(writer!=null)
                    writer.Close();
                if(document!=null)
                    document.Close();
            }
        }

        public void AppendPDF(byte[] pdf)
        {   
			PdfReader.unethicalreading = true;		
            PdfReader reader = new PdfReader(pdf);
            reader.ConsolidateNamedDestinations();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                writer.AddPage(page);
            }
            reader.Close();
            pageCount++;
        }

        public void AppendPDF(byte[] pdf,ref int lastPgNo,  ref List<Int32> pgNos)
        {
			PdfReader.unethicalreading = true;
            PdfReader reader = new PdfReader(pdf);
            reader.ConsolidateNamedDestinations();
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                PdfImportedPage page = writer.GetImportedPage(reader, i);
                writer.AddPage(page);
                pgNos.Add(lastPgNo);
                lastPgNo++;      
            }
            reader.Close();
            pageCount++;
        }
    }
}