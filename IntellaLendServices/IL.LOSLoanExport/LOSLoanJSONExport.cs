using SmartFormat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;

namespace IL.LOSLoanExport
{
    public class LOSLoanJSONExport
    {
        LOSLoanJSONExportData _dataAccess = null;

        public LOSLoanJSONExport(string _tenantSchema)
        {
            _dataAccess = new LOSLoanJSONExportData(_tenantSchema);
            Smart.Default.Parser.UseAlternativeBraces('$', '^');
        }

        public string ExportJSON(Int64 LoanID, string ExportPath)
        {
            string TemplateJson = GetExportJSONStructure();
            Dictionary<string, object> loanDic = _dataAccess.GetLoanData(LoanID, ExportPath);
            return Smart.Format(TemplateJson, loanDic);
        }


        private string GetExportJSONStructure()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["LOSTemplateBaseUrl"] + "LOSExportJSONStructure");
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            StreamReader streader = new System.IO.StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            return streader.ReadToEnd().Trim();
        }
    }
}
