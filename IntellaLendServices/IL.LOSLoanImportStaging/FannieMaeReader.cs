using IntellaLend.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IL.LOSLoanImport
{
    public class FannieMaeReader
    {
        private string[] fileDatas;
        private List<LOSDocumentFields> tempData = new List<LOSDocumentFields>();
        private string mutliOccuranceSchema = "M";
        private string singleOccuranceSchema = "S";

        public FannieMaeReader(string FannieMaeFilePath, List<LOSDocumentFields> TemplateFields)
        {
            fileDatas = File.ReadAllLines(FannieMaeFilePath);
            tempData = TemplateFields;
        }

        public string Read()
        {
            try
            {
                Dictionary<string, object> FannieMaeFieldDic = new Dictionary<string, object>();
                FannieMaeFieldDic.Append(ReadSingleOccurrenceFields());
                FannieMaeFieldDic.Append(ReadMultiOccurrenceFields());
                return JsonConvert.SerializeObject(FannieMaeFieldDic);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, object> ReadSingleOccurrenceFields()
        {
            List<LOSDocumentFields> singleOccurance = tempData.Where(x => x.FieldOccurrences.ToString() == singleOccuranceSchema).ToList();
            Dictionary<string, object> singleOccuranceDic = new Dictionary<string, object>();
            try
            {
                foreach (LOSDocumentFields dr in singleOccurance)
                {
                    string fieldIDStart = dr.FieldName.ToString();
                    string dataStream = fileDatas.Where(x => x.StartsWith(fieldIDStart)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(dataStream))
                    {
                        List<LOSDocumentFields> queryLine = tempData.Where(x => x.FieldID.ToString().StartsWith(fieldIDStart)).ToList();
                        Dictionary<string, object> lineData = GetFieldValue(queryLine, dataStream);
                        foreach (var fieldID in lineData.Keys)
                        {
                            singleOccuranceDic[fieldID] = lineData[fieldID];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return singleOccuranceDic;
        }

        private Dictionary<string, object> ReadMultiOccurrenceFields()
        {
            List<LOSDocumentFields> multiOccurance = tempData.Where(x => x.FieldOccurrences.ToString() == mutliOccuranceSchema).ToList();
            Dictionary<string, object> multiOccuranceDic = new Dictionary<string, object>();
            try
            {
                foreach (LOSDocumentFields dr in multiOccurance)
                {
                    string fieldIDStart = dr.FieldName.ToString();
                    string[] dataStreams = fileDatas.Where(x => x.StartsWith(fieldIDStart)).ToArray();
                    if (dataStreams.Length > 0)
                    {
                        List<LOSDocumentFields> queryLine = tempData.Where(x => x.FieldID.ToString().StartsWith(fieldIDStart)).ToList();
                        foreach (var lineQueryDR in queryLine)
                        {
                            int count = 1;
                            int startIndex = lineQueryDR.FieldPosition.ToInt32() - 1;
                            int fieldLength = lineQueryDR.FieldLength.ToInt32();
                            List<string> fieldValues = new List<string>();
                            foreach (string line in dataStreams)
                            {
                                try
                                {
                                    string fieldValue = string.Empty;
                                    if (line.Length >= (startIndex + fieldLength))
                                    {
                                        fieldValue = line.Substring(startIndex, fieldLength).Trim();
                                        string tempfieldValue = fieldValue;
                                        if (!string.IsNullOrEmpty(lineQueryDR.FieldValueCode.ToString()))
                                        {
                                            dynamic fieldValueCode = JsonConvert.DeserializeObject(lineQueryDR.FieldValueCode.ToString());
                                            fieldValue = fieldValueCode[fieldValue];
                                            if (fieldValue == null) fieldValue = string.Empty;
                                        }
                                    }
                                    fieldValues.Add(fieldValue);
                                    count++;
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }

                            }
                            multiOccuranceDic[lineQueryDR.FieldID.ToString()] = new { Type = "M", Value = fieldValues };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return multiOccuranceDic;
        }
        private Dictionary<string, object> GetFieldValue(List<LOSDocumentFields> queryLine, string line)
        {
            Dictionary<string, object> lineData = new Dictionary<string, object>();
            foreach (LOSDocumentFields lineQueryDR in queryLine)
            {
                int startIndex = lineQueryDR.FieldPosition.ToInt32() - 1;
                int fieldLength = lineQueryDR.FieldLength.ToInt32();

                string fieldValue = line.Substring(startIndex, fieldLength).Trim();
                string tempfieldValue = fieldValue;
                if (!string.IsNullOrEmpty(lineQueryDR.FieldValueCode.ToString()))
                {
                    dynamic fieldValueCode = JsonConvert.DeserializeObject(lineQueryDR.FieldValueCode.ToString());
                    fieldValue = fieldValueCode[fieldValue];
                    if (fieldValue == null) fieldValue = string.Empty;
                }
                lineData[lineQueryDR.FieldID.ToString()] = new { Type = "S", Value = fieldValue };
            }
            return lineData;
        }
    }
    public static class MTSExtentions
    {
        public static Int32 ToInt32(this object str)
        {
            return Convert.ToInt32(str);
        }

        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            List<KeyValuePair<K, V>> pairs = second.ToList();
            pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
        }
    }
}
