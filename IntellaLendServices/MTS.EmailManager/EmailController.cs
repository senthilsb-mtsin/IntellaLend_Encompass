using IL.EmailManager.EmailDataAccess;
using IntellaLend.Constance;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;


namespace IL.EmailManager
{
    public class EmailController : IMTSServiceBase
    {

        public void OnStart(string Params)
        {

            LoadAlarmList();
            GetEmailSchedule();
            GetEmailTemplates();
        }
        public bool DoTask()
        {
            try
            {
                SendEmail();
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
        }

        #region "Declarations"
        DataSet dsSchedule, dsTimeScheduler, dsEmailMaster, dsEmailTemplates;
        EmailDAL dataaccess = new EmailDAL();
        Dictionary<Int64, TimeSpan> dictAlaram;
        #endregion

        #region "Constants"

        const string cImmediate = "1";
        const string cDaily = "2";
        const string cWeekly = "3";
        const string cMonthly = "4";
        const int cStatusSucess = 1;
        const int cStatusFailure = 2;

        #endregion

        #region "Public Functions"

        public void SendEmail()
        {
            List<long> lstRemove = new List<long>();
            try
            {
                string currenttime = DateTime.Now.ToString("HH:mm");
                if (currenttime == "00:00") // Load alarmlist everyday at 12'O clock midnight
                {
                    LoadAlarmList();
                    GetEmailSchedule();
                    GetEmailTemplates();
                }
                else
                {
                    foreach (var lsttime in dictAlaram)
                    {
                        if (DateTime.Now.TimeOfDay >= lsttime.Value)
                        {
                            SendScheduledEmail(lsttime.Key);
                            lstRemove.Add(lsttime.Key);
                        }
                    }

                    foreach (var remove in lstRemove)
                    {
                        dictAlaram.Remove(remove);
                    }
                }
                SendImmediateEmail();
            }
            finally
            {
                lstRemove = null;
            }
        }
        #endregion

        #region "Private Functions"


        private void SendImmediateEmail()
        {
            GetWaitingEmailTobeSent();
            DoSendMailImmediateProcess();
        }

        private void SendScheduledEmail(long ScheduleID)
        {

            GetWaitingEmailTobeSent();
            string TemplateId = GetTemplateIdFromSchedule(ScheduleID).ToString();
            DoSendMailScheduledProcess(TemplateId);
        }

        private void LoadAlarmList()
        {
            dictAlaram = null;
            dictAlaram = new Dictionary<Int64, TimeSpan>();
            GetEmailScheduleForTimeScheduler();
            CheckForDailySendBy();
            CheckForWeeklySendBy();
            CheckForMonthlySendBy();
        }

        private void GetEmailSchedule()
        {
            dsSchedule = dataaccess.GetEmailSchedule();
        }

        private void GetEmailScheduleForTimeScheduler()
        {
            dsTimeScheduler = dataaccess.GetEmailScheduleForTimeScheduler();
        }
        private void GetWaitingEmailTobeSent()
        {
            dsEmailMaster = dataaccess.GetWaitingEmailTobeSent();
        }

        private void GetEmailTemplates()
        {
            dsEmailTemplates = dataaccess.GetEmailTemplates();
        }

        private void DoSendMailImmediateProcess()
        {
            DataRow[] drImmediate = dsSchedule.Tables[0].Select("SendBy=" + cImmediate);
            foreach (var item in drImmediate)
            {
                DoEmailProcess(item["TemplateId"].ToString());
            }
        }

        private void DoSendMailScheduledProcess(string TemplateId)
        {
            DoEmailProcess(TemplateId);
        }

        private void DoEmailProcess(string TemplateId)
        {
            int Status = cStatusSucess;
            DataRow[] drEmailDataImmediate = dsEmailMaster.Tables[0].Select("TemplateId=" + TemplateId);
            foreach (var itemdata in drEmailDataImmediate)
            {
                Status = cStatusSucess;

                try
                {
                    EmailTemplate template = new EmailTemplate();

                    DataRow[] itemtemplate = dsEmailTemplates.Tables[0].Select("TemplateId=" + itemdata["TemplateId"].ToString());
                    string[] _emailSP = itemdata["EmailSP"].ToString().Split(',');
                    Template templateForSend = template.CreateEmailTemplateAndProcess(itemtemplate[0]["HTMLPAGE"].ToString(), itemdata["EmailSP"].ToString());
                    if(TemplateId != EmailTemplateConstants.MASJsonEmail.ToString())
                    {
                        templateForSend.To = _emailSP[1];
                    }
                    string smtpId = dsEmailTemplates.Tables[0].Select("TemplateId=" + itemdata["TemplateId"].ToString())[0]["SMTPID"].ToString();

                    if (!SendMail(templateForSend, smtpId))
                        Status = cStatusFailure;
                }
                catch (Exception ex)
                {
                    Status = cStatusFailure;
                    MTSExceptionHandler.HandleException(ref ex);
                }

                UpdateEmailStatus((Int64)itemdata["ID"], Status);
            }
        }

        private DataSet GetDataFromEMailSP(string SPandParameter)
        {
            string[] sparr = SPandParameter.Split('|');
            return dataaccess.GetEmailDataFromSP(sparr[0].Trim(), sparr[1].Split(','));
        }

        private void UpdateEmailStatus(Int64 Id, int Status)
        {
            dataaccess.UpdateEmailStatus(Id, Status);
        }

        private void CheckForDailySendBy()
        {
            DataRow[] drDaily = dsTimeScheduler.Tables[0].Select("sendby=" + cDaily);
            foreach (var dr in drDaily)
            {
                dictAlaram.Add((Int64)dr["ScheduleId"], (TimeSpan)dr["Time"]);
            }
        }

        private void CheckForWeeklySendBy()
        {
            int TodayDayOfWeek = (int)DateTime.Now.DayOfWeek;
            DataRow[] drDaily = dsTimeScheduler.Tables[0].Select("sendby=" + cWeekly);

            foreach (var dr in drDaily)
            {
                List<string> lst = dr["Day"].ToString().Split(',').ToList<string>();
                if (lst.Exists(x => x.Contains(TodayDayOfWeek.ToString())))
                {
                    dictAlaram.Add((Int64)dr["ScheduleId"], (TimeSpan)dr["Time"]);
                }
            }
        }

        private void CheckForMonthlySendBy()
        {
            bool IsEndOfMonth = false;
            bool CanAddToList = false; ;
            string DayOfMonth = DateTime.Now.Day.ToString();
            string EndDayofMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month).ToString();
            if (DayOfMonth == EndDayofMonth)
            {
                IsEndOfMonth = true;
            }
            DataRow[] drDaily = dsTimeScheduler.Tables[0].Select("sendby=" + cMonthly);
            foreach (var dr in drDaily)
            {
                List<string> lst = dr["Day"].ToString().Split(',').ToList<string>();
                if (lst.Exists(y => y.Contains(DayOfMonth)))
                    CanAddToList = true;
                if ((IsEndOfMonth) && (Convert.ToInt16(lst[0]) > DateTime.Now.Day))
                    CanAddToList = true;

                if (CanAddToList)
                    dictAlaram.Add((Int64)dr["ScheduleId"], (TimeSpan)dr["Time"]);
            }
        }

