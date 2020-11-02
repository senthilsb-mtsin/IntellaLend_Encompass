using IntellaLend.Model;
using Minio;
using Minio.Exceptions;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IntellaLend.MinIOWrapper
{
    public class ImageMinIOWrapper
    {
        private static string TenantSchema;
        private static string DefaultTenantSchema = "T1";

        private string _accessKey;
        private string _secretKey;
        private string _endPoint;
        private string _region;

        private Loan lastLoanDetails;
        MinIOWrapperDataAccess dataAccess;
        private MinioClient _minioCLient = null;

        #region Constructor

        public ImageMinIOWrapper(string tenantSchema)
        {
            TenantSchema = tenantSchema;
            dataAccess = new MinIOWrapperDataAccess(TenantSchema);
            var config = dataAccess.GetTenantConfig();
            _endPoint = config["ImageEndPoint"];
            _accessKey = config["ImageAccessKey"];
            _secretKey = config["ImageSecretKey"];
            _minioCLient = new MinioClient(_endPoint, _accessKey, _secretKey);
        }

        public ImageMinIOWrapper(string endPoint, string accessKey, string secretKey, string region = "")
        {
            TenantSchema = DefaultTenantSchema;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _endPoint = endPoint;
            _region = region;
            dataAccess = new MinIOWrapperDataAccess(TenantSchema);
            _minioCLient = new MinioClient(_endPoint, _accessKey, _secretKey);
        }

        #endregion

        #region Public Methods

        public void InsertLoanImage(Int64 loanID, byte[] imageByte, System.Guid? imageGUID)
        {
            if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                lastLoanDetails = dataAccess.GetLoanDetails(loanID);


            var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
            var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + imageGUID.GetValueOrDefault().ToString();
            var contentType = "application/image";
            MemoryStream imageStream = new MemoryStream(imageByte);
            UploadFile(bucketName, ObjectName, imageStream, contentType);
        }

        public void InsertLoanImage(Int64 loanID, byte[] imageByte, LoanImage _loanImage)
        {
            try
            {
                dataAccess.InsertLoanImage(_loanImage);

                if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                    lastLoanDetails = dataAccess.GetLoanDetails(loanID);

                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + _loanImage.ImageGUID.GetValueOrDefault().ToString();
                var contentType = "application/image";
                MemoryStream imageStream = new MemoryStream(imageByte);
                UploadFile(bucketName, ObjectName, imageStream, contentType);
            }
            catch (Exception ex)
            {
                if (_loanImage.LoanImageID != null)
                    dataAccess.DeleteLoanImage(_loanImage.LoanImageID);

                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        public void InsertLoanPDF(Int64 loanID, byte[] imageByte)
        {
            try
            {
                Guid _pdfGUIDID = dataAccess.InsertLoanPDF(loanID);

                if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                    lastLoanDetails = dataAccess.GetLoanDetails(loanID);

                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + _pdfGUIDID.ToString();

                //var bucketName = TenantSchema.ToLower() + "stackingorderpdf"; //Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                //var ObjectName = _pdfGUIDID.ToString(); //lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + _pdfGUIDID.ToString();
                var contentType = "application/pdf";
                MemoryStream imageStream = new MemoryStream(imageByte);
                UploadFile(bucketName, ObjectName, imageStream, contentType);
            }
            catch (Exception ex)
            {
                if (loanID != null)
                    dataAccess.DeleteLoanPDF(loanID);

                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        public void MoveLoanPDF(Int64 loanID, byte[] imageByte)
        {
            try
            {
                MinIOStorage _storage = dataAccess.GetLoanStackingOrderDetails(loanID);
                if (_storage != null)
                {
                    var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(_storage.CreatedOn).ToString("yyyyMMdd");
                    var ObjectName = _storage.LoanGUID.GetValueOrDefault().ToString() + "/" + _storage.ObjectGUID.GetValueOrDefault().ToString();
                    var contentType = "application/pdf";
                    MemoryStream imageStream = new MemoryStream(imageByte);
                    UploadFile(bucketName, ObjectName, imageStream, contentType);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        public byte[] GetLoanImage(Int64 loanID, System.Guid? imageGUID)
        {
            if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                lastLoanDetails = dataAccess.GetLoanDetails(loanID);

            var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
            var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + imageGUID.GetValueOrDefault().ToString();
            return GetObject(bucketName, ObjectName);
        }

        public byte[] GetLoanPDF(Int64 loanID)
        {
            MinIOStorage _stored = dataAccess.GetLoanStackingOrderDetails(loanID);

            if (_stored != null)
            {
                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(_stored.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = _stored.LoanGUID.GetValueOrDefault().ToString() + "/" + _stored.ObjectGUID.GetValueOrDefault().ToString();

                return GetObject(bucketName, ObjectName);
            }

            return new byte[0];
        }

        public Stream GetLoanPDFStream(Int64 loanID)
        {
            MinIOStorage _stored = dataAccess.GetLoanStackingOrderDetails(loanID);

            if (_stored != null)
            {
                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(_stored.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = _stored.LoanGUID.GetValueOrDefault().ToString() + "/" + _stored.ObjectGUID.GetValueOrDefault().ToString();

                return GetObjectStream(bucketName, ObjectName);
            }

            return null;
        }
        public void InsertLoanImage(Int64 loanID, string imagePath, System.Guid? imageGUID)
        {
            if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                lastLoanDetails = dataAccess.GetLoanDetails(loanID);

            var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
            var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + imageGUID.GetValueOrDefault().ToString();
            var contentType = "application/image";
            UploadFile(bucketName, ObjectName, imagePath, contentType);
        }

        public void InsertReverificationImage(Int64 ReverificationID, byte[] imageByte, System.Guid? imageGUID)
        {
            //if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
            //    lastLoanDetails = dataAccess.GetReverifi(loanID);

            var bucketName = "reverification";//Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
            var ObjectName = imageGUID.GetValueOrDefault().ToString();
            var contentType = "application/image";
            MemoryStream imageStream = new MemoryStream(imageByte);
            UploadFile(bucketName, ObjectName, imageStream, contentType);
        }

        public void UploadFile(string bucketName, string objectName, Stream fileStream, string contentType)
        {
            bucketName = bucketName.ToLower();
            // Make a bucket on the server, if not already present.
            bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
            if (!found)
            {
                CreateBucket(bucketName);
            }

            Task.Run(() => _minioCLient.PutObjectAsync(bucketName, objectName, fileStream, fileStream.Length, contentType)).GetAwaiter().GetResult();
        }

        public void UploadFile(string bucketName, string objectName, string filePath, string contentType)
        {
            bucketName = bucketName.ToLower();
            // Make a bucket on the server, if not already present.
            bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
            if (!found)
            {
                CreateBucket(bucketName);
            }

            Task.Run(() => _minioCLient.PutObjectAsync(bucketName, objectName, filePath, contentType)).GetAwaiter().GetResult();
        }

        public bool CheckFileExists(Int64 loanID)
        {
            MinIOStorage _stored = dataAccess.GetLoanStackingOrderDetails(loanID);

            if (_stored != null)
            {
                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(_stored.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = _stored.LoanGUID.GetValueOrDefault().ToString() + "/" + _stored.ObjectGUID.GetValueOrDefault().ToString();

                //var bucketName = TenantSchema.ToLower() + "stackingorderpdf";  //Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                //var ObjectName = _stored.ObjectGUID.GetValueOrDefault().ToString(); //lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + _pdfGUIDID.ToString();

                bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
                if (found)
                {
                    try
                    {
                        Task.Run(() => _minioCLient.StatObjectAsync(bucketName, ObjectName)).GetAwaiter().GetResult();
                        return true;
                    }
                    catch (ObjectNotFoundException ex)
                    { return false; }
                    catch (Exception ex)
                    { return false; }
                }
            }
            return false;
        }

        public bool CheckAndDeleteReverificationFile(string bucketName, Guid? logoGuid)
        {
            bool isDeleted = false;
            var ObjectName = logoGuid.GetValueOrDefault().ToString();
            bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
            if (found)
            {
                Task.Run(() => _minioCLient.RemoveObjectAsync(bucketName, ObjectName)).GetAwaiter().GetResult();
                return true;
            }
            else
            {
                return false;
            }

        }

        public void DeleteLoanStackingOrder(Int64 loanID)
        {
            MinIOStorage _stored = dataAccess.GetLoanStackingOrderDetails(loanID);
            if (_stored != null)
            {
                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(_stored.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = _stored.LoanGUID.GetValueOrDefault().ToString() + "/" + _stored.ObjectGUID.GetValueOrDefault().ToString();
                bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
                if (found)
                {
                    Task.Run(() => _minioCLient.RemoveObjectAsync(bucketName, ObjectName)).GetAwaiter().GetResult();
                }
            }
        }

        public void DeleteLoanImage(Int64 loanID, Guid? imageGUID)
        {
            try
            {
                if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                    lastLoanDetails = dataAccess.GetLoanDetails(loanID);

                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                var ObjectName = lastLoanDetails.LoanGUID.GetValueOrDefault().ToString() + "/" + imageGUID.GetValueOrDefault().ToString();
                bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
                if (found)
                {
                    Task.Run(() => _minioCLient.RemoveObjectAsync(bucketName, ObjectName)).GetAwaiter().GetResult();
                    dataAccess.DeleteLoanImage(imageGUID);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        public void DeleteBucket(Int64 loanID)
        {
            try
            {
                if (lastLoanDetails == null || lastLoanDetails.LoanID != loanID)
                    lastLoanDetails = dataAccess.GetLoanDetails(loanID);

                bool loanExits = dataAccess.CheckBucket(lastLoanDetails.CreatedOn);

                var bucketName = TenantSchema.ToLower() + Convert.ToDateTime(lastLoanDetails.CreatedOn).ToString("yyyyMMdd");
                bool found = Task.Run(() => _minioCLient.BucketExistsAsync(bucketName)).GetAwaiter().GetResult();
                if (found)
                {
                    Task.Run(() => _minioCLient.RemoveBucketAsync(bucketName)).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        public void DeleteFile(string bucketName, string objectName)
        {
            Task.Run(() => _minioCLient.RemoveObjectAsync(bucketName, objectName)).GetAwaiter().GetResult();
        }

        public byte[] GetObject(string bucketName, string objectName)
        {
            bucketName = bucketName.ToLower();

            MemoryStream ms = new MemoryStream();

            Task.Run(() => _minioCLient.GetObjectAsync(bucketName, objectName, (stream) =>
                                     { stream.CopyTo(ms); })).GetAwaiter().GetResult();

            ms.Position = 0;
            return StreamToByte(ms);
        }

        public Stream GetObjectStream(string bucketName, string objectName)
        {
            bucketName = bucketName.ToLower();

            MemoryStream ms = new MemoryStream();

            Task.Run(() => _minioCLient.GetObjectAsync(bucketName, objectName, (stream) =>
            { stream.CopyTo(ms); })).GetAwaiter().GetResult();

            ms.Position = 0;
            return ms;
        }

        public void CreateBucket(string bucketName)
        {
            if (String.IsNullOrEmpty(_region))
                Task.Run(() => _minioCLient.MakeBucketAsync(bucketName)).GetAwaiter().GetResult();
            else
                Task.Run(() => _minioCLient.MakeBucketAsync(bucketName, _region)).GetAwaiter().GetResult();
        }

        #endregion

        #region Private Methods

        private static byte[] StreamToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

    }

    public class MinIOStorage
    {
        public DateTime? CreatedOn { get; set; }
        public Guid? LoanGUID { get; set; }
        public Guid? ObjectGUID { get; set; }
    }
}
