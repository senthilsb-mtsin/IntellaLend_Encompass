using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFConversionCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PDFMerger merger = new PDFMerger(@"C:\Users\mtsuser\Downloads\PADILLA\T1_01.pdf");
            merger.OpenDocument();
            string[] fileItems = Directory.GetFiles(@"C:\Users\mtsuser\Downloads\PADILLA\PADILLA", "*.pdf", SearchOption.AllDirectories);
            string fileNames = string.Empty;
            foreach (var item in fileItems)
            {
                try
                {
                    merger.AppendPDF(File.ReadAllBytes(item));
                }
                catch (Exception ex)
                {
                    fileNames += item + Environment.NewLine;
                }
               
            }
            merger.SaveDocument();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Int32 _min = 15;
            Int32 _milliSec = 15 * 60000;
            Int32 _milliSe1c = 25 * 60000;

            string parentPath = @"D:\MTS\BOX Files\PageWiseTIFF";
            string folderPath = @"D:\UploadIssue\";
            string pdfPath = @"D:\UploadIssue\invoice batch as of 20180517.pdf";

            //FileInfo fileInfo = new FileInfo(pdfPath);

            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            Int32 tiffGenerated = CreateTiffBatchFolder(pdfPath, folderPath, "Test2");

            //if (tiffGenerated != 0)
            //{
            //    if (Directory.Exists(folderPath))
            //        Directory.Delete(folderPath, true);

            //    //copy file to Ephesoft folder
            //    File.Copy(pdfPath, Path.Combine(parentPath, fileInfo.Name));
            //}

            //Directory.Move(folderPath, Path.Combine(Path.Combine(EphesoftInputPath, "Input", _file)));
        }

        private int CreateTiffBatchFolder(string originalFile, string ephsoftTempPath, string filename)
        {
            string GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
            //String ars = $"-dNOPAUSE -sDEVICE=tiffscaled8 -dEmbedAllFonts=true -sCompression=lzw -r300 -sOutputFile=\"{ephsoftTempPath}%d.tif\" -sPAPERSIZE=a4 \"{originalFile}\" -c quit";
            String ars = $"-dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=\"{Path.Combine(ephsoftTempPath, filename)}.pdf\" -sPAPERSIZE=a4 \"{originalFile}\" -c quit";
            Process proc = new Process();
            proc.StartInfo.FileName = GhostScriptPath;
            proc.StartInfo.Arguments = ars;

            //proc.StartInfo.CreateNoWindow = true;
            // proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            //proc.WaitForExit(60000);
            proc.WaitForExit();
            //Environment.FailFast("Takes too Long");


            //if ()

            bool isRunning = !proc.HasExited;
            if (isRunning)
                proc.Kill();
            return isRunning ? -1 : proc.ExitCode;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string folderPath = @"D:\MTS\BOX Files\PageWiseTIFF\T1_01";
            List<byte[]> _tiffByteArry = new List<byte[]>();
            string[] _tiffFiles = Directory.GetFiles(folderPath, "*.tif", SearchOption.AllDirectories);
            foreach (string file in _tiffFiles)
            {
                _tiffByteArry.Add(File.ReadAllBytes(file));
            }

            byte[] pdfByte = CreatePdfBytes(_tiffByteArry);

            File.WriteAllBytes(@"D:\MTS\BOX Files\LoanPDF\01.pdf", pdfByte);
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
    }
}
