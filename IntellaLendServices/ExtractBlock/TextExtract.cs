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
    public class TextExtract
    {
        private Dictionary<string, object> _dicInputs;
        private XmlDocument _doc;
        private TemplateParser _tp;

        private DataSet _ds = null;

        public delegate string CustomBeforeTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public delegate string CustomTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public event CustomTokenHandler OnCustomToken;
        

        public Dictionary<string, object> GetInputsValues
        {
            get { return _dicInputs; }
        }

        public TextExtract(XmlDocument template)
        {
            _doc = template; 
            _tp = new TemplateParser();
            _dicInputs = new Dictionary<string, object>();

            _tp.OnToken += TokenHandler;
        }
        public TextExtract(Dictionary<string, object> Inputs, XmlDocument template)
        {
            _doc = template;
            _tp = new TemplateParser();
            _dicInputs = Inputs;

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

        public void SetFileTemplate(Dictionary<string, object> input)
        {
            foreach (KeyValuePair<string, object> kv in input)
            {
                if (kv.Value.GetType() == typeof(string))
                {
                    _dicInputs[kv.Key] = _tp.ParseString(Convert.ToString(kv.Value));
                }
                else
                {
                    _dicInputs[kv.Key] = kv.Value;
                }
            }
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

        public string GenetateExtract(ref string outfile )
        {
            string ext = "";
            try
            {
                SetDataset();
                ext = Extract(ref outfile);
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
            return ext;
        }

        private void SetInputValues()
        {
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
            {
                if(!_dicInputs.Keys.Contains(node.GetAttribute("name")))
                    _dicInputs[node.GetAttribute("name")] = _tp.ParseString(node.InnerText);
            }
        }

        private string TokenHandler(string strToken, string strfmt, DataRow dr = null)
        {
            object value = null;
            string outstring = string.Empty;
            
            try
            {
                if (strToken[0] == '#')
                {
                    //Remove the # and get value from input
                    strToken = strToken.Substring(1, strToken.Length - 1);
                    _dicInputs.TryGetValue(strToken, out value);
                    outstring = string.Format(strfmt, value);
                }
                else if (dr != null)
                {

                    if (strToken[0] == '@')
                    {
                        outstring = dr[strToken.Substring(1, strToken.Length - 1)].ToString().Trim();
                    }
                    else
                    {
                    int outLength = string.Format(strfmt, value).Length;
                    //if (dr[strToken] is DBNull)                      
                        outstring = string.Format(strfmt, dr[strToken] != null ? dr[strToken] : strToken);
                        if (outLength != 0)
                            outstring.Substring(0, outLength);
                    }
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
                throw new Exception(string.Concat("Token:", strToken, "Format String: ", strfmt, "Input String :",Convert.ToString(value),"Output String :", outstring), ex);
            }
        }

        private void SetDataset()
        {
            string spName;
            StringBuilder sb = new StringBuilder();
            List<object> spParams = new List<object>();

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

        private string Extract(ref string OutFile )
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
                OutFile = outputfilename;

                return sb.ToString();
        }

        private string GetOutputFileName()
        {
            return _tp.ParseString(_doc.SelectSingleNode("//outfilename/file").InnerText);               
        }


    }
}
