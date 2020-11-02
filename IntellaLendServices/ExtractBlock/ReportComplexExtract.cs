using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.UtilsBlock;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MTSEntBlocks.ExtractBlock
{
    public class ReportComplexExtract : ComplexExtract
    {

        private DataSet _ds = null;

        public ReportComplexExtract(Dictionary<string, object> Inputs, string FileName)
        {
            base._dicInputs = Inputs;
        }

       public string GenetateExtract(int outputgroupidx, XmlNode node,string TempFolderPath,ref string outfile)
        {
            string extract = "";
            try
            {
                _ds = SetDatasetForSubExtracts(node);
                ReportExtractFile(outputgroupidx,node, TempFolderPath, ref outfile);
            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = BaseExceptionHandler.HandleException(ref ex);
                if (rethrow)
                {
                    throw ex;
                }
            }
            return extract;
        }

       private void ReportExtractFile(int outputgroupidx,XmlNode node, string TempFolderPath, ref string outfilename)
        {
    
                string ReportExportType = node.SelectSingleNode("exporttype").InnerText;                
                
                int intFileName = 0;

                XmlNodeList nodelist = node.SelectNodes("parentchild/parent");
                if ( nodelist.Count  > 0)
                {    

                    foreach(XmlNode parent in nodelist)
                    {
                    string parenttype =  parent.Attributes.GetNamedItem("type").Value;
                    int tableidx = int.Parse(parent.Attributes.GetNamedItem("tableidx").Value);

                    if (parenttype == "report")
                    {
                        string rptname = parent.SelectSingleNode("filename").InnerText;
                        CreateReport(_ds.Tables[tableidx], rptname, ReportExportType, TempFolderPath, outputgroupidx, intFileName);
                    }

                    XmlNodeList childlist = parent.SelectNodes("child");
                    if (childlist.Count > 0)
                    {
                        foreach (DataRow masterRow in _ds.Tables[tableidx].AsEnumerable())
                        {

                            foreach (XmlNode xnode in childlist)
                            {
                                string relationName = xnode.Attributes.GetNamedItem("relationname").Value;
                                string ChildType = xnode.Attributes.GetNamedItem("type").Value;

                                DataTable dtTemp = new DataTable();


                                DataRow[] drchild = masterRow.GetChildRows(relationName);

                                if (drchild.Length > 0)
                                {
                                    dtTemp = drchild.CopyToDataTable();

                                    if (ChildType == "report")
                                    {
                                        string rptname = xnode.SelectSingleNode("filename").InnerText;
                                        CreateReport(dtTemp, rptname, ReportExportType, TempFolderPath, outputgroupidx, intFileName);
                                    }
                                    else if (ChildType == "image")
                                    {

                                        string strFileNameField = xnode.SelectSingleNode("filenamefield").InnerText;
                                        string strImageField = xnode.SelectSingleNode("imagefield").InnerText;
                                        CreateImage(dtTemp, strImageField, strFileNameField, TempFolderPath, outputgroupidx, intFileName);

                                    }
                                    intFileName = intFileName + 1;
                                }
                            }
                        }
                    }
                    }
                }
                else
                {
                        string InputFileName = node.SelectSingleNode("inputfilename").InnerText;
                        CreateReport(_ds.Tables[0], InputFileName, ReportExportType, TempFolderPath, outputgroupidx, intFileName); 
                }
                
        }

        public static void ConvertImageToPDF(byte[] ImgArr, string FilePath, string Filename)
        {
            Document document;
                Image img = GetImage(ImgArr);

            if (img.Width > PageSize.A4.Width || img.Height > PageSize.A4.Height)
            {
                document = new Document(PageSize.LEGAL, 5, 50, 5, 50);
                float ImgHeight = img.Height / img.Width * document.PageSize.Width;
                img.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, ImgHeight);
            }
            else
            {
                document = new Document();
            }

            PdfWriter.GetInstance(document, new FileStream(FilePath + Filename, FileMode.Create));
            document.Open();
            document.Add(img);
            document.Close();
        }

        private static Image GetImage(byte[] Img)
        {
            Image img;
            try
            {
                 img = Image.GetInstance(Img);
            }
            catch 
            {
                img = Image.GetInstance(File.ReadAllBytes(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\"+"imgnotavailable.jpg"));
            }
            return img;
        }

        private void CreateReport(DataTable dtdata, string ReportFileName, string reporttype,string tempfolderpath,int outputgroupidx, int idx)
        {
            ReportDocument repdoc = new ReportDocument();
            try
            {
                repdoc.Load(ReportFileName);
                repdoc.SetDataSource(dtdata);
                repdoc.ExportToDisk((ExportFormatType)Enum.Parse(typeof(ExportFormatType), reporttype), tempfolderpath + outputgroupidx + "_" + idx.ToString());
            }
            finally
            {
                repdoc.Close();
                repdoc.Dispose();
                GC.Collect();
            }
        }

        private void CreateImage(DataTable dtdata, string imagefield, string filenamefield , string tempfolderpath, int outputgroupidx, int idx)
        {
            foreach (DataRow row in dtdata.AsEnumerable())
            {
                object objImg = row[imagefield];

                if (!objImg.Equals(DBNull.Value))
                {
                    byte[] img = (byte[])objImg;
                    string filename = row[filenamefield].ToString();

                    if (Path.GetExtension(filename).ToUpper() == ".PDF")
                    {
                        FileStream fspdf = new FileStream(tempfolderpath + outputgroupidx + "_" + filename, FileMode.Create);
                        fspdf.Write(img, 0, img.Length);
                        fspdf.Flush();
                        fspdf.Close();
                    }
                    else
                    {
                        ConvertImageToPDF(img, tempfolderpath, string.Concat(outputgroupidx + "_" + idx.ToString(), "_0_", filename));
                    }
                }
            }
        }

    }
}
