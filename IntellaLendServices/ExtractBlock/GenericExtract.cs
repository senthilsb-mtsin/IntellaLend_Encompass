using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Reflection;

using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.UtilsBlock;

namespace MTSEntBlocks.ExtractBlock
{
    public class GenericExtract
    {
        private Dictionary<string, object> _dicInputs;
        private XmlDocument _doc;
        private TemplateParser _tp;

        private DataSet _ds = null;

        public delegate string CustomTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public event CustomTokenHandler OnCustomToken;

        public Dictionary<string, object> GetInputsValues
        {
            get { return _dicInputs; }
        }

        public GenericExtract()
        {
            _doc = new XmlDocument();
            _tp = new TemplateParser();
            _dicInputs = new Dictionary<string, object>();

            _tp.OnToken += TokenHandler;
        }

        public void SetStringTemplate(Dictionary<string, object> input, string template)
        {
            foreach (KeyValuePair<string, object> kv in input)
            {
                _dicInputs[kv.Key] = kv.Value;
            }
            SetTemplate(template);
            SetInputValues();
        }

        public void SetFileTemplate(Dictionary<string, object> input, string templateFile)
        {
            foreach (KeyValuePair<string, object> kv in input)
            {
                _dicInputs[kv.Key] = kv.Value;
            }
            SetTemplate(templateFile, true);
            SetInputValues();
        }

