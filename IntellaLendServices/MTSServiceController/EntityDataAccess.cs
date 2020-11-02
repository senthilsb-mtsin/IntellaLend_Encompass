using IntellaLend.Model;
using MTSEntityDataAccess;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;

namespace ServiceController
{
    public class ServiceDataAccess
    {
        private static string SystemSchema = "IL";
        private static string TenantSchema;

        public ServiceDataAccess()
        {
                        
        }

        public void SetMinioCredentials(string accessKey,string secretKey,string endPoint)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                var tenantList = GetTenantList();
                foreach (var t in tenantList)
                {

                    if (!string.IsNullOrEmpty(t.Configuration))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(t.Configuration);
                        foreach (XmlElement node in doc.DocumentElement)
                        {
                            if (node.Name == "MinioAccessKey")
                                node.InnerText = accessKey;

                            if (node.Name == "MinioSecretKey")
                                node.InnerText = secretKey;

                            if (node.Name == "MinioEndPoint")
                                node.InnerText = endPoint;
                        }

                        t.Configuration = GetXMLString(doc);                        
                        db.Entry(t).State = EntityState.Modified;
                        db.SaveChanges();
                    }                    
                }
            }
        }

        private List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenantMs = db.TenantMaster.ToList();
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }


        private string GetXMLString(XmlDocument xmlDoc)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
}
