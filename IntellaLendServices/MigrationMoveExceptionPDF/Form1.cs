using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigrationMoveExceptionPDF
{
    public partial class Form1 : Form
    {
        ImageMinIOWrapper _imageWrapper = null;
        BackgroundWorker _mWorker;
        private System.Windows.Forms.Timer _timer;
        private DateTime _tickerStartTime = DateTime.MinValue;
        private TimeSpan _tickerCurrentElapsedTime = TimeSpan.Zero;
        private TimeSpan _tickerTotalElapsedTime = TimeSpan.Zero;


        public Form1()
        {
            InitializeComponent();
            _mWorker = new BackgroundWorker();
            _mWorker.DoWork += new DoWorkEventHandler(_mWorker_DoWork);
            _mWorker.ProgressChanged += new ProgressChangedEventHandler
                    (_mWorker_ProgressChanged);
            _mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                    (_mWorker_RunWorkerCompleted);
            _mWorker.WorkerReportsProgress = true;
            _mWorker.WorkerSupportsCancellation = true;
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += new EventHandler(_timer_Tick);
        }
        private void _timer_Tick(object sender, EventArgs e)
        {
            var timeSinceStartTime = DateTime.Now - _tickerStartTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);

            _tickerCurrentElapsedTime = timeSinceStartTime + _tickerTotalElapsedTime;
            _migrationDurationLabel.Text = timeSinceStartTime.ToString();
        }

        private void _processBtn_Click(object sender, EventArgs e)
        {
            _tickerStartTime = DateTime.Now;
            _tickerTotalElapsedTime = _tickerCurrentElapsedTime;
            _timer.Start();
            _processBtn.Enabled = false;
            _cancelBtn.Enabled = true;
            _mWorker.RunWorkerAsync();
        }

        private void _mWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_mWorker.CancellationPending)
            {
                e.Cancel = true;
                _mWorker.ReportProgress(0);
                return;
            }
            _processingLabel.Invoke(new Action(() => _processingLabel.Text = "Migration Started"));


            DateTime _startTime = DateTime.Now;
            _startTimeLabel.Invoke(new Action(() => _startTimeLabel.Text = _startTime.ToString("MM-dd-yyyy hh:mm:ss")));
            _imageWrapper = new ImageMinIOWrapper(_minIOURLTextBox.Text, _minIOAccessKeyTextBox.Text, _minIOSecretKeyTextBox.Text);

            LoanMigrationDataAccess _dataAccess = new LoanMigrationDataAccess();

            List<PDFObj> _pdfDetails = _dataAccess.GetLoanPDFDeatils();

            for (int i = 0; i < _pdfDetails.Count; i++)
            {
                PDFObj _pdfObj = _pdfDetails[i];
                ProcessLoans(_pdfObj, _dataAccess);
                int dProgress = ((int)(((decimal)i / (decimal)_pdfDetails.Count) * 100));
                _mWorker.ReportProgress(dProgress);
            }

            _mWorker.ReportProgress(100);

            DateTime _endTime = DateTime.Now;

            _processingLabel.Invoke(new Action(() => _processingLabel.Text = "Migration Completed"));
        }

        private void ProcessLoans(PDFObj _pdfObj, LoanMigrationDataAccess _dataAccess)
        {
            try
            {
                if (_pdfObj != null)
                {
                    _processingLabel.Invoke(new Action(() => _processingLabel.Text = $"{_pdfObj.LoanPDFID.ToString()}"));
                    StartMigration(_pdfObj, _dataAccess);
                }
                else
                {
                    _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"{_pdfObj.LoanPDFID.ToString()}, Message : Image Object NULL {Environment.NewLine}"));
                    Exception exc = new Exception($"ImageID : {_pdfObj.LoanPDFID.ToString()}");
                    MTSExceptionHandler.HandleException(ref exc);
                }
            }
            catch (Exception ex)
            {
                _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"{_pdfObj.LoanPDFID.ToString()}, Message : {ex.Message} {Environment.NewLine}"));
                Exception exc = new Exception($"ImageID : {_pdfObj.LoanPDFID.ToString()}", ex);
                MTSExceptionHandler.HandleException(ref exc);
            }
        }

        private void StartMigration(PDFObj _pdfObj, LoanMigrationDataAccess _dataAccess)
        {
            Guid LoanPDFGUID = Guid.NewGuid();
            try
            {        
                _imageWrapper.MoveLoanPDF(_pdfObj.LoanID, File.ReadAllBytes(_pdfObj.LoanPDFPath));
                //_dataAccess.UpdateLoanPDFGUID(_pdfObj.LoanPDFID, LoanPDFGUID);
            }
            catch (Exception ex)
            {
                _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"LoanPDFID : {_pdfObj.LoanPDFID.ToString()}, Message : {ex.Message} {Environment.NewLine}"));
                Exception exc = new Exception($"LoanID PDF ID : {_pdfObj.LoanPDFID.ToString()}", ex);
                MTSExceptionHandler.HandleException(ref exc);
            }
        }

        //private byte[] CompressImage(byte[] _compressedImg)
        //{
        //    CompressedImage _cImg = new CompressedImage();

        //    using (Stream sr = new MemoryStream(_compressedImg))
        //    {
        //        using (SDI.Bitmap bmp = new SDI.Bitmap(sr))
        //        {
        //            _cImg = _imageUtility.ResizeTiff(bmp, _maxWidth, _maxHeight);
        //        }
        //    }

        //    return _cImg.Image;
        //}

        private void _mWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void _mWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _processingLabel.Text = "Task Cancelled.";
            }
            else if (e.Error != null)
            {
                _processingLabel.Text = "Error while performing background operation.";
            }
            else
            {
                _processingLabel.Text = "Migration Completed...";
            }
            _timer.Stop();
            _tickerTotalElapsedTime = TimeSpan.Zero;
            _tickerCurrentElapsedTime = TimeSpan.Zero;
        }

        private void _cancelBtn_Click(object sender, EventArgs e)
        {
            if (_mWorker.IsBusy)
            {
                _mWorker.CancelAsync();
                _processBtn.Enabled = true;
            }
        }
    }

    public class PDFObj
    {
        public Int64 LoanID { get; set; }
        public Int64 LoanPDFID { get; set; }
        public Guid LoanGUID { get; set; }
        public string LoanPDFPath { get; set; }
    }
}
