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

namespace MTSEntBlocks.ExtractBlock
{
    public class ReportExtract
    {
        private Dictionary<string, object> _dicInputs;
        private XmlDocument _doc;
        private TemplateParser _tp;

        private DataSet _ds = null;

        public delegate string CustomBeforeTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public delegate string CustomTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public event CustomTokenHandler OnCustomToken;
        public event CustomBeforeTokenHandler OnBeforeCustomToken;

        public Dictionary<string, object> GetInputsValues
        {
            get { return _dicInputs; }
        }

        public ReportExtract(XmlDocument template)
        {
            _doc = template;
            _tp = new TemplateParser();
            _dicInputs = new Dictionary<string, object>();

            _tp.OnToken += TokenHandler;
            SetInputValues();
        }

       public string GenetateExtract(string outfile = "")
        {
            string extract = "";
            try
            {
                SetDataset();
                ReportExtractFile();
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

        public void SetInputValues()
        {
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
            {
                _dicInputs[node.GetAttribute("name")] = _tp.ParseString(node.InnerText);
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


        private void ReportExtractFile()
        {
            
            ReportDocument repdoc = new ReportDocument();
            
            int ReportExportType = int.Parse(_doc.SelectSingleNode("//outputs/output/exporttype").InnerText);
            string InputFileName =  string.Empty;
                  
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
                {
                    if (node.Attributes["name"].Value == "inputfile")
                        InputFileName = node.InnerText;
                    break;
                }

            repdoc.Load(InputFileName);
            repdoc.SetDataSource(_ds.Tables[0]);

            repdoc.ExportToDisk((ExportFormatType)Enum.Parse(typeof(ExportFormatType), ReportExportType.ToString()),GetOutputFileName());

               // _dicInputs["outputfile"] = configuredoutfilename;
            
            _dicInputs["ExtractFile"] = Path.GetDirectoryName(GetOutputFileName());
        }
    
        private string GetOutputFileName()
        {
            return _tp.ParseString(_doc.SelectSingleNode("//outfilename/file").InnerText);               
        }

        private string TokenHandler(string strToken, string strfmt, DataRow dr = null)
        {
            try
            {
                object value = null;
                string outstring = string.Empty;

                if (OnBeforeCustomToken != null)
                {
                    outstring = OnBeforeCustomToken(strToken, strfmt, outstring, dr);
                    return outstring;
                }

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
    }
}
