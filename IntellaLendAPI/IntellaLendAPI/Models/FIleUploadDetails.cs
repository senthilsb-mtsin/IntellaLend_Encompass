using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntellaLendAPI.Models
{
    public class FIleUploadDetails : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string FileName { get; set; }
    }
}