using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDI = System.Drawing;



namespace LoanImageMigration
{
    public partial class Form1 : Form
    {
        Int64 fromLoanID = 0;
        Int64 lastLoanID = 0;
        Int64 count = 10;
        Int32 _maxWidth = 1654;
        Int32 _maxHeight = 2339;
        ImageUtilities _imageUtility = null;
        ImageMinIOWrapper _imageWrapper = null;
        BackgroundWorker _mWorker;
        private System.Windows.Forms.Timer _timer;
        private DateTime _tickerStartTime = DateTime.MinValue;
        private TimeSpan _tickerCurrentElapsedTime = TimeSpan.Zero;
        private TimeSpan _tickerTotalElapsedTime = TimeSpan.Zero;


        public Form1()
        {
            InitializeComponent();
            _imageUtility = new ImageUtilities();
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
            _migrationDuration.Text = timeSinceStartTime.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_fromLoanIDTextBox.Text) && string.IsNullOrEmpty(_countTextBox.Text) && string.IsNullOrEmpty(_minIOURLTextBox.Text) && string.IsNullOrEmpty(_minIOAccessKeyTextBox.Text) && string.IsNullOrEmpty(_minIOSecretKeyTextBox.Text))
            {
                MessageBox.Show("Textbox cannot be EMPTY");
            }
            else
            {
                try
                {
                    Int64.TryParse(_fromLoanIDTextBox.Text, out fromLoanID);
                    Int64.TryParse(_countTextBox.Text, out count);

                    if (fromLoanID > 0 && count > 0)
                    {
                        _tickerStartTime = DateTime.Now;
                        _tickerTotalElapsedTime = _tickerCurrentElapsedTime;
                        _timer.Start();
                        _processBtn.Enabled = false;
                        _cancelBtn.Enabled = true;
                        _mWorker.RunWorkerAsync();
                    }
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                    MTSExceptionHandler.HandleException(ref ex);
                }
            }
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

            Int64 _latestLoanID = fromLoanID; int _progressID = 1;
            LoanMigrationDataAccess _dataAccess = new LoanMigrationDataAccess();
            if (_recursiveCheckBox.Checked)
            {
                lastLoanID = _dataAccess.GetLoanID();
                lastLoanID = lastLoanID - _latestLoanID;
                Int32 _lastSet = 0, _setLength = 0;
                Int64 _endLoanID = 0;
                _setLength = Math.DivRem(Convert.ToInt32(lastLoanID), Convert.ToInt32(count), out _lastSet);
                int i = 0;
                while (i < _setLength)
                {
                    _endLoanID = _latestLoanID + count;
                    _latestLoanID = ProcessLoans(_latestLoanID, _endLoanID, lastLoanID, _dataAccess, ref _progressID);
                    i++;
                }
                _endLoanID = _latestLoanID + _lastSet;
                _latestLoanID = ProcessLoans(_latestLoanID, _endLoanID, lastLoanID, _dataAccess, ref _progressID);
            }
            else
            {
                ProcessLoans(_latestLoanID, fromLoanID + count, count, _dataAccess, ref _progressID);
            }

            _mWorker.ReportProgress(100);

            DateTime _endTime = DateTime.Now;

            _endDateTimeLabel.Invoke(new Action(() => _endDateTimeLabel.Text = _endTime.ToString("MM-dd-yyyy hh:mm:ss")));

            _processingLabel.Invoke(new Action(() => _processingLabel.Text = "Migration Completed"));