        private void SetTemplate(string template, bool isTemplateFile = false)
        {
            try
            {
                    if (isTemplateFile)
                    {
                        _doc.Load(template);
                    }
                    else
                    {
                        _doc.LoadXml(template);
                    }
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

        }

        public string GenetateExtract(string outfile = "")
        {
            string extract = "";
            try
            {
                SetDataset();
                string outputType = "Data";
                XmlNode extracttype  = _doc.SelectSingleNode("//extract");
                
                if (extracttype != null)
                {
                    XmlElement ele = extracttype as XmlElement;
                    outputType = ele.GetAttribute("type");
                }

                if (outputType == "Image")
                    ImageExtract();
                else
                    extract = DataExtract(outfile);

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

        private void SetInputValues()
        {
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
            {
                _dicInputs[node.GetAttribute("name")] = _tp.ParseString(node.InnerText);
            }
        }

        private string TokenHandler(string strToken, string strfmt, DataRow dr = null)
        {
            try
            {
                object value = null;
                string outstring;

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
                if (OnCustomToken != null)
                {
                    outstring = OnCustomToken(strToken, strfmt, outstring, dr);
                }
                return outstring;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Format String: ", strfmt, "Input String :", dr[strToken]), ex);
            }
        }

        private void SetDataset()
        {
            string spName;
            StringBuilder sb = new StringBuilder();
            List<object> spParams = new List<object>();

            spName = _doc.SelectSingleNode("//datamap/storedprocedure/name").InnerText;

            foreach (XmlNode node in _doc.SelectNodes("//datamap/storedprocedure/params"))
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

        private string DataExtract(string OutFile = "")
        {

            StringBuilder sb = new StringBuilder();
            foreach (XmlNode node in _doc.SelectNodes("//outputs/output"))
            {
                XmlElement ele = node as XmlElement;
                string outputType = ele.GetAttribute("type");
                string outLine = string.Empty;
                int tableindex = -1;
                switch (outputType)
                {
                    case "single":
                        {
                            tableindex = Convert.ToInt16(ele.GetAttribute("tableindex"));

                            foreach (DataRow row in _ds.Tables[tableindex].Rows)
                            {
                                outLine = _tp.ParseString(node.InnerText, row);
                                if (outLine.Length > 0)
                                    sb.AppendLine(outLine);
                            }
                            break;
                        }
                    case "parentchild":
                        {
                            bool tail = false;
                            tableindex = Convert.ToInt16(ele.GetAttribute("tableindex"));
                            string relationName = ele.GetAttribute("relationname");

                            ele = node.SelectSingleNode("parentheader") as XmlElement;
                            string masterTemplate = ele.InnerText;

                            ele = node.SelectSingleNode("detail") as XmlElement;
                            string detailTemplate = ele.InnerText;

                            ele = node.SelectSingleNode("parenttailer") as XmlElement;
                            tail = (ele != null);

                            foreach (DataRow masterRow in _ds.Tables[tableindex].Rows)
                            {
                                //Parent Header row
                                outLine = _tp.ParseString(masterTemplate, masterRow);
                                if (outLine.Length > 0)
                                    sb.AppendLine(outLine);
                                foreach (DataRow childrow in masterRow.GetChildRows(relationName))
                                {
                                    //Child row
                                    outLine = _tp.ParseString(detailTemplate, childrow);
                                    if (outLine.Length > 0)
                                        sb.AppendLine(outLine);
                                }
                                if (tail)
                                {
                                    string tailTemplate = ele.InnerText;
                                    //Parent Tailer row
                                    outLine = _tp.ParseString(tailTemplate, masterRow);
                                    if (outLine.Length > 0)
                                        sb.AppendLine(outLine);
                                }

                            }
                            break;
                        }
                    default://type="text"
                        {
                            outLine = _tp.ParseString(node.InnerText);
                            if (outLine.Length > 0)
                                sb.AppendLine(outLine);
                            break;
                        }
                }

            }

            if (!bool.Parse(_doc.SelectSingleNode("//outfilename").Attributes.GetNamedItem("generateempty").Value) && (sb.ToString().Trim() == string.Empty))
            {
                _dicInputs["ExtractFile"] = string.Empty;
                return string.Empty;
            }
                string outputfilename = OutFile;
                if (outputfilename.Trim().Length == 0)
                {
                    outputfilename = GetOutputFileName();
                }
                if (!Directory.Exists(Path.GetDirectoryName(outputfilename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(outputfilename));

                using (TextWriter writer = File.CreateText(outputfilename))
                {
                    writer.Write(sb.ToString());
                }
                _dicInputs["ExtractFile"] = outputfilename;

                return sb.ToString();
        }

        private void ImageExtract()
        {
            string dllName = _doc.SelectSingleNode("//outputs/output/dllname").InnerText;
            string className = _doc.SelectSingleNode("//outputs/output/classname").InnerText;
            int ReportExportType = int.Parse(_doc.SelectSingleNode("//outputs/output/exporttype").InnerText);
            
            List<object> spParams = new List<object>();

            string spName = _doc.SelectSingleNode("//outputs/output/storedprocedure/name").InnerText;
            foreach (XmlNode node in _doc.SelectNodes("//outputs/output/storedprocedure/params"))
            {
                if (!string.IsNullOrEmpty(node.InnerText))
                {

                    spParams.Add(_tp.ParseString(node.InnerText));
                }
            }

            Assembly assembly = Assembly.LoadFile(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\" + dllName);
            Type type = assembly.GetType(className);
            object objimageextractdll = Activator.CreateInstance(type);

            string configuredoutfilename = _dicInputs["outputfile"].ToString();    
            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                _dicInputs["outputfile"] = row[_dicInputs["outputfile"].ToString()];

                string ofile = _dicInputs["outputfile"].ToString();

                MethodInfo method = type.GetMethod("CreateImageExtract");
                method.Invoke(objimageextractdll, new object[] { row, ReportExportType, _dicInputs["inputfile"].ToString(), GetOutputFileName(), spName, spParams });
                _dicInputs["outputfile"] = configuredoutfilename;
             }
            _dicInputs["ExtractFile"] = Path.GetDirectoryName(GetOutputFileName());
        }

        private string GetOutputFileName()
        {
            return _tp.ParseString(_doc.SelectSingleNode("//outfilename/file").InnerText);               
        }

    }
}
