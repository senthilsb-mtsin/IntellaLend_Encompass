using MTSEntBlocks.DataBlock;
using System.Data;

namespace IntellaLend.EntityDataHandler
{
    public static class QCIQDataAccess
    {
        #region Constructor

        static QCIQDataAccess() { }

        #endregion

        #region Public Methods

        public static DataTable GetQueryResult(string QueryString)
        {
            return DataAccess.ExecuteSQLDataTable(QueryString);
        }

        public static DataSet QCIQLookUp(string QueryString)
        {
            return DataAccess.ExecuteSQLDataSet(QueryString);
        }

        #endregion

    }
}
