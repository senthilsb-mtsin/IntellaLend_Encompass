using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.UtilsBlock;

namespace MTSEntBlocks.ExtractBlock
{
    public class ComplexExtract
    {
        protected XmlDocument _doc = new XmlDocument();
        protected TemplateParser _tp;
        protected Dictionary<string, object> _dicInputs;
        

        public ComplexExtract()
        {
            _tp = new TemplateParser();
            _tp.OnToken += TokenHandler;
        }

        protected DataSet SetDatasetForSubExtracts(XmlNode node)
        {
            string spName;
            DataSet ds;
            StringBuilder sb = new StringBuilder();
            List<object> spParams = new List<object>();

            spName = node.SelectSingleNode("storedprocedure/name").InnerText;

            foreach (XmlNode xnode in node.SelectNodes("storedprocedure/params/param"))
            {
                if (!string.IsNullOrEmpty(xnode.InnerText))
                {
                    spParams.Add(_tp.ParseString(xnode.InnerText));
                }
            }

            if (spParams.Count == 0)
            {
                ds = DataAccess.ExecuteDataset(spName);
            }
            else
            {
                ds = DataAccess.ExecuteDataset(spName, spParams.ToArray());
            }

            foreach (XmlElement xnode in node.SelectNodes("storedprocedure/relations/relation"))
            {
                string name = xnode.GetAttribute("name");
                string column = xnode.GetAttribute("column");
                int parenttableindex = Convert.ToInt16(xnode.GetAttribute("parenttableindex"));
                int childtableindex = Convert.ToInt16(xnode.GetAttribute("childtableindex"));

                ds.Relations.Add(name, ds.Tables[parenttableindex].Columns[column], ds.Tables[childtableindex].Columns[column]);
            }
            return ds;
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
    }
}