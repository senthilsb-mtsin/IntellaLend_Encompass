using IL.EmailManager;
using IL.EmailTrackerTemplate;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace IL.EmailTrackers
{
    public class EmailTrackerService : IMTSServiceBase
    {
        public void OnStart(string Params)
        {
            GetEmailTemplates();
        }
        public bool DoTask()
        {
            try
            {
                DoEmailProcess();
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
        }
        #region "Declarations"

        DataSet dsEmailTemplates;
        DataSet _customEmailTemplates;

        #endregion
        #region "Constants"

        const string cImmediate = "1";
        const string cDaily = "2";
        const string cWeekly = "3";
        const string cMonthly = "4";
        const int cStatusSucess = 1;
        const int cStatusFailure = 2;

        #endregion
        private void GetEmailTemplates()
        {
            dsEmailTemplates = EmailTrackerDataAccess.GetEmailTemplates();
            //_customEmailTemplates = EmailTrackerDataAccess.GetCustomEmailTemplates();
        }
        private void DoEmailProcess()
        {
            var TenantList = EmailTrackerDataAccess.GetTenantList();
            //int Status = cStatusSucess;
            foreach (var tenant in TenantList)
            {
                //Status = cStatusSucess;
                
                try
                {
                    EmailTrackerDataAccess _dataAccess = new EmailTrackerDataAccess(tenant.TenantSchema);
                    EmailTemplate _template = new EmailTemplate();
                    List<EmailTracker> _emailtracker = _dataAccess.GetEmailTrackerPendingData();
                    foreach (EmailTracker _Edata in _emailtracker)
                    {
                        int Status = 0;
                        string Message = string.Empty;
                        string userEmail = _dataAccess.GetUserEmail(_Edata.UserID);
                        try
                        {
                            if (_Edata.TemplateID == 0)
                            {
                                _template.To = string.IsNullOrEmpty(userEmail) ? _Edata.To : userEmail; //_Edata.To;
                                _template.Body = _Edata.Body;
                                _template.Subject = _Edata.Subject;
                                Dictionary<string, byte[]> _attachments = _dataAccess.GetEmailAttachment(_Edata.LoanID ,_Edata.Attachments, _Edata.AttachmentsName);
                                // string smtpId = dsEmailTemplates.Tables[0].Select("TemplateId=" + _Edata.TemplateID)[0]["SMTPID"].ToString();
                              
                               
                                 SendMail(_template, "1", _attachments, ref Message, ref Status);
                                _Edata.Delivered = Status;
                                _Edata.ErrorMessage = Message;
                                _dataAccess.UpdateEmailStatus(_Edata);
                            }
                            else
                            {
                                //DataRow[] itemtemplate = dsEmailTemplates.Tables[0].Select("TemplateId=" + itemdata["TemplateId"].ToString());
                                //string smtpId = dsEmailTemplates.Tables[0].Select("TemplateId=" + itemdata["TemplateId"].ToString())[0]["SMTPID"].ToString();
                                EmailXMLTemplate template = new EmailXMLTemplate();

                                DataRow[] _customitemtemplate = dsEmailTemplates.Tables[0].Select("TemplateId=" + _Edata.TemplateID.ToString());

                                string _Content = tenant.TenantSchema +"|" + _Edata.To + "|" + _Edata.Subject + "|" + _Edata.LoanID;
                                Template templateForSend = template.CreateEmailTemplateAndProcess(_customitemtemplate[0]["HTMLPAGE"].ToString(), _Content);
                                templateForSend.To = string.IsNullOrEmpty(userEmail) ? _Edata.To : userEmail;
                                string smtpId = dsEmailTemplates.Tables[0].Select("TemplateId=" + _Edata.TemplateID.ToString())[0]["SMTPID"].ToString();

                                     SendCustomMail(templateForSend, smtpId, ref Message, ref Status);
                                    _Edata.Delivered = Status;
                                    _Edata.ErrorMessage = Message;
                                    _dataAccess.UpdateEmailStatus(_Edata);
                                
                             
                                //Status = cStatusFailure;
                            }
                        }
                        catch (Exception ex)
                        {
                            MTSExceptionHandler.HandleException(ref ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                }
            }

        }

        private bool SendMail(EmailTemplate email, string smtpId, Dictionary<string, byte[]> _attachments, ref string Message, ref int Status)
        {
            try
            {

                DataSet dsSTMPDetails = EmailTrackerDataAccess.GetSTMPDetails();
                if ((dsSTMPDetails == null) || (dsSTMPDetails.Tables.Count == 0))
                {
                    Message = "SMTP Details Not Found";
                    return false;
                }
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
                    string[] ToAddress = email.To.Split(',');
                    foreach (var item in ToAddress)
                    {
                        message.To.Add(item.Trim());
                    }

                }

                message.Subject = email.Subject;

                message.Body = email.Body;//.Replace("\n", "");
                message.IsBodyHtml = true;

                if (_attachments.Count > 0)
                {
                    Int64 fileSize = 0;
                    foreach (var item in _attachments.Keys)
                    {
                        fileSize = fileSize +_attachments[item].Length;
                    }
                    fileSize = fileSize / 1048576;
                    if (fileSize <= 10)
                    {
                        foreach (var item in _attachments.Keys)
                        {
                            message.Attachments.Add(new Attachment(new MemoryStream(_attachments[item]), item + ".pdf", MediaTypeNames.Application.Pdf));
                        }
                    }
                    else
                    {
                        throw new Exception("Document Size Greater than 10 MB");

                    }
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
                    message.From = new System.Net.Mail.MailAddress(drsmtp[0]["USERNAME"].ToString());
                    smtp.Timeout = Convert.ToInt32(drsmtp[0]["TIMEOUT"]);
                    smtp.DeliveryMethod = (SmtpDeliveryMethod)(Convert.ToInt32((drsmtp[0]["SMTPDELIVERYMETHOD"])));
                    smtp.UseDefaultCredentials = Convert.ToBoolean(drsmtp[0]["USEDEFAULTCREDENTIALS"]);
                    smtp.Credentials = new NetworkCredential(drsmtp[0]["USERNAME"].ToString(), drsmtp[0]["PASSWORD"].ToString(), drsmtp[0]["DOMAIN"].ToString());
                }
                smtp.Send(message);
                Status = 1;
            }
            catch (Exception ex)
            {
                Status = -1;
                Message = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
            return true;
        }
        private bool SendCustomMail(Template email, string smtpId,ref string Message,ref int Status)
        {
            try
            {
                DataSet dsSTMPDetails = EmailTrackerDataAccess.GetSTMPDetails();
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
                   string [] mailIds = email.To.Split(',');

                    foreach (var item in mailIds)
                    {
                        message.To.Add(item);
                    }
                   // message.To.Add(email.To);

                    if (!string.IsNullOrEmpty(email.Cc))
                        message.To.Add(email.Cc);
                    if (!string.IsNullOrEmpty(email.BCc))
                        message.To.Add(email.BCc);
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
                Status = 1;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Status = -1;
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
            return true;
        }
    }

  
}

