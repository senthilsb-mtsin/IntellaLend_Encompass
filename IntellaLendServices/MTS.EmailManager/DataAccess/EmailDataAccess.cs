using MTSEntBlocks.DataBlock;
using System;
using System.Data;
namespace IL.EmailManager.EmailDataAccess
{
    public class EmailDAL
    {
        public DataSet GetEmailSchedule()
        {
            return DataAccess.ExecuteDataset("[IL].GetEmailSchedule", null);
        }

        public DataSet GetEmailScheduleForTimeScheduler()
        {
            return DataAccess.ExecuteDataset("[IL].GetEmailScheduleForTimeScheduler", null);
        }
        public DataSet GetWaitingEmailTobeSent()
        {
            return DataAccess.ExecuteDataset("[IL].GetEmailsWaitingToBeSend", null);
        }

        public DataSet GetEmailTemplates()
        {
            return DataAccess.ExecuteDataset("[IL].GetEmailTemplate", null);
        }

        public void UpdateEmailStatus(Int64 Id, int Status)
        {
            DataAccess.ExecuteNonQuery("[IL].UpdateEmailStatus", new object[] { Id, Status });
        }
        public DataSet GetSTMPDetails()
        {
            return DataAccess.ExecuteDataset("[IL].GetSTMPDetails", null);
        }
        public object  GetTemplateIDFromSchedule(long ScheduleID)
        {
            return DataAccess.ExecuteScalar("[IL].GETTEMPLATEIDFROMSCHEDULEID", ScheduleID);
        }
        public DataSet GetEmailDataFromSP(string SPName, object[] Parameters)
        {
            return DataAccess.ExecuteDataset(SPName, Parameters);
        }
    }
}