        private int GetTemplateIdFromSchedule(long ScheduleId)
        {
            return int.Parse(dataaccess.GetTemplateIDFromSchedule(ScheduleId).ToString());
        }



        private bool SendMail(Template email, string smtpId)
        {
            try
            {
                DataSet dsSTMPDetails = dataaccess.GetSTMPDetails();
                if ((dsSTMPDetails == null) || (dsSTMPDetails.Tables.Count == 0))
                    return false;
                MailMessage message = new System.Net.Mail.MailMessage();

                if (ConfigurationManager.AppSettings.AllKeys.Contains("Environment") && ConfigurationManager.AppSettings["Environment"].ToLower() == "test")
                {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains("TestingEmail") && ConfigurationManager.AppSettings["TestingEmail"] != string.Empty)
                    {
                        message.To.Add(ConfigurationManager.AppSettings["TestingEmail"]);
                    }
                    else
                    {
                        throw new Exception("Provide value for TestingEmail in config");
                    }
                }
                else
                {
                    message.To.Add(email.To);

                    if (!string.IsNullOrEmpty(email.Cc))
                        message.CC.Add(email.Cc);
                    if (!string.IsNullOrEmpty(email.BCc))
                        message.Bcc.Add(email.BCc);
                }

                message.Subject = email.Subject;

                message.Body = email.Body.Replace("\n", "");
                message.IsBodyHtml = true;

                if (email.Attachment != null)
                {
                    message.Attachments.Add(new Attachment(email.Attachment, email.AttachmnetName));
                }

                DataRow[] drsmtp = dsSTMPDetails.Tables[0].Select("SmtpId='" + smtpId + "'");

                SmtpClient smtp = new SmtpClient(drsmtp[0]["SMTPCLIENTHOST"].ToString(), Convert.ToInt32(drsmtp[0]["SMTPCLIENTPORT"]));
                smtp.EnableSsl = Convert.ToBoolean(drsmtp[0]["ENABLESSL"]);

                if (ConfigurationManager.AppSettings.AllKeys.Contains("LocalDomain") && ConfigurationManager.AppSettings["LocalDomain"].ToLower() == "true")
                {
                    message.From = new System.Net.Mail.MailAddress(drsmtp[0]["USERNAME"].ToString());
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                }
                else
                {
                    message.From = new System.Net.Mail.MailAddress(email.From);
                    smtp.Timeout = Convert.ToInt32(drsmtp[0]["TIMEOUT"]);
                    smtp.DeliveryMethod = (SmtpDeliveryMethod)(Convert.ToInt32((drsmtp[0]["SMTPDELIVERYMETHOD"])));
                    smtp.UseDefaultCredentials = Convert.ToBoolean(drsmtp[0]["USEDEFAULTCREDENTIALS"]);
                    smtp.Credentials = new NetworkCredential(drsmtp[0]["USERNAME"].ToString(), drsmtp[0]["PASSWORD"].ToString(), drsmtp[0]["DOMAIN"].ToString());
                }
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
            return true;
        }

        #endregion
    }
}