            _pageProcessingLabel.Invoke(new Action(() => _pageProcessingLabel.Text = ""));
        }

        private Int64 ProcessLoans(Int64 startLoanID, Int64 lastLength, Int64 total, LoanMigrationDataAccess _dataAccess, ref Int32 _progressID)
        {
            Int64 _loanID = 0;
            for (_loanID = startLoanID; _loanID < lastLength; _loanID++)
            {
                try
                {
                    _processingLabel.Invoke(new Action(() => _processingLabel.Text = $"Processing LoanID : {_loanID.ToString()}"));
                    Int64? _loan = _dataAccess.GetMigrationLoan(_loanID);
                    if (_loan != null)
                        StartMigration(_loan.GetValueOrDefault(), _dataAccess);
                }
                catch (Exception ex)
                {
                    _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"{_loanID.ToString()}, Message : {ex.Message} {Environment.NewLine}"));
                    Exception exc = new Exception($"LoanID : {_loanID.ToString()}", ex);
                    MTSExceptionHandler.HandleException(ref exc);
                }
                int dProgress = ((int)(((decimal)_progressID / (decimal)total) * 100));
                _mWorker.ReportProgress(dProgress);
                _progressID++;
            }
            return _loanID;
        }

        private void StartMigration(Int64 _loanID, LoanMigrationDataAccess _dataAccess)
        {
            bool _imageMigrationCompleted = true, _pdfMigrationCompleted = true;

            string LoanGUID = _dataAccess.UpdateLoanGUID(_loanID);

            List<Int64> _loanImageList = _dataAccess.GetLoanImageIDs(_loanID);

            foreach (Int64 _loanImageID in _loanImageList)
            {
                ImageObj _loanImage = _dataAccess.GetLoanImage(_loanImageID);

                if (_loanImage != null)
                {
                    try
                    {
                        _pageProcessingLabel.Invoke(new Action(() => _pageProcessingLabel.Text = $"Processing Loan Image ID : {_loanImageID.ToString()}"));

                        //LoanMigrationDataAccess.UpdateLoanImageGUID(_loanImage);

                        byte[] _compressedImg = CompressImage(_loanImage.ImageBytes);

                        _imageWrapper.InsertLoanImage(_loanID, _compressedImg, _loanImage.ImageGuid);
                    }
                    catch (Exception ex)
                    {
                        _imageMigrationCompleted = false;
                        _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"Loan ImageID : {_loanImageID.ToString()} {Environment.NewLine}"));
                        Exception exc = new Exception($"Loan ImageID : {_loanImageID.ToString()}", ex);
                        MTSExceptionHandler.HandleException(ref exc);
                    }
                }
            }


            PDFObj _loanPDF = _dataAccess.GetLoanPDF(_loanID);

            if (_loanPDF != null)
            {
                //LoanMigrationDataAccess.UpdateLoanPDFGUID(_loanPDF);

                string _pdfPath = Path.Combine(Path.GetDirectoryName(_loanPDF.LoanPDFPath), _loanPDF.LoanPDFGUID.ToString());
                try
                {
                    //File.Move(_loanPDF.LoanPDFPath, _pdfPath);
                    _pageProcessingLabel.Invoke(new Action(() => _pageProcessingLabel.Text = $"Moving Loan PDF : {_loanPDF.LoanPDFPath}"));
                    _imageWrapper.MoveLoanPDF(_loanID, File.ReadAllBytes(_loanPDF.LoanPDFPath));
                }
                catch (Exception ex)
                {
                    _pdfMigrationCompleted = false;
                    _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"LoanID : {_loanID.ToString()}, Message : {ex.Message} {Environment.NewLine}"));
                    Exception exc = new Exception($"LoanID PDF : {_loanID.ToString()}", ex);
                    MTSExceptionHandler.HandleException(ref exc);
                }
            }
            else
            {
                _exceptionLoanIDs.Invoke(new Action(() => _exceptionLoanIDs.Text += $"Loan PDF not Available : {_loanID.ToString()} {Environment.NewLine}"));
            }

            if (_imageMigrationCompleted && _pdfMigrationCompleted)
            {
                _migratedLoanIDs.Invoke(new Action(() => _migratedLoanIDs.Text += $"{_loanID.ToString()} {Environment.NewLine}"));
            }
            _pageProcessingLabel.Invoke(new Action(() => _pageProcessingLabel.Text = ""));
        }

        private byte[] CompressImage(byte[] _compressedImg)
        {
            CompressedImage _cImg = new CompressedImage();

            using (Stream sr = new MemoryStream(_compressedImg))
            {
                using (SDI.Bitmap bmp = new SDI.Bitmap(sr))
                {
                    _cImg = _imageUtility.ResizeTiff(bmp, _maxWidth, _maxHeight);
                }
            }

            return _cImg.Image;
        }

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
            _processBtn.Enabled = true;
            _cancelBtn.Enabled = false;
            _timer.Stop();
            _tickerTotalElapsedTime = TimeSpan.Zero;
            _tickerCurrentElapsedTime = TimeSpan.Zero;
        }

        private void btnStartAsyncOperation_Click_1(object sender, EventArgs e)
        {
            _processBtn.Enabled = false;
            _cancelBtn.Enabled = true;
            _mWorker.RunWorkerAsync();
        }

        private void _cancelBtn_Click(object sender, EventArgs e)
        {
            if (_mWorker.IsBusy)
                _mWorker.CancelAsync();
        }

        private void _resetBtn_Click(object sender, EventArgs e)
        {
            _startTimeLabel.Text = "";
            _endDateTimeLabel.Text = "";
            _migrationDuration.Text = "";
            _exceptionLoanIDs.Text = "";
            _migratedLoanIDs.Text = "";
            progressBar1.Value = 0;
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    LoanMigrationDataAccess.GenerateLoanGUID();
        //}
    }

    public class ImageObj
    {
        public Guid ImageGuid { get; set; }
        public byte[] ImageBytes { get; set; }
    }

    public class PDFObj
    {
        public Guid LoanPDFGUID { get; set; }
        public string LoanPDFPath { get; set; }
    }
}
