using IntellaLend.MinIOWrapper;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanPDFCheck
{
    class Program
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

        static void Main(string[] args)
        {
            //Console.WriteLine("Enter Stacking Order PDF Path:");
            //string stackPDFPath = Console.ReadLine();

            Console.WriteLine("Processing...");
            //string outputLoanIDs = Console.ReadLine();
            List<Int64> _lsLoanIDs = new List<long>();
            using (var db = new DBConnect("T1"))
            {
                _lsLoanIDs = (from l in db.Loan.AsNoTracking()
                        join p in db.LoanPDF.AsNoTracking() on l.LoanID equals p.LoanID
                        select l.LoanID).ToList();
            }

            ImageMinIOWrapper _imgWrapper = new ImageMinIOWrapper("T1");
            List<string> sr = new List<string>();
            foreach (Int64 loanID in _lsLoanIDs)
            {
               bool result = _imgWrapper.CheckFileExists(loanID);

                if (!result)
                    sr.Add(loanID.ToString());
            }

            string sql = $"select * from [T1].LoanPDF where LoanID in ({string.Join(",", sr)})";
            string OutputFile = @"Output\FileNotExistsLoanIDs.txt";

            if (!Directory.Exists("Output"))
                Directory.CreateDirectory("Output");

            if (File.Exists(OutputFile))
                File.Delete(OutputFile);

            File.WriteAllText(OutputFile, sql);

            //ImageMinIOWrapper 

            //using (SqlConnection conn = new SqlConnection(_connectionString))
            //{
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("select LoanID, LoanPDFPath from [T1].LoanPDF Order by LoanID", conn);
            //    SqlDataReader _reader = cmd.ExecuteReader();
            //    DataTable _dt = new DataTable();
            //    _dt.Load(_reader);

            //    if (_dt.Rows.Count > 0) {
            //        //StringBuilder sr = new StringBuilder();
            //        List<string> sr = new List<string>();
            //        foreach (DataRow dr in _dt.Rows)
            //        {
            //            if (!File.Exists(dr["LoanPDFPath"].ToString()))
            //                sr.Add(dr["LoanID"].ToString());
            //        }
            //        string sql = $"select * from [T1].LoanPDF where LoanID in ({string.Join(",", sr)})";
            //        string OutputFile = @"Output\FileNotExistsLoanIDs.txt";

            //        if (!Directory.Exists("Output"))
            //            Directory.CreateDirectory("Output");

            //        if (File.Exists(OutputFile))
            //            File.Delete(OutputFile);

            //        File.WriteAllText(OutputFile, sql);
            //    }
            //}

            Console.WriteLine("Completed!!!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
