using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.Reflection;
using MTSEntBlocks.UtilsBlock;
using MTSEntBlocks.DataBlock;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MTSEntBlocks.ExtractBlock
{
    public class ExtractHandler
    {
        protected XmlDocument _doc = new XmlDocument();
        protected TemplateParser _tp;
        protected Dictionary<string, object> _dicInputs;
        private Dictionary<string, string> dicCols;
        private DataSet _ds = null;

        public delegate string CustomTextTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public event CustomTextTokenHandler OnCustomExtToken;

        public ExtractHandler()
        {
            _tp = new TemplateParser();
            _dicInputs = new Dictionary<string, object>();
            _tp.OnToken += TokenHandler;
        }
        public Dictionary<string, object> GetInputsValues
        {
            get { return _dicInputs; }
        }
        public string GenerateExtract(Dictionary<string, object> input, string TemplateFileName)
        {
            string outputType = string.Empty;
            bool customhandler = false;
            _doc.Load(TemplateFileName);
            XmlNode extracttype = _doc.SelectSingleNode("//extract");

            if (extracttype != null)
            {
                XmlElement ele = extracttype as XmlElement;
                outputType = ele.GetAttribute("type");
                customhandler = bool.Parse(ele.GetAttribute("customhandler"));
            }

            switch (outputType)
            {
                case "text":
                    {
                        string outFileName = string.Empty;
                        string extract = string.Empty;

                        TextExtract txtExt = new TextExtract(_doc);

                        txtExt.SetFileTemplate(input);

                        if (customhandler)
                        {
                            txtExt.OnCustomToken += CustomTextHandler;
                        }
                        extract = txtExt.GenetateExtract(ref outFileName);

                        _dicInputs["ExtractFile"] = outFileName;
                        return extract;
                    }
                case "excel":
                    {
                        string outFileName = string.Empty;
                        string extract = string.Empty;

                        ExcelExtract txtExt = new ExcelExtract(_doc);

                        txtExt.SetFileTemplate(input);

                        if (customhandler)
                        {
                            txtExt.OnCustomToken += CustomTextHandler;
                        }
                        extract = txtExt.GenetateExtract(ref outFileName);

                        _dicInputs["ExtractFile"] = outFileName;
                        return extract;
                    }
                case "report":
                    {
                        ReportExtract rptExt = new ReportExtract(_doc);
                        rptExt.GenetateExtract();
                        return string.Empty;
                    }
                case "image":
                    {
                        ImageExtract imgExt = new ImageExtract(_doc);
                        imgExt.GenetateExtract();
                        return string.Empty;
                    }
                case "complex":
                    {
                        _dicInputs["ExtractFile"] = GenerateComplexExtract(input, TemplateFileName);
                        return string.Empty;
                    }
            }
            return string.Empty;
        }

        public string GenerateComplexExtract(Dictionary<string, object> input, string TemplateFileName)
        {
            string outputType = string.Empty;
            string outFileName = string.Empty;

            dicCols = new Dictionary<string, string>();

            SetDataset();

            string TempFolderPath = string.Empty;

            foreach (XmlElement xnode in _doc.SelectNodes("//inputs/input"))
            {
                if (xnode.Attributes["name"].Value == "tempfolderpath")
                {
                    TempFolderPath = xnode.InnerText;
                    if (!Directory.Exists(TempFolderPath))
                        Directory.CreateDirectory(TempFolderPath);
                    break;
                }
            }

            foreach (DataRow dr in _ds.Tables[0].Rows)
            {
                SetFileTemplate(input);
                foreach (DataColumn col in _ds.Tables[0].Columns)
                {
                    _dicInputs.Add(col.ColumnName, dr[col.ColumnName]);
                }

                //_dicInputs["outputfile"] = dr[_dicInputs["outputfile"].ToString()];

                XmlNodeList nodelist = _doc.SelectNodes("//outputs/outputgroup");
                int outputgroupidx = 0;
                foreach (XmlNode node in nodelist)
                {
                    outputgroupidx = outputgroupidx + 1;
                    if (node != null)
                    {
                        outputType = node.SelectSingleNode("extract").Attributes.Item(0).Value;
                    }

                    switch (outputType)
                    {
                        case "text":
                            {
                                string extract = string.Empty;
                                TextExtract txtExt = new TextExtract(_dicInputs,_doc);
                             //  txtExt.SetFileTemplate(input);
                                extract = txtExt.GenetateExtract(ref outFileName);
                                _dicInputs["ExtractFile"] = outFileName;
                                break;
                            }
                        case "report":
                            {
                                ReportComplexExtract rptExt = new ReportComplexExtract(_dicInputs, TemplateFileName);
                                rptExt.GenetateExtract(outputgroupidx, node, TempFolderPath, ref outFileName);
                                break;
                            }
                        case "image":
                            {
                                ImageComplexExtract imgExt = new ImageComplexExtract(_dicInputs, TemplateFileName);
                                imgExt.GenetateExtract(node, TempFolderPath);
                                break;
                            }
                    }
                }

                outFileName = GetOutputFileName();

                string ConsolidatedFiles = _doc.SelectSingleNode("//mtsextract/extract").Attributes.GetNamedItem("consolidatefiles").Value;
                if (ConsolidatedFiles == "all")
                {
                    DoConsolidateFiles(TempFolderPath, outFileName);
                }

                _dicInputs.Clear();
            }
            return string.IsNullOrEmpty(outFileName) ? "" : Path.GetDirectoryName(outFileName);
        }

        private void SetDataset()
        {
            string spName;
            StringBuilder sb = new StringBuilder();
            List<object> spParams = new List<object>();
            SetInputValues();
            spName = _doc.SelectSingleNode("//datamap/storedprocedure/name").InnerText;

            foreach (XmlNode node in _doc.SelectNodes("//datamap/storedprocedure/params/param"))
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    spParams.Add(_tp.ParseString(node.InnerText));
                }
            }

            if (spParams.Count == 0)
            {
                _ds = DataAccess.ExecuteDataset(spName);
            }
            else
            {
                _ds = DataAccess.ExecuteDataset(spName, spParams.ToArray());
            }

            foreach (XmlElement node in _doc.SelectNodes("//datamap/relations/relation"))
            {
                string name = node.GetAttribute("name");
                string column = node.GetAttribute("column");
                int parenttableindex = Convert.ToInt16(node.GetAttribute("parenttableindex"));
                int childtableindex = Convert.ToInt16(node.GetAttribute("childtableindex"));

                _ds.Relations.Add(name, _ds.Tables[parenttableindex].Columns[column], _ds.Tables[childtableindex].Columns[column]);
            }


        }

        public void SetFileTemplate(Dictionary<string, object> input)
        {
            foreach (KeyValuePair<string, object> kv in input)
            {
                _dicInputs[kv.Key] = kv.Value;
            }
            SetInputValues();
        }

        protected void SetInputValues()
        {
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
            {
                _dicInputs[node.GetAttribute("name")] = _tp.ParseString(node.InnerText);
            }
        }

        private void DoConsolidateFiles(string TempFolderPath, string outfilename)
        {
            string[] Files = Directory.GetFiles(TempFolderPath);
            CombineMultiplePDFs(Files, outfilename);

            foreach (string file in Files)
            {
                File.Delete(file);
            }
        }

        public void CombineMultiplePDFs(string[] fileNames, string outFile)
        {
            PdfReader.unethicalreading = true;
            int pageOffset = 0;
            ArrayList master = new ArrayList();
            int f = 0;
            Document document = null;
            PdfReader reader = null;
            PdfCopy writer = null;
            FileStream sfile = null;
            try
            {
                while (f < fileNames.Length)
                {
                    try
                    {
                        sfile = CreateStreamFromFile(fileNames[f]);
                        try
                        {
                            // we create a reader for a certain document 
                            // Check if PDF file is good, if exception assign default bad pdf file
                            reader = new PdfReader(sfile);
                        }
                        catch (BadPasswordException)
                        {
                            sfile = CreateStreamFromFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + "BadFile.pdf");
                            reader = new PdfReader(sfile);
                        }
                        reader.ConsolidateNamedDestinations();
                        // we retrieve the total number of pages
                        int n = reader.NumberOfPages;
                        pageOffset += n;
                        if (document == null)
                        {
                            // step 1: creation of a document-object
                            document = new Document(reader.GetPageSizeWithRotation(1));
                            // step 2: we create a writer that listens to the document
                            writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                            // step 3: we open the document
                            document.Open();
                        }
                        // step 4: we add content
                        for (int i = 0; i < n; )
                        {
                            ++i;
                            if (writer != null)
                            {
                                PdfImportedPage page = writer.GetImportedPage(reader, i);
                                writer.AddPage(page);
                            }
                        }
                        PRAcroForm form = reader.AcroForm;
                        if (form != null && writer != null)
                        {
                            writer.CopyAcroForm(reader);
                        }
                        f++;
                        sfile.Close();
                        sfile = null;
                        reader.Dispose();
                    }
                    catch (Exception)
                    {
                        // Any other exception file will be skiped
                        f++;
                        sfile.Close();
                        sfile = null;
                        continue;
                    }
                }
                // step 5: we close the document
                if (document != null)
                {
                    document.Close();
                }
            }
            finally
            {
                sfile = null;
                document = null;
                reader = null;
                writer = null;
            }

        }

        protected string GetOutputFileName()
        {
            return _tp.ParseString(_doc.SelectSingleNode("//outfilename/file").InnerText);
        }

        protected string TokenHandler(string strToken, string strfmt, DataRow dr = null)
        {
            try
            {
                object value = null;
                string outstring = string.Empty;

                if (strToken[0] == '#')
                {
                    //Remove the # and get value from input
                    strToken = strToken.Substring(1, strToken.Length - 1);
                    _dicInputs.TryGetValue(strToken, out value);
                    outstring = string.Format(strfmt, value);
                }
                else if (dr != null)
                {
                    int outLength = string.Format(strfmt, value).Length;
                    //if (dr[strToken] is DBNull)                      
                    outstring = string.Format(strfmt, dr[strToken] != null ? dr[strToken] : strToken).Substring(0, outLength);

                }
                else
                {
                    _dicInputs.TryGetValue(strToken, out value);
                    outstring = string.Format(strfmt, value);
                }

                //chaining of custom hook

                return outstring;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Format String: ", strfmt, "Input String :", dr[strToken]), ex);
            }
        }

        public string CustomTextHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null)
        {
            return OnCustomExtToken(strToken, strfmt, parseredOutput, dr);
        }

        private FileStream CreateStreamFromFile(string Filename)
        {
            FileStream fs = new FileStream(Filename, FileMode.Open);
            return fs;
        }
    }
}
