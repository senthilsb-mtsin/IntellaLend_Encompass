using EphesoftService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService
{
    public class FieldValidation
    {
        public void RemoveQuestionMarkFieldOption(XMLBatch batch)
        {
            foreach (var doc in batch.Documents)
            {
              foreach(var field in doc.DocumentLevelFields)
                {
                    if(!string.IsNullOrEmpty(field.FieldValueOptionList) && field.Value.Contains("?"))
                    {
                        var optionList = field.FieldValueOptionList.Split(';').ToList();
                        optionList.Remove(field.Value);
                        field.FieldValueOptionList = string.Join(";", optionList.ToArray());
                        field.Value = string.Empty;
                    }

                }
            }

        }
    }
}