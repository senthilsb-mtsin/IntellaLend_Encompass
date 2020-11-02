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
    public class ImageExtract
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

        public ImageExtract(XmlDocument template)
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
                ImagetExtractFile();
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


        private void ImagetExtractFile()
        {
            int imgseq = 0;
            string ImageFileName =  string.Empty;


            
            foreach (XmlElement node in _doc.SelectNodes("//inputs/input"))
                {
                    if (node.Attributes["name"].Value == "imagefilename")
                        ImageFileName = node.InnerText;
                    break;
                }

            foreach (DataRow row in _ds.Tables[0].Rows)
            {
                    imgseq = imgseq + 1;
                    object objImg = row["upload"];
                    if (!objImg.Equals(DBNull.Value))
                    {
                        byte[] img = (byte[])objImg;
                        string filename = row["FILENAME"].ToString();

                        FileStream fspdf = new FileStream(GetOutputFileName() +   ImageFileName  , FileMode.Create);
                            fspdf.Write(img, 0, img.Length);
                            fspdf.Flush();
                            fspdf.Close();        
                    }
            }
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
