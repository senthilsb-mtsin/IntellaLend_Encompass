using IL.EmailManager;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

namespace IL.EmailTrackerTemplate
{
    public class EmailXMLTemplate
    {
        public Template CreateEmailTemplateAndProcess(string htmlpage, string queryStr)
        {
            XmlDocument _doc = new XmlDocument();
            string xmlString = GetEmailBody(htmlpage, queryStr);
            if (xmlString.StartsWith("Error:"))
                throw new Exception(xmlString);
            _doc.LoadXml(xmlString);
            return FillEmailTemplate(_doc, queryStr);
        }


        private string GetEmailBody(string page, string queryStr)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["EmailTemplateBaseUrl"] + page + "?ID=" + HttpUtility.UrlEncode(queryStr));

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            StreamReader streader = new System.IO.StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.UTF8);

            return streader.ReadToEnd();
        }

        private MemoryStream GetEmailAttachment(string page, string queryStr)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["EmailTemplateBaseUrl"] + page + "?ID=" + HttpUtility.UrlEncode(queryStr));

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            MemoryStream memStream;
            using (Stream response = webResponse.GetResponseStream())
            {
                memStream = new MemoryStream();
                byte[] buffer = new byte[1024];
                int byteCount;
                do
                {
                    byteCount = response.Read(buffer, 0, buffer.Length);
                    memStream.Write(buffer, 0, byteCount);
                } while (byteCount > 0);
            }

            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        private Template FillEmailTemplate(XmlDocument TemplateAndData, string queryString)
        {
            Template template = new Template();
            template.From = TemplateAndData.SelectSingleNode("email/From").InnerText != "" ? TemplateAndData.SelectSingleNode("email/From").InnerText : template.From;
            template.To = TemplateAndData.SelectSingleNode("email/To").InnerText != "" ? TemplateAndData.SelectSingleNode("email/To").InnerText : template.To;
            template.Cc = TemplateAndData.SelectSingleNode("email/Cc").InnerText != "" ? TemplateAndData.SelectSingleNode("email/Cc").InnerText : template.Cc;
            template.BCc = TemplateAndData.SelectSingleNode("email/BCc").InnerText != "" ? TemplateAndData.SelectSingleNode("email/BCc").InnerText : template.BCc;
            template.Subject = TemplateAndData.SelectSingleNode("email/Subject").InnerText != "" ? TemplateAndData.SelectSingleNode("email/Subject").InnerText : template.Subject;
            template.Body = TemplateAndData.SelectSingleNode("email/Body").InnerText != "" ? TemplateAndData.SelectSingleNode("email/Body").InnerText : template.Body;
            if (TemplateAndData.SelectSingleNode("email/AttachmentPath") != null)
            {
                template.Attachment = GetEmailAttachment(TemplateAndData.SelectSingleNode("email/AttachmentPath").InnerText, queryString);
                template.AttachmnetName = TemplateAndData.SelectSingleNode("email/AttachmentName").InnerText != "" ? TemplateAndData.SelectSingleNode("email/AttachmentName").InnerText : "AttachmentFile";
            }
            return template;
        }



    }
}
