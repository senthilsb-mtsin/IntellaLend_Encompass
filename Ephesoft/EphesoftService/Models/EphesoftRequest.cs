using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EphesoftService.Models
{
    /// <summary>
    /// This class represents the request from the client. It contains the Ephesoft XML file, the Ephesoft 
    /// module which could be REVIEW, VALIDATION and EXPORT. The three flags indicate if the append, 
    /// concatenate and convert operations should be performed for the input XML.
    /// </summary>
    public class EphesoftRequest
    {
        [JsonProperty(Required = Required.Always)]
        public string inputXML { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean appendFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean concatenateFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean convertFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean pageSequenceFlag { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ephesoftModule { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string orderOfExecution { get; set; }

        //[JsonProperty(Required = Required.AllowNull)]
        //public string ephesoftModule { get; set; }
    }

    public class MASEphesoftRequest
    {
        [JsonProperty(Required = Required.Always)]
        public string inputXML { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean appendFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean concatenateFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean convertFlag { get; set; }

        //[JsonProperty(Required = Required.Always)]
        //public Boolean pageSequenceFlag { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int ephesoftModule { get; set; }

        //[JsonProperty(Required = Required.AllowNull)]
        //public string ephesoftModule { get; set; }
    }

    public class IntellaLnedWrapper
    {
        [JsonProperty(Required = Required.Always)]
        public string inputXML { get; set; }

    }

    public class EphesoftLoanDetailsResponse : IntellaLendResponse
    {
        public string LoanDetailsJson { get; set; }
    }

    public class StackingOrderDocumentsResponse : IntellaLendResponse
    {
        public string stackingOrderDocuments { get; set; }
    }

    public class EphesoftLoanPageCountRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 PageCount { get; set; }
    }


    public class EnDocumentType
    {
        public string DocumentTypeName { get; set; }
        public List<Int32> Pages { get; set; }
    }

    public class EncompassDocPagesResponse : IntellaLendResponse
    {
        public bool isEncompassLoan { get; set; }
        public string EncompassDocPages { get; set; }
    }

    public class EphesoftLookupRequest
    {
        [JsonProperty(Required = Required.Always)]
        public string inputXML { get; set; }

        [JsonProperty(Required = Required.Default)]
        public bool isManual { get; set; }

    }


    public class IntellaLendResponse
    {
        [JsonProperty(PropertyName = "response-message")]
        public ResponseMessage ResponseMessage { get; set; }
    }

    public class ResponseMessage
    {

        [JsonProperty(PropertyName = "message-id")]
        public long MessageID { get; set; }

        [JsonProperty(PropertyName = "message-desc")]
        public string MessageDesc { get; set; }
    }


    public class QCIQUpdateEphesoftBatchIDRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string BatchID { get; set; }
    }

    public class QCIQDBDetailsResponse : IntellaLendResponse
    {
        public string data { get; set; }
    }

    public class QCIQDBDetailsRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }

    public class EphesoftLoanDetailsRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public string BatchID { get; set; }
        public string BatchClassID { get; set; }
        public string BatchClassName { get; set; }
    }

    public class EphsoftLOSExportFileStagingRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public int FileType { get; set; }
        public string BatchID { get; set; }
        public string BatchName { get; set; }
        public string BatchClassID { get; set; }
        public string BCName { get; set; }
        public List<MASDocument> MASDocumentList { get; set; }
    }

    public class StackOrderDocumentsRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 ConfigID { get; set; }
    }

    public class MASDocument
    {
        public string DocumentID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentDesc { get; set; }
    }

    public class EncompassDocPagesRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
    }


    public class EncompassUpdateLoanTypeRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanNumber { get; set; }
        public string BorrowerName { get; set; }
    }


    public class QCIQUpdateLoanTypeRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanType { get; set; }
        public DateTime? QCIQStartDate { get; set; }
    }
}