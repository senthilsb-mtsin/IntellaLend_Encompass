using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTSEntityDataAccess;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;

namespace UpdateLoanDuration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<LoanDurationObject> _loanObjects = LoanDurationDataAccess.GetLoanDurationObjects();

                QCIQConnectionString _masterConnection = LoanDurationDataAccess.GetQCIQConnectionString(0);

                foreach (LoanDurationObject item in _loanObjects)
                {
                    try
                    {
                        string _sql = _masterConnection.SQLScript.Replace("<<CUSTOMER_NAME>>", item.CustomerName.Trim());

                        DataSet _masterDS = DynamicDataAccess.ExecuteSQLDataSet(_masterConnection.ConnectIonString, _sql);

                        QCIQConnectionString _reviewTypeConnection = LoanDurationDataAccess.GetQCIQConnectionString(item.ReviewTypeID);

                        System.Data.DataTable _dTable = GetTableFromSet(_masterDS, "CUSTOMER");

                        string _customerID = _dTable.Rows[0]["customerid"].ToString();

                        _sql = _reviewTypeConnection.SQLScript.Replace("<<CUSTOMER_ID>>", _customerID).Replace("<<LOAN_NUMBER>>", item.LoanNumber.Trim());

                        DataSet _qciqDS = DynamicDataAccess.ExecuteSQLDataSet(_reviewTypeConnection.ConnectIonString, _sql);

                        _dTable = GetTableFromSet(_qciqDS, "ASSIGNMENTHISTORYDATA");

                        DateTime _startDateTime = Convert.ToDateTime(_dTable.Rows[0]["WhenAssigned"]);

                        DateTime _endDateTime = LoanDurationDataAccess.GetLoanCompletionTime(item.LoanID);

                        TimeSpan time = TimeSpan.FromMilliseconds(_endDateTime.Subtract(_startDateTime).TotalMilliseconds);
                        string _duration = $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";


                        richTextBox1.Text += $"UPDATE [T1].LOANS SET LoanDuration = '{_duration}', QCIQStartDate = '{_startDateTime}'  WHERE LoanID = {item.LoanID.ToString()}" + Environment.NewLine;

                    }
                    catch (Exception ex)
                    {
                        Exception exc = new Exception($"Loan ID : {item.LoanID.ToString()}", ex);
                        MTSExceptionHandler.HandleException(ref exc);
                    }
                }


                //string existingPDF = @"D:\New folder\T1_1673.pdf";
                //string newPDF = @"D:\New folder\T1_2.pdf";
                //int lastPageNo = CommonUtils.GetPDFPageCount(existingPDF);
                //List<Int32> pageNoList = new List<int>();
                //for (int i = 1; i <= lastPageNo; i++)
                //    pageNoList.Add(i);

                //byte[] newBytes = CommonUtils.ReOrderPDFPages(File.ReadAllBytes(existingPDF), newPDF, pageNoList.ToArray());
                //File.WriteAllBytes(newPDF, newBytes);
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

        }

        private System.Data.DataTable GetTableFromSet(DataSet QCIQData, string TableName)
        {

            foreach (System.Data.DataTable dt in QCIQData.Tables)
            {
                if (dt.Rows.Count > 0 &&
                    Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToUpper().Trim() == TableName)
                    return dt;
            }

            return null;
        }
    }

    public class LoanDurationObject
    {
        public string CustomerName { get; set; }
        public string LoanNumber { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanID { get; set; }
    }

    public static class LoanDurationDataAccess
    {
        private static string TenantSchema = "T1";

        public static List<LoanDurationObject> GetLoanDurationObjects()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return (from l in db.Loan.AsNoTracking()
                        join c in db.CustomerMaster.AsNoTracking() on l.CustomerID equals c.CustomerID
                        where l.Status == 1 && (l.LoanDuration == null || l.LoanDuration == "")
                        select new LoanDurationObject
                        {
                            CustomerName = c.CustomerName,
                            LoanNumber = l.LoanNumber,
                            ReviewTypeID = l.ReviewTypeID,
                            LoanID = l.LoanID
                        }).OrderBy(l => l.LoanID).ToList();
            }
        }

        public static DateTime GetLoanCompletionTime(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.AuditLoan.AsNoTracking().Where(l => l.LoanID == LoanID && l.Status == 1).OrderBy(a => a.AuditDateTime).FirstOrDefault().AuditDateTime;
            }
        }

        public static QCIQConnectionString GetQCIQConnectionString(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.QCIQConnectionString.AsNoTracking().Where(q => q.ReviewTypeID == ReviewTypeID).FirstOrDefault();
            }
        }

    }
}
