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
    public class ImageComplexExtract : ComplexExtract
    {
        private DataSet _ds = null;

        public ImageComplexExtract(Dictionary<string, object> Inputs, string TemplateFileName)
        {
            base._dicInputs = Inputs;
        }

        public string GenetateExtract(XmlNode node, string outfile = "")
        {
            string extract = "";
            try
            {
                _ds = SetDatasetForSubExtracts(node);
                ImagetExtractFile(node);
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


        private void ImagetExtractFile(XmlNode node)
        {
            int imgseq = 0;
            string ImageFileName = string.Empty;

            string strFileNameField = node.SelectSingleNode("filenamefield").InnerText;
            string strImageField = node.SelectSingleNode("imagefield").InnerText;
            int tableIndex = Convert.ToInt16(node.Attributes["tableindex"]);
            foreach (DataRow row in _ds.Tables[tableIndex].Rows)
            {
                imgseq = imgseq + 1;
                object objImg = row[strImageField];
                if (!objImg.Equals(DBNull.Value))
                {
                    byte[] img = (byte[])objImg;
                    string filename = row["FILENAME"].ToString();
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    FileStream fspdf = new FileStream(row[strFileNameField].ToString(), FileMode.Create);
                    fspdf.Write(img, 0, img.Length);
                    fspdf.Flush();
                    fspdf.Close();
                }
            }
        }
    }
}
