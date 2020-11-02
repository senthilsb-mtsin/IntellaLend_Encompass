using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using MTSXMLParsing;
using Newtonsoft.Json;
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

namespace MoveImageAndPDFToMinIO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void _moveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_loanIDtxt.Text) && string.IsNullOrEmpty(_stackingOrderPDFtxt.Text) && string.IsNullOrEmpty(_ephesoftOutputPathtxt.Text))
            {
                MessageBox.Show("Enter all Fields");
            }
            else {
                _moveLoanImageAndPDF();
            }
        }

        private void _moveLoanImageAndPDF()
        {
            MoveToMinIODataAccess _dataAccess = new MoveToMinIODataAccess();
            Int64 LoanID = 0;
            Int64.TryParse(_loanIDtxt.Text, out LoanID);

            if (LoanID != 0)
            {
                Loan _loan = _dataAccess.GetLoan(LoanID);
                AuditLoanDetail _loanDetail = _dataAccess.GetLoanDetail(LoanID);
              //  string[] xmlFiles = Directory.GetFiles(_ephesoftOutputPathtxt.Text, "*.don", SearchOption.AllDirectories);
                if (_loanDetail != null)
                {
                    Batch batchObj = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);                    
                    Int64 _pageCount = _dataAccess.InsertImagesToDB(batchObj, "2339", "1654");

                    _loan.PageCount = _pageCount;
                    _loan.ModifiedOn = DateTime.Now;

                    _dataAccess.saveChange(_loan);

                    _dataAccess.GenerateLoanPdfByStackingOrder(batchObj, _stackingOrderPDFtxt.Text);
                }


            }
        }
        public void Log(string _msg)
        {
            Exception ex = new Exception(_msg);
            MTSExceptionHandler.HandleException(ref ex);
        }


    }
}
