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
    public class TextComplexExtract : ComplexExtract
    {
        
        private DataSet _ds = null;

        public delegate string CustomBeforeTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public delegate string CustomTokenHandler(string strToken, string strfmt, string parseredOutput, DataRow dr = null);
        public event CustomTokenHandler OnCustomToken;
        public event CustomBeforeTokenHandler OnBeforeCustomToken;

        public TextComplexExtract(Dictionary<string, object> Inputs,string TemplateFileName )
        {
            base._dicInputs = Inputs;
        }
    

        public string GenetateExtract(XmlNode node,string outfile = "")
        {
            string extract = "";
            try
            {
                _ds = SetDatasetForSubExtracts(node);
                extract = DataExtract(node,outfile);
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

        private string DataExtract(XmlNode node, string OutFile = "")
        {

                StringBuilder sb = new StringBuilder();
            
                XmlElement ele = node.SelectSingleNode("output") as XmlElement;
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

             

                return sb.ToString();
        }

        private string GetOutputFileName()
        {
            return _tp.ParseString(_doc.SelectSingleNode("//outfilename/file").InnerText);
        }
    }
}
