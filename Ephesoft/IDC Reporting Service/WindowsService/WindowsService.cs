using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace MTSIDCReportService
{
    class Service : ServiceBase
    {
        private static string KEY_LAST_MODIFIED = "LAST_MODIFIED";
        private static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private Timer timer;
        public string DbType;
        public string EphesoftConnection;
        public string IntellaConnection;
        //public static int INTERVAL = 1;
        public string REPORT_DATA_FOLDER;
        public bool SyncEphesoft = false;
        public bool DebugLog = false;
        public static REPORTDB ReportDb { get; set; }

        public Service()
        {
            this.Log("Service() Construtor", EventLogEntryType.Information);
            this.ServiceName = "MTS IDC Reporting Service";
            this.EventLog.Source = "MTS IDC Reporting Service";
            this.EventLog.Log = "Application";

            //INTERVAL = Convert.ToInt32(ConfigurationManager.AppSettings["INTERVAL"].ToString());
            //REPORT_DATA_FOLDER = ConfigurationManager.AppSettings["REPORT_DATA_FOLDER"].ToString();
            //bool.TryParse(ConfigurationManager.AppSettings["SYNC_EPHESOFT"].ToString(), out SyncEphesoft);
            //DbType = ConfigurationManager.AppSettings["DB_Type"].ToString();
            //EphesoftConnection = ConfigurationManager.ConnectionStrings["EPHESOFTDB"].ConnectionString;
            //IntellaConnection = ConfigurationManager.ConnectionStrings["INTELLADB"].ConnectionString;

            if (!EventLog.SourceExists("MTS IDC Reporting Service"))
                EventLog.CreateEventSource("MTS IDC Reporting Service", "Application");


            this.Log("Service() Construtor End", EventLogEntryType.Information);
        }

        /// <summary>
        /// The Main Thread: This is where your Service is Run.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase.Run(new Service());
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        /// <summary>
        /// Dispose of objects that need it here.
        /// </summary>
        /// <param name="disposing">Whether or not disposing is going on.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// OnStart: Put startup code here
        ///  - Start threads, get inital data, etc.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            this.Log("OnStart()", EventLogEntryType.Information);
            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(this.DoTask);
            this.timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["INTERVAL"].ToString()) * 1000 * 60;

            bool.TryParse(ConfigurationManager.AppSettings["DEBUGLOG"].ToString(), out DebugLog);
            REPORT_DATA_FOLDER = ConfigurationManager.AppSettings["REPORT_DATA_FOLDER"].ToString();
            bool.TryParse(ConfigurationManager.AppSettings["SYNC_EPHESOFT"].ToString(), out SyncEphesoft);
            DbType = ConfigurationManager.AppSettings["DB_Type"].ToString();
            EphesoftConnection = ConfigurationManager.ConnectionStrings["EPHESOFTDB"].ConnectionString;
            IntellaConnection = ConfigurationManager.ConnectionStrings["INTELLADB"].ConnectionString;
            this.timer.Start();
            this.Log("OnStart() End", EventLogEntryType.Information);
            //base.OnStart(args);
        }

        private void DoTask(object source, ElapsedEventArgs e)
        {
            try
            {
                this.Log($"Service DoTask", EventLogEntryType.Information);
                this.timer.Enabled = false;
                this.InsertReportData();
            }
            finally
            {
                this.timer.Enabled = true;
            }

        }

        public void InsertReportData()
        {
            //   if (Service.InProgress)
            //      return;
            // Service.InProgress = true;
            this.Log($"Service InsertReportData()", EventLogEntryType.Information);
            try
            {
                using (Service.ReportDb = new REPORTDB())
                {
                    bool flag1 = true;
                    CONFIGURATION entity1 = Service.ReportDb.CONFIGURATIONS.Where(x => x.KEY.Equals(Service.KEY_LAST_MODIFIED)).FirstOrDefault();
                    if (entity1 == null)
                    {
                        flag1 = false;
                        entity1 = new CONFIGURATION();
                        entity1.KEY = Service.KEY_LAST_MODIFIED;
                        entity1.VALUE = new DateTime(2014, 1, 1).ToString(Service.DATETIME_FORMAT);
                        Service.ReportDb.CONFIGURATIONS.Add(entity1);
                        Service.ReportDb.Entry(entity1).State = EntityState.Added;
                    }
                    if (flag1)
                        Service.ReportDb.Entry(entity1).State = EntityState.Modified;

                    this.Log($"Last Modified Date to Query : {entity1.VALUE}", EventLogEntryType.Information);

                    List<QueueItem> batchesToProcess = this.GetBatchesToProcess(entity1.VALUE);

                    this.Log($"batch Count : {batchesToProcess.Count}", EventLogEntryType.Information);

                    entity1.VALUE = DateTime.Now.ToString(Service.DATETIME_FORMAT);
                    batchesToProcess.ForEach(x =>
                    {
                        try
                        {
                            string path = Path.Combine(REPORT_DATA_FOLDER, x.InstanceId);
                            this.Log($"batch path : {path}", EventLogEntryType.Information);
                            if (!Directory.Exists(path))
                            {
                                //   this.Log();
                                this.Log(string.Format("Report Data Folder '{0}' does not exist forbatch instance '{1}'", path, x.InstanceId), EventLogEntryType.Error);
                            }
                            else
                            {
                                bool flag = true;
                                BATCH entity = Service.ReportDb.BATCHES.Where(y => y.BATCH_INSTANCEID.Equals(x.InstanceId)).FirstOrDefault();
                                if (entity == null)
                                {
                                    flag = false;
                                    entity = new BATCH();
                                    entity.BATCH_INSTANCEID = x.InstanceId;
                                    Service.ReportDb.BATCHES.Add(entity);
                                    Service.ReportDb.Entry(entity).State = EntityState.Added;
                                }
                                if (flag)
                                    Service.ReportDb.Entry(entity).State = EntityState.Modified;

                                this.Log($"Before SetEphesoftMetadata()", EventLogEntryType.Information);
                                entity.SetEphesoftMetadata();
                                this.Log($"Before ProcessPatterns()", EventLogEntryType.Information);
                                entity.ProcessPatterns();

                                this.Log($"After ProcessPatterns()", EventLogEntryType.Information);
                            }
                        }
                        catch (SqlException ex)
                        {
                            this.Log(ex.Message, EventLogEntryType.Error);
                        }
                    });
                    Service.ReportDb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string message = "";
                this.GetInnerException(ex, ref message);
                //  BaseExceptionHandler.HandleException(ref ex);
                this.Log("Error occured \n" + ex.Message + "\nInnerException:\n" + message + "\n" + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private void Log(string message, EventLogEntryType type)
        {
            if (DebugLog)
                Logger.WriteTraceLog(message);
            //this.EventLog.WriteEntry(message, type);

            if (type == EventLogEntryType.Error && !DebugLog)
                Logger.WriteErrorLog(message);
        }

        //private void Log(string message)
        //{
        //    Exception ex = new Exception(message);
        //    // BaseExceptionHandler.HandleException(ref ex);
        //}

        private void GetInnerException(Exception ex, ref string message)
        {
            if (ex.InnerException != null)
                this.GetInnerException(ex.InnerException, ref message);
            else
                message += string.Format("\n\n\tInnerException: {0}, \n\tStackTrace: {1}", ex.Message, ex.StackTrace);
        }

        private List<QueueItem> GetBatchesToProcess(string LastModified)
        {
            List<QueueItem> queueItemList = new List<QueueItem>();
            if (SyncEphesoft)
            {
                string str = !DbType.Equals("SQL") ? string.Format("select bi.identifier batchInstanceId, bi.batch_status, bi.last_modified,bi.creation_date, bi.review_operator_user_name, bi.validation_operator_user_name from batch_instance bi where bi.last_modified >'" + LastModified + "'") : string.Format("select bi.identifier batchInstanceId, bi.batch_status, bi.last_modified,bi.creation_date, bi.review_operator_user_name, bi.validation_operator_user_name from batch_instance bi with(nolock) where bi.last_modified >'" + LastModified + "'");
                using (SqlConnection connection = new SqlConnection(EphesoftConnection))
                {
                    connection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                    {
                        sqlCommand.CommandText = str;
                        using (IDataReader dataReader = sqlCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                                queueItemList.Add(new QueueItem()
                                {
                                    InstanceId = dataReader["batchInstanceId"].ToString(),
                                    BatchStatus = dataReader["batch_status"].ToString(),
                                    LastModified = Convert.ToDateTime(dataReader["last_modified"])
                                });
                        }
                    }
                }
            }
            return queueItemList;
        }


        /// <summary>
        /// OnStop: Put your stop code here
        /// - Stop threads, set final data, etc.
        /// </summary>
        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer.Dispose();
            if (Service.ReportDb == null)
                return;
            Service.ReportDb.Dispose();

            //base.OnStop();
        }

        /// <summary>
        /// OnPause: Put your pause code here
        /// - Pause working threads, etc.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// OnContinue: Put your continue code here
        /// - Un-pause working threads, etc.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// OnShutdown(): Called when the System is shutting down
        /// - Put code here when you need special handling
        ///   of code that deals with a system shutdown, such
        ///   as saving special data before shutdown.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        /// <summary>
        /// OnCustomCommand(): If you need to send a command to your
        ///   service without the need for Remoting or Sockets, use
        ///   this method to do custom methods.
        /// </summary>
        /// <param name="command">Arbitrary Integer between 128 & 256</param>
        protected override void OnCustomCommand(int command)
        {
            //  A custom command can be sent to a service by using this method:
            //#  int command = 128; //Some Arbitrary number between 128 & 256
            //#  ServiceController sc = new ServiceController("NameOfService");
            //#  sc.ExecuteCommand(command);

            base.OnCustomCommand(command);
        }

        /// <summary>
        /// OnPowerEvent(): Useful for detecting power status changes,
        ///   such as going into Suspend mode or Low Battery for laptops.
        /// </summary>
        /// <param name="powerStatus">The Power Broadcase Status (BatteryLow, Suspend, etc.)</param>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// OnSessionChange(): To handle a change event from a Terminal Server session.
        ///   Useful if you need to determine when a user logs in remotely or logs off,
        ///   or when someone logs into the console.
        /// </summary>
        /// <param name="changeDescription"></param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
    }
}
