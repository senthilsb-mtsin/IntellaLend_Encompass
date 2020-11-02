using MTSEntBlocks.DataBlock;
using System.Collections.Generic;
using System.Data;

namespace EphesoftService
{
    /// <summary>
    /// This class provides the methods to access the database.
    /// </summary>
    public class QCIQLookupDataAccess
    {
        #region Private Variable

        private static readonly CustomLogger logger = new CustomLogger("MTSEphesoftServiceDBAccess");

        #endregion

        #region Public Methods

        public DataTable GetMappingDetails(List<string> documentList, string reviewType)
        {
            DataTable dbResult = DataAccess.ExecuteDataTable("GetDocumentFieldMapping", string.Join("|", documentList.ToArray()), reviewType);
            return dbResult;
        }

        public DataSet GetQCIQData(string connectIonString, string sqlScript)
        {
            return DynamicDataAccess.ExecuteSQLDataSet(connectIonString, sqlScript); ;
        }
        #endregion

    }
}