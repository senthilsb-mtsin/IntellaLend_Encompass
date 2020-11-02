using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrationTool
{
    public class MigrateTables
    {
        #region Private Static Variables

        private static string _sourceConnectionString;
        private static string _destConnectionString;

        #endregion

        #region Public Methods

        public static void Connect(string sourceConnection, string destConnection)
        {
            _sourceConnectionString = sourceConnection;
            _destConnectionString = destConnection;
        }

        public static void Start(string Schema, string TableName)
        {
            if (_sourceConnectionString != string.Empty && _destConnectionString != string.Empty && Schema != string.Empty && TableName != string.Empty)
            {
                string truncateSQL = $"Truncate table [{Schema}].[{TableName}]";

                //Truncating Destination Table
                DynamicDataAccess.ExecuteSQLNonQuery(_destConnectionString, truncateSQL);

                //if (TableName.Equals("DocumentFieldMasters"))
                //{
                string sqlFieldCount = $"SELECT COUNT(1) AS [Count] FROM [{Schema}].[{TableName}]";

                Int64 _rowCount = 0;

                //Select Source Table Count
                DataTable dt = DynamicDataAccess.ExecuteSQLDataTable(_sourceConnectionString, sqlFieldCount);

                if (dt.Rows.Count > 0)
                    _rowCount = Convert.ToInt64(dt.Rows[0]["Count"]);

                Int64 _firstSet = (Int64)_rowCount / 2;
                Int64 _secoundSet = _rowCount - _firstSet;

                string sqlFirstField = $"SELECT TOP {_firstSet} *  AS [FieldCount] FROM [{Schema}].[{TableName}] ORDER BY 1";
                string sqlSecoundField = $"SELECT TOP {_secoundSet} *  AS [FieldCount] FROM [{Schema}].[{TableName}] ORDER BY 1 DESC";

                DataTable _firstSetDT = DynamicDataAccess.ExecuteSQLDataTable(_sourceConnectionString, sqlFirstField);
                DataTable _secoundSetDT = DynamicDataAccess.ExecuteSQLDataTable(_sourceConnectionString, sqlSecoundField);

                //Bulk Insert Source to Destination Table
                using (var sqlBulk = new SqlBulkCopy(_destConnectionString, SqlBulkCopyOptions.KeepIdentity))
                {
                    sqlBulk.DestinationTableName = $"[{Schema}].[{TableName}]";
                    sqlBulk.WriteToServer(_firstSetDT);
                    sqlBulk.WriteToServer(_secoundSetDT);
                }
                //}
                //else
                //{
                //    string sql = $"SELECT * FROM [{Schema}].[{TableName}]";

                //    //Truncating Destination Table
                //    DynamicDataAccess.ExecuteSQLNonQuery(_destConnectionString, truncateSQL);

                //    //Select Source Table
                //    DataTable dt = DynamicDataAccess.ExecuteSQLDataTable(_sourceConnectionString, sql);
                //}


                ////Bulk Insert Source to Destination Table
                //using (var sqlBulk = new SqlBulkCopy(_destConnectionString, SqlBulkCopyOptions.KeepIdentity))
                //{                    
                //    sqlBulk.DestinationTableName = $"[{Schema}].[{TableName}]";
                //    sqlBulk.WriteToServer(dt);
                //}

            }
        }

        public static bool TestConnection(string connectionStr, string Schema, string TableName, ref string errorMsg)
        {
            try
            {
                string sql = $"SELECT * FROM [{Schema}].[{TableName}]";

                //Truncating Destination Table
                DynamicDataAccess.ExecuteSQLNonQuery(connectionStr, sql);

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

            return true;
        }

        public static DataTable GetSourceDBData(string Sql)
        {
            DataTable dt = new DataTable();
            if (_sourceConnectionString != string.Empty && Sql != string.Empty)
            {
                dt = DynamicDataAccess.ExecuteSQLDataTable(_sourceConnectionString, Sql);
            }
            return dt;
        }

        public static DataSet GetSourceDBDataSet(string Sql)
        {
            DataSet ds = new DataSet();
            if (_sourceConnectionString != string.Empty && Sql != string.Empty)
            {
                ds = DynamicDataAccess.ExecuteSQLDataSet(_sourceConnectionString, Sql);
            }
            return ds;
        }


        public static DataTable GetDestinationDBData(string Sql)
        {
            DataTable dt = new DataTable();
            if (_destConnectionString != string.Empty && Sql != string.Empty)
            {
                dt = DynamicDataAccess.ExecuteSQLDataTable(_destConnectionString, Sql);
            }
            return dt;
        }

        public static void DestinationBulkInsert(string DestinationTableName, DataTable dt)
        {
            using (var sqlBulk = new SqlBulkCopy(_destConnectionString))
            {
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("StackingOrderID", "StackingOrderID"));
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DocumentTypeID", "DocumentTypeID"));
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("SequenceID", "SequenceID"));
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Active", "Active"));
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CreatedOn", "CreatedOn"));
                sqlBulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ModifiedOn", "ModifiedOn"));

                sqlBulk.DestinationTableName = DestinationTableName;
                sqlBulk.WriteToServer(dt);
            }
        }

        public static void NonQueryDestinationDB(string Sql)
        {            
            if (_destConnectionString != string.Empty && Sql != string.Empty)
            {
                DynamicDataAccess.ExecuteSQLNonQuery(_destConnectionString, Sql);
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }

    public class DifferentDocument
    {
        public Int64 SourceDocumentID { get; set; }
        public string DocumentName { get; set; }
        public Int64 DestinationDocumentID { get; set; }
    }

    public class CommonDocument
    {
        public Int64 SourceDocumentID { get; set; }
        public string DocumentName { get; set; }
        public Int64 DestinationDocumentID { get; set; }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public Int64 Value { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public Int64 GetValue()
        {
            return Value;
        }
    }
}
