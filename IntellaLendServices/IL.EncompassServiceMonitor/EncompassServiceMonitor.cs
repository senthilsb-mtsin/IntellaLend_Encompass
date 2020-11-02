using MTS.ServiceBase;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace IL.EncompassServiceMonitor
{
    public class EncompassServiceMonitor : IMTSServiceBase
    {
        int timeInterval;
        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            timeInterval = Convert.ToInt32(Params.Find(f => f.Key == "TimeInterval").Value);
        }
        public bool DoTask()
        {
            try
            {
                DataTable LastAccessDetails = GetLastAccessDetails();
                if (LastAccessDetails.Rows.Count > 0)
                {
                    TimeSpan varTime = (DateTime.Now - Convert.ToDateTime(LastAccessDetails.Rows[0]["ConfigValue"]));
                    double fractionalMinutes = varTime.TotalMinutes;
                    int minutes = (int)fractionalMinutes;
                    if (minutes > timeInterval)
                    {
                        string emailsp = ConfigurationManager.AppSettings["EmailTo"];
                        AddServiceControllerNotification(emailsp);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
        }

        public static DataTable GetLastAccessDetails()
        {
            return DataAccess.ExecuteDataTable("IL.GETSERVICECONTROLLERACCESSTIME");
        }

        public static void AddServiceControllerNotification(string emailsp)
        {
            DataAccess.ExecuteNonQuery("IL.Cus_Notification_ServiceController",emailsp);
        }
    }
}
