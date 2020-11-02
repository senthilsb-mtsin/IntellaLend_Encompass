using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveEphesoftOutputFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dt = DataAccess.ExecuteSQLDataTable("select IDCReviewDuration, EphesoftReviewerName, IDCValidationDuration, EphesoftValidatorName, LoanID from [t1].loans ");
            List<string> loanids = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                
                try
                {
                    Console.WriteLine($"LoanID : {dr["LoanID"].ToString()}");
                    string EphesoftReviewerName = dr["EphesoftReviewerName"].ToString();
                    string IDCReviewDuration = dr["IDCReviewDuration"].ToString();
                    string IDCValidationDuration = dr["IDCValidationDuration"].ToString();
                    string EphesoftValidatorName = dr["EphesoftValidatorName"].ToString();
                    string MaxIDCReviewDuration = string.Empty;
                    string MaxEphesoftReviewerName = string.Empty;
                    string MaxIDCValidationDuration = string.Empty;
                    string MaxEphesoftValidatorName = string.Empty;

                    if (!string.IsNullOrEmpty(EphesoftReviewerName) && !string.IsNullOrEmpty(IDCReviewDuration))
                    {
                        List<string> nameLs = EphesoftReviewerName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        List<DateTime> durationLs = IDCReviewDuration.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(a => DateTime.ParseExact(a.Trim(), "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();

                        //Console.WriteLine($"Count MisMatch : {durationLs.Count != nameLs.Count}");

                        if (durationLs.Count != nameLs.Count) {
                            loanids.Add(dr["LoanID"].ToString());
                        }
                        //if (durationLs.Count > 0)
                        //{
                        //    MaxIDCReviewDuration = durationLs.Max().ToString("HH:mm:ss");
                        //    int maxTimeIndex = durationLs.IndexOf(durationLs.Max());
                        //    MaxEphesoftReviewerName = nameLs[maxTimeIndex].Trim();
                        //}
                    }

                    if (!string.IsNullOrEmpty(EphesoftValidatorName) && !string.IsNullOrEmpty(IDCValidationDuration))
                    {
                        List<string> nameLs = EphesoftValidatorName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        List<DateTime> durationLs = IDCValidationDuration.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(a => DateTime.ParseExact(a.Trim(), "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();

                       // Console.WriteLine($"Count MisMatch : {durationLs.Count != nameLs.Count}");
                        if (durationLs.Count != nameLs.Count)
                        {
                            loanids.Add(dr["LoanID"].ToString());
                        }
                        //if (durationLs.Count > 0)
                        //{
                        //    MaxIDCValidationDuration = durationLs.Max().ToString("HH:mm:ss");
                        //    int maxTimeIndex = durationLs.IndexOf(durationLs.Max());
                        //    MaxEphesoftValidatorName = nameLs[maxTimeIndex].Trim();
                        //}
                    }

                    //Console.WriteLine($"Processing Batch : {dr["EphesoftBatchInstanceID"].ToString()}");
                    ////string directoryPath = Path.Combine(@"E:\EphesoftInputOutput\UHS_Release_0308\Output", dr["EphesoftBatchInstanceID"].ToString(), SearchOption.AllDirectories);
                    //string[] directoryPath = Directory.GetDirectories(@"E:\EphesoftInputOutput\UHS_Release_0308\Output", dr["EphesoftBatchInstanceID"].ToString(), SearchOption.AllDirectories);
                    //Console.WriteLine($"{dr["EphesoftBatchInstanceID"].ToString()} found Count : {directoryPath.Length.ToString()}");
                    //foreach (string folder in directoryPath)
                    //{
                    //    Console.WriteLine($"Current Path : {folder}");
                    //    try
                    //    {
                    //        if (Directory.Exists(folder))
                    //        {
                    //            Directory.Delete(folder, true);
                    //            Console.WriteLine($"Deleted : {folder}");
                    //        }
                    //        else
                    //        {
                    //            Console.WriteLine($"Not Found : {folder}");
                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //        Console.WriteLine(ex.StackTrace);
                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

            }

            Console.WriteLine(string.Join(",", loanids));
            Console.WriteLine("Done!!!");

        }
    }
}
