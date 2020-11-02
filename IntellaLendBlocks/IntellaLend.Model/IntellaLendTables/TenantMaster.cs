using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace IntellaLend.Model
{
    public class TenantMaster
    {
        [Key]
        public Int64 ID { get; set; }
        public string TenantName { get; set; }
        public string TenantSchema { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Configuration { get; set; }

        [NotMapped]
        public Dictionary<string, string> TenantConfig
        {
            get
            {
                Dictionary<string, string> config = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(Configuration))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Configuration);
                    foreach (XmlElement node in doc.DocumentElement)
                    {
                        config.Add(node.Name, node.InnerText);
                    }
                }
                return config;
            }
        }
    }
}
