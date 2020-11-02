using MTSEntBlocks.DataBlock;
using System.Data;

namespace EphesoftService
{
    /// <summary>
    /// This class provides the methods to access the database.
    /// </summary>
    public class AutoValidationDataAccess
    {
        #region Private Variable

        private static readonly CustomLogger logger = new CustomLogger("MTSEphesoftServiceDBAccess");

        #endregion

        #region Public Methods

        public DataTable GetDocumentsToSkip()
        {
            DataTable dbResult = DataAccess.ExecuteDataTable("GetDocumentAutoValidationSkip");
            return dbResult;
        }
        #endregion

    }
}