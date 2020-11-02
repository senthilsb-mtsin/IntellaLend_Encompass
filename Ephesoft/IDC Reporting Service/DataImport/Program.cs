using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DataImport
{
    class Program
    {
        public static REPORTDB ReportDb { get; set; }
        public static string REPORT_DATA_FOLDER;

        public static void Log(string message, EventLogEntryType type)
        {
            Console.WriteLine(message);
        }

        public static List<QueueItem> GetBatchesToProcess()
        {
            List<QueueItem> queueItemList = new List<QueueItem>();

            string[] Batches = File.ReadAllLines(@"batchlist.txt");

            foreach (var item in Batches)
            {
                queueItemList.Add(new QueueItem()
                {
                    InstanceId = item,
                    BatchStatus = "Finished",
                    LastModified = DateTime.Now
                });
            }

            return queueItemList;
        }

        static void Main(string[] args)
        {

            REPORT_DATA_FOLDER = ConfigurationManager.AppSettings["REPORT_DATA_FOLDER"].ToString();

            bool flag1 = true;

            List<QueueItem> batchesToProcess = GetBatchesToProcess();

            Log($"batch Count : {batchesToProcess.Count}", EventLogEntryType.Information);

            batchesToProcess.ForEach(x =>
            {
                using (Program.ReportDb = new REPORTDB())
                {
                    try
                    {
                        string path = Path.Combine(REPORT_DATA_FOLDER, x.InstanceId);
                        Log($"batch path : {path}", EventLogEntryType.Information);
                        if (!Directory.Exists(path))
                        {
                            //   Log();
                            Log(string.Format("Report Data Folder '{0}' does not exist forbatch instance '{1}'", path, x.InstanceId), EventLogEntryType.Error);
                        }
                        else
                        {
                            bool flag = true;
                            BATCH entity = Program.ReportDb.BATCHES.Where(y => y.BATCH_INSTANCEID.Equals(x.InstanceId)).FirstOrDefault();
                            if (entity == null)
                            {
                                flag = false;
                                entity = new BATCH();
                                entity.BATCH_INSTANCEID = x.InstanceId;
                                Program.ReportDb.BATCHES.Add(entity);
                                Program.ReportDb.Entry(entity).State = EntityState.Added;
                            }
                            if (flag)
                                Program.ReportDb.Entry(entity).State = EntityState.Modified;

                            Log($"Before SetEphesoftMetadata()", EventLogEntryType.Information);
                            entity.SetEphesoftMetadata();
                            Log($"Before ProcessPatterns()", EventLogEntryType.Information);
                            entity.ProcessPatterns();

                            Log($"After ProcessPatterns()", EventLogEntryType.Information);
                            Program.ReportDb.SaveChanges();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Log(ex.Message, EventLogEntryType.Error);
                    }
                }
            });


            Console.WriteLine("Completed");
            Console.ReadKey();
        }
    }

    internal class QueueItem
    {
        public long Pk { get; set; }

        public string InstanceId { get; set; }

        public string BatchStatus { get; set; }

        public DateTime LastModified { get; set; }

        public DateTime CreateDate { get; set; }

        public string Review_Operator { get; set; }

        public string Validation_Operator { get; set; }
    }
}
