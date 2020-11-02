using EphesoftService.Models;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace EphesoftService
{
    /// <summary>
    /// This class provides the methods to access the database.
    /// </summary>
    public class MTSEphesoftServiceDBAccess
    {
        #region Private Variable

        private static readonly CustomLogger logger = new CustomLogger("MTSEphesoftServiceDBAccess");

        #endregion

        #region Public Methods


        /// <summary>
        /// This method retrieves the configId from the [MTS.IDC_RULE_CONFIG] table for the 
        /// batch class Id of the input Ephesoft XML.
        /// </summary>
        /// <param name="EphesoftBatchClass">The value of the BatchClassIdentifier tag in the input XML</param>
        /// <returns>The configId in the database for the input batch class identifier</returns>
        public int GetCongidId(string ephesoftBatchClass)
        {
            logger.Debug("Retrieve the Config Id from the database for the input batch class Id");

            int configId = 0;
            System.Data.DataTable dbResult = new System.Data.DataTable();
            try
            {
                dbResult = DataAccess.ExecuteDataTable("GetConfigId", ephesoftBatchClass);
                if (dbResult.Rows.Count == 1)
                {
                    configId = int.Parse(dbResult.Rows[0]["ConfigId"].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error retrieving the configId for the batch class of the input XML. BatchClass - " + ephesoftBatchClass, ex);
                throw new MTSPassThruException("Error retrieving the ConfigId for the batch class of the input XML from the database");
            }

            if (dbResult.Rows.Count > 1)
            {
                logger.Error("Multiple mapping found in IDC_RULE_CONFIG for Ephesoft Batch classs Id - " + ephesoftBatchClass);
                throw new MTSPassThruException("Error retrieving the ConfigId for the batch class of the input XML from the database");
            }
            if (dbResult.Rows.Count == 0)
            {
                logger.Error("No mapping found in IDC_RULE_CONFIG for Ephesoft Batch classs Id - " + ephesoftBatchClass);
                throw new MTSPassThruException("Error retrieving the ConfigId for the batch class of the input XML from the database");
            }
            logger.Debug(String.Format("ConfigId for Batch class {0} is {1}", ephesoftBatchClass, configId));
            return configId;
        }

        public List<MergeDocuments> GetMergeDocumentConfig(int configId)
        {
            System.Data.DataTable dt = DataAccess.ExecuteDataTable("GetMergeConfigDocTypes", configId);

            return JsonConvert.DeserializeObject<List<MergeDocuments>>(JsonConvert.SerializeObject(dt));
        }

        /// <summary>
        /// This method retrieves the conversion rules defined for the given ConfigId and module from MTS.IDC_DOC_CONVERSION table. 
        /// </summary>
        /// <param name="configId">The configId for the current input XML</param>
        /// <param name="ephesoftModule">The module from which the webservice is invoked.</param>
        /// <returns>The conversion rules for the configId</returns>
        public System.Data.DataTable GetConversionRules(int configId, Int64 ephesoftModuleId)
        {
            logger.Debug("Get the conversion rules from the DB for the config Id - " + configId);
            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetConversionRules", configId, ephesoftModuleId);
            return dbResult;
        }


        public System.Data.DataTable GetParentChildMergeRules(int configId)
        {
            logger.Debug("Get the advance merge rules from the DB for the config Id - " + configId);
            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetParentChildMergeRules", configId);
            return dbResult;
        }


        /// <summary>
        /// This method retrieves the concatenate rules defined for the given ConfigId from MTS.IDC_DOC_CONCATENATE table. 
        /// </summary>
        /// <param name="configId">The configId for the current input XML</param>
        /// <returns>The System.Data.DataTable containing the concatenate rules for the configId</returns>
        public System.Data.DataTable GetConcatenateRules(int configId, int ephesoftModule)
        {
            logger.Debug("Get the concatenate rules from the DB for the config Id - " + configId);

            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetConcatenateRules", configId, ephesoftModule);
            return dbResult;
        }

        /// <summary>
        /// This method retrieves the append rules defined for the given ConfigId from MTS.IDC_DOC_APPEND table. 
        /// </summary>
        /// <param name="configId">The configId for the current input XML</param>
        /// <returns>The System.Data.DataTable containing the append rules for the configId</returns>
        public System.Data.DataTable GetAppendRules(int configId)
        {
            logger.Debug("Get the append rules from the DB for the config Id - " + configId);

            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetAppendRules", configId);
            return dbResult;
        }


        public System.Data.DataTable GetDocumentStackingOrder(int configID)
        {
            logger.Debug("Get Document Stacking Order with Document Name from DB for the ConfigID -" + configID);
            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetDocStackingOrder", configID);
            return dbResult;
        }

        /// <summary>
        /// This method retrieves the advanced rules for the given ConfigId from MTS.IDC_DOC_CONFIG table. 
        /// </summary>
        /// <param name="configId">The configId for the current input XML</param>
        /// <returns>The System.Data.DataTable containing the advanced rules for the configId</returns>
        public System.Data.DataTable GetAdvancedRules(int configId)
        {
            logger.Debug("Get the advanced rules from the DB for the config Id - " + configId);

            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetAdvancedRules", configId);
            return dbResult;
        }


        public System.Data.DataTable GetMappingDetails(List<string> documentList)
        {
            System.Data.DataTable dbResult = DataAccess.ExecuteDataTable("GetDocumentFieldMapping", string.Join("|", documentList.ToArray()));
            return dbResult;
        }
        #endregion

    }
}