using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseMigrationTool
{
    public partial class Form1 : Form
    {
        #region Private Static Variables

        private static string _srcConnectionStr = string.Empty;
        private static string _destConnectionStr = string.Empty;
        private static string _sysSchema = "IL";
        private static string _tenantSchema = "T1";
        private List<string> _sysTables = new List<string>()
        {
            "AppConfig",
            "CheckListDetailMasters",
            "CheckListMasters",
            "CustLoanDocMapping",
            "CustReverificationDocMapping",
            "CustReviewLoanCheckMapping",
            "CustReviewLoanMapping",
            "CustReviewLoanReverifyMapping",
            "CustReviewLoanStackMapping",
            "CustReviewMapping",
            "DocumentFieldMasters",
            "DocumentTables",
            "DocumentTypeMasters",
            "DocumetTypeTables",
            "EmailSchedule",
            "EmailTemplate",
            "LoanTypeMasters",
            "MenuGroupMasters",
            "MenuMasters",
            "QCIQConnectionString",
            "ReverificationMasters",
            "ReverificationTemplates",
            "ReviewPriorityMasters",
            "ReviewTypeMasters",
            "RoleMasters",
            "RoleMenuMappings",
            "RuleMasters",
            "SecurityQuestionMasters",
            "ServiceConfigs",
            "SMTPDETAILS",
            "StackingOrderDetailMasters",
            "StackingOrderMasters",
            "TenantMasters",
            "WorkFlowStatusMaster" };

        private List<string> _tenantTables = new List<string>()
        {
            "AccessURLs",
            "CheckListDetailMasters",
            "CheckListMasters",
            "CustLoanDocMapping",
            "CustomAddressDetails",
            "CustomerConfig",
            "CustomerMasters",
            "CustReverificationDocMapping",
            "CustReviewLoanCheckMapping",
            "CustReviewLoanMapping",
            "CustReviewLoanReverifyMapping",
            "CustReviewLoanStackMapping",
            "CustReviewMapping",
            "DocumentFieldMasters",
            "DocumentTypeMasters",
            "DocumetTypeTables",
            "LoanTypeMasters",
            "MenuGroupMasters",
            "MenuMasters",
            "QCIQConnectionString",
            "ReverificationMasters",
            "ReverificationTemplates",
            "ReviewPriorityMasters",
            "ReviewTypeMasters",
            "RoleMasters",
            "RoleMenuMappings",
            "RuleMasters",
            "SecurityQuestionMasters",
            "StackingOrderDetailMasters",
            "StackingOrderMasters",
            "UserAddressDetails",
            "UserRoleMappings",
            "Users",
            "UserSecurityQuestion",
             };

        #endregion

        #region SQL Queries

        //#region SQL Select Queries

        private static string _getLoanTypes = "SELECT [LoanTypeID], [LoanTypeName], [Type], [LoanTypePriority], [Active], [CreatedOn], [ModifiedOn] FROM [IL].LoanTypeMasters WITH(NOLOCK)";
        //private static string _getDocumentType = "SELECT DocumentTypeID FROM [IL].DocumentTypeMasters WITH(NOLOCK) WHERE Name = '{Name}'";
        //private static string _getLoanType = "SELECT [LoanTypeID], [LoanTypeName], [Type], [LoanTypePriority], [Active], [CreatedOn], [ModifiedOn] FROM [IL].LoanTypeMasters WITH(NOLOCK) WHERE LoanTypeName LIKE '{LoanTypeName}%'";
        //private static string _getLoanTypeDocs = "SELECT D.[DocumentTypeID], D.[Name] FROM [IL].CustLoanDocMapping CD WITH(NOLOCK) INNER JOIN [IL].DocumentTypeMasters D WITH(NOLOCK) ON CD.DocumentTypeID = D.DocumentTypeID WHERE CD.CustomerID = 1 AND CD.LoanTypeID = {LOANTYPEID}";
        //private static string _getDocMaster = "SELECT D.[DocumentTypeID], D.[Name] FROM [IL].DocumentTypeMasters D WITH(NOLOCK)";
        //private static string _getDocTableMaster = "SELECT Name,DisplayName, DocOrderByField, AllowAccuracyCalc FROM [IL].DocumentFieldMasters D WITH(NOLOCK) WHERE DocumentTypeID = {DocumentTypeID}; SELECT TableName, TableJson FROM [IL].DocumetTypeTables D WITH(NOLOCK) WHERE DocumentTypeID = {DocumentTypeID}; ";
        //private static string _getLoanCheckList = "select CM.CheckListID, CM.CheckListName from [IL].[CustReviewLoanCheckMapping] CD WITH(NOLOCK) INNER JOIN [IL].[CheckListMasters] CM WITH(NOLOCK) ON CD.CheckListID = CM.CheckListID WHERE CD.CustomerID = 1 AND  CD.ReviewTypeID = 0 AND CD.LoanTypeID = {LoanTypeID}";
        //private static string _getLoanCheckListMaster = "select CheckListName from [IL].[CheckListMasters] where CheckListID = {CheckListID}";
        //private static string _getLoanCheckListMasterWithName = "select top 1 CheckListID from [IL].[CheckListMasters] Order by 1 desc";
        //private static string _getLoanCheckListDetail = "select CheckListDetailID from [IL].[CheckListDetailMasters] where CheckListID = {CheckListID} AND Description = '{Description}'";
        //private static string _getLoanRuleMaster = "select RuleID from [IL].[RuleMasters] where CheckListDetailID = {CheckListDetailID}";
        //private static string _getRuleMaster = "select RuleDescription,RuleJson,DocumentType,ActiveDocumentType from [IL].[RuleMasters] where CheckListDetailID = {CheckListDetailID}";
        //private static string _getLoanCheckListDetailMaster = "SELECT CheckListDetailID, CheckListID,Description,Name,UserID,Rule_Type,SequenceID from [IL].[CheckListDetailMasters] CD WITH(NOLOCK) WHERE CD.CheckListID = {CheckListID}";
        //private static string _getLoanStackingOrder = "select S.StackingOrderID, S.Description FROM [IL].[CustReviewLoanStackMapping] CD INNER JOIN [IL].[StackingOrderMasters] S ON S.StackingOrderID = CD.StackingOrderID where CustomerID =1 and ReviewTypeID= 0  and LoanTypeID = {LoanTypeID}";
        //private static string _getStackingOrderDetail = "select StackingOrderID,DocumentTypeID,SequenceID from [IL].[StackingOrderDetailMasters] where StackingOrderID = {StackingOrderID}";
        //private static string _getLoanStackingOrderWithDesc = "select top 1 S.StackingOrderID, S.Description FROM [IL].[StackingOrderMasters] S Order by 1 desc";
        //#endregion

        //#region SQL Insert Queries

        //private static string _insertLoanTypeMaster = "INSERT INTO [IL].LoanTypeMasters (LoanTypeName,Active,Type,LoanTypePriority,CreatedOn,ModifiedOn) VALUES ('{LoanTypeName}',{Active},0,NULL,'{CreatedOn}','{ModifiedOn}'  )";
        //private static string _insertCustLoanDocMapping = "INSERT INTO [IL].[CustLoanDocMapping] (CustomerID,LoanTypeID,DocumentTypeID,Active,CreatedOn,ModifiedOn) VALUES ({CustomerID},{LoanTypeID},{DocumentTypeID},{Active},'{CreatedOn}','{ModifiedOn}'  )";
        //private static string _insertDocMaster = "INSERT INTO [IL].DocumentTypeMasters (Name,DisplayName,Active,CreatedOn,ModifiedOn,DocumentLevel) VALUES ('{Name}','{DisplayName}',1,'{CreatedOn}','{ModifiedOn}', 0)";
        //private static string _insertDocFieldMaster = "INSERT INTO [IL].DocumentFieldMasters (DocumentTypeID,Name,DisplayName,CreatedOn,ModifiedOn,Active,DocOrderByField,AllowAccuracyCalc) VALUES ({DocumentTypeID}, '{Name}','{DisplayName}','{CreatedOn}','{ModifiedOn}',1, '{DocOrderByField}', NULL)";
        //private static string _insertDocTableMaster = "INSERT INTO [IL].DocumetTypeTables (DocumentTypeID,TableName,TableJson,CreatedDate,ModifiedDate) VALUES ({DocumentTypeID},'{TableName}','{TableJson}','{CreatedOn}','{ModifiedOn}')";
        //private static string _insertCheckListMaster = "INSERT INTO [IL].[CheckListMasters] ([CheckListName] ,[Active] ,[CreatedOn] ,[ModifiedOn]) VALUES ('{CheckListName}' ,1 ,'{CreatedOn}' ,'{ModifiedOn}')";
        //private static string _insertCheckListDetailMaster = "INSERT INTO [IL].[CheckListDetailMasters] ([CheckListID] ,[Description] ,[Active] ,[Name] ,[CreatedOn] ,[ModifiedOn] ,[UserID] ,[Rule_Type] ,[SequenceID])     VALUES ({CheckListID} ,'{Description}' ,1 ,'{Name}','{CreatedOn}' ,'{ModifiedOn}' ,1 ,{Rule_Type} ,{SequenceID})";
        //private static string _insertRuleMaster = "INSERT INTO [IL].[RuleMasters] ([CheckListDetailID] ,[RuleDescription] ,[Active] ,[RuleJson] ,[DocumentType] ,[CreatedOn] ,[ModifiedOn] ,[ActiveDocumentType])     VALUES ({CheckListDetailID} ,'{RuleDescription}' ,1 ,'{RuleJson}' ,'{DocumentType}' ,'{CreatedOn}' ,'{ModifiedOn}' ,'{ActiveDocumentType}')";
        //private static string _insertCustReviewLoanCheckMapping = "INSERT INTO [IL].[CustReviewLoanCheckMapping]([CustomerID],[ReviewTypeID],[LoanTypeID],[CheckListID],[CreatedOn],[ModifiedOn],[Active])     VALUES({CustomerID},{ReviewTypeID},{LoanTypeID},{CheckListID},'{CreatedOn}','{ModifiedOn}',1)";
        //private static string _insertStackingOrder = "INSERT INTO [IL].[StackingOrderMasters]([Description],[Active],[CreatedOn],[ModifiedOn])     VALUES('{Description}',1,'{CreatedOn}','{ModifiedOn}')";
        //private static string _insertCustReviewLoanStackMapping = "INSERT INTO [IL].[CustReviewLoanStackMapping]([CustomerID],[ReviewTypeID],[LoanTypeID],[StackingOrderID],[CreatedOn],[ModifiedOn],[Active])     VALUES({CustomerID},{ReviewTypeID},{LoanTypeID},{StackingOrderID},'{CreatedOn}','{ModifiedOn}',1)";

        //#endregion

        //#region SQL Update Queries

        //private static string _updateCheckListMaster = "UPDATE [IL].[CheckListMasters] SET CheckListName = '{CheckListName}' WHERE CheckListID = {CheckListID}";

        //#endregion

        //#region SQL Delete Queries

        //private static string _deleteCheckListMaster = "DELETE FROM [IL].[CheckListDetailMasters] WHERE CheckListDetailID = {CheckListDetailID}"; 
        //private static string _deleteRuleMaster = "DELETE FROM [IL].[RuleMasters]  WHERE CheckListDetailID = {CheckListDetailID}";

        //#endregion

        #endregion

        public Form1()
        {
            InitializeComponent();
            _plsWaitLabel.Visible = false;
        }

        private bool _testConnection()
        {
            _srcConnectionStr = $"Database={_sourceDatabaseTxt.Text};Server={_sourceServerTxt.Text};User ID={_sourceUserNameTxt.Text};Password={_sourcePwdTxt.Text};Trusted_Connection=False";
            _destConnectionStr = $"Database={_destDatabaseTxt.Text};Server={_destServerTxt.Text};User ID={_destUserNameTxt.Text};Password={_destPwdTxt.Text};Trusted_Connection=False";

            string sourceErrorMsg = string.Empty, destErrorMsg = string.Empty;
            bool sourceConnection = testConnection(_srcConnectionStr, ref sourceErrorMsg);
            bool destConnection = testConnection(_destConnectionStr, ref destErrorMsg);

            if (sourceConnection && destConnection)
            {
                return true;
            }
            else
            {
                string sourceStatus = sourceConnection ? "PASS" : $"FAIL, Error : {sourceErrorMsg}";
                string destStatus = destConnection ? "PASS" : $"FAIL, Error : {destErrorMsg}";

                string msg = $"Source Connection : {sourceStatus} {Environment.NewLine} Destination Connection : {destStatus}";

                MessageBox.Show(msg, "Connection Failure", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            return false;
        }

        private void _startMigration()
        {
            if (MessageBox.Show("Destination tables will be truncated. Do you still want to continue ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                _sysTableProgressGrid.Rows.Clear();
                _tenantTableProgressGrid.Rows.Clear();

                MigrateTables.Connect(_srcConnectionStr, _destConnectionStr);

                int rowSysPosition = 0;
                foreach (string tableName in _sysTables)
                {
                    string errorMsg = string.Empty, status = "Progress...";
                    _sysTableProgressGrid.Rows.Insert(rowSysPosition, tableName, status, errorMsg);
                    try
                    {
                        MigrateTables.Start(_sysSchema, tableName);
                        status = "Pass";
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        status = "Failed";
                        MTSExceptionHandler.HandleException(ref ex);
                    }
                    _sysTableProgressGrid.Rows.RemoveAt(rowSysPosition);
                    _sysTableProgressGrid.Rows.Insert(rowSysPosition, tableName, status, errorMsg);
                    rowSysPosition++;
                }

                int rowTenantPosition = 0;
                foreach (string tableName in _tenantTables)
                {
                    string errorMsg = string.Empty, status = "Progress...";
                    _tenantTableProgressGrid.Rows.Insert(rowTenantPosition, tableName, status, errorMsg);
                    try
                    {
                        MigrateTables.Start(_tenantSchema, tableName);
                        status = "Pass";
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        status = "Failed";
                        MTSExceptionHandler.HandleException(ref ex);
                    }
                    _tenantTableProgressGrid.Rows.RemoveAt(rowTenantPosition);
                    _tenantTableProgressGrid.Rows.Insert(rowTenantPosition, tableName, status, errorMsg);
                    rowTenantPosition++;
                }

                MessageBox.Show("Migration Completed!!!");
            }
        }

        private void _migrateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    if (_testConnection())
                        _startMigration();
                }
                else
                {
                    MessageBox.Show("Fill all the fields");
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        private bool validate()
        {
            //return true;
            return _sourceServerTxt.Text != string.Empty
                && _sourceUserNameTxt.Text != string.Empty
                && _sourcePwdTxt.Text != string.Empty
                && _sourceDatabaseTxt.Text != string.Empty
                && _destServerTxt.Text != string.Empty
                && _destUserNameTxt.Text != string.Empty
                && _destPwdTxt.Text != string.Empty
                && _destDatabaseTxt.Text != string.Empty;
        }

        private void _moveLoanTypeTab_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (validate())
                {
                    if (_testConnection())
                    {
                        MigrateTables.Connect(_srcConnectionStr, _destConnectionStr);
                        DataTable dt = MigrateTables.GetSourceDBData(_getLoanTypes);

                        //foreach (DataRow dr in dt.Rows)
                        //    _sourceLoanTypes.Items.Add(new ComboboxItem() { Text = dr["LoanTypeName"].ToString(), Value = Convert.ToInt64(dr["LoanTypeID"]) });

                        _sourceLoanTypes.DataSource = dt;
                        _sourceLoanTypes.DisplayMember = "LoanTypeName";
                        _sourceLoanTypes.ValueMember = "LoanTypeID";
                    }
                }
                else
                {
                    MessageBox.Show("Fill all Source and Destination fields", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tabControl1.SelectedIndex = 0;
                }
            }
        }

        private bool testConnection(string connectionStr, ref string errorMsg)
        {
            try
            {
                MigrateTables.TestConnection(connectionStr, _sysSchema, _sysTables[0], ref errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }

            return true;
        }

        private void _testConnectionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (validate())
                {
                    string _srcConnectionStr = $"Database={_sourceDatabaseTxt.Text};Server={_sourceServerTxt.Text};User ID={_sourceUserNameTxt.Text};Password={_sourcePwdTxt.Text};Trusted_Connection=False";
                    string _destConnectionStr = $"Database={_destDatabaseTxt.Text};Server={_destServerTxt.Text};User ID={_destUserNameTxt.Text};Password={_destPwdTxt.Text};Trusted_Connection=False";

                    string sourceErrorMsg = string.Empty, destErrorMsg = string.Empty;

                    bool sourceConnection = testConnection(_srcConnectionStr, ref sourceErrorMsg);
                    bool destConnection = testConnection(_destConnectionStr, ref destErrorMsg);


                    string sourceStatus = sourceConnection ? "PASS" : $"FAIL, Error : {sourceErrorMsg}";
                    string destStatus = destConnection ? "PASS" : $"FAIL, Error : {destErrorMsg}";

                    string msg = $"Source Connection : {sourceStatus} {Environment.NewLine} Destination Connection : {destStatus}";

                    MessageBox.Show(msg, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    MessageBox.Show("Fill all the fields");
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

        }

        private void _resetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                _sourceServerTxt.Text = string.Empty;
                _sourceUserNameTxt.Text = string.Empty;
                _sourcePwdTxt.Text = string.Empty;
                _sourceDatabaseTxt.Text = string.Empty;

                _destServerTxt.Text = string.Empty;
                _destUserNameTxt.Text = string.Empty;
                _destPwdTxt.Text = string.Empty;
                _destDatabaseTxt.Text = string.Empty;

                _sysTableProgressGrid.Rows.Clear();
                _tenantTableProgressGrid.Rows.Clear();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

        }

        private void _moveLoanType_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show($" Select YES to overwrite exisiting LoanType.{Environment.NewLine} Select NO to create new LoanType.", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    _plsWaitLabel.Visible = true;

                    if (MigrateLoanType.OverWriteLoanType)
                        MessageBox.Show("Overwrite Successfully Completed");
                    else
                        MessageBox.Show("Overwrite Failed");

                    _plsWaitLabel.Visible = false;
                }
                else if (result == DialogResult.No)
                    if (MigrateLoanType.CreateNewLoanType)
                        MessageBox.Show("Overwrite Successfully Completed");
                    else
                        MessageBox.Show("Overwrite Failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed");
                MTSExceptionHandler.HandleException(ref ex);
            }
          
        }


        //private void _overWriteLoanType()
        //{
        //    _plsWaitLabel.Visible = true;

        //    Int64 sLoanTypeID = Convert.ToInt64(_sourceLoanTypes.SelectedValue);

        //    DataTable dtLoanTypeDocs = MigrateTables.GetSourceDBData(_getLoanTypeDocs.Replace("{LOANTYPEID}", Convert.ToString(sLoanTypeID)));
        //    DataTable dtDestDocs = MigrateTables.GetDestinationDBData(_getDocMaster);

        //    List<string> docName = dtDestDocs.AsEnumerable().Select(s => s["Name"].ToString().ToLower().Trim()).ToList<string>();

        //    List<DifferentDocument> diffDocs = dtLoanTypeDocs.AsEnumerable().Where(d => !docName.Contains(d["Name"].ToString().ToLower().Trim()))
        //        .Select(a => new DifferentDocument
        //        {
        //            DocumentName = Convert.ToString(a["Name"]),
        //            DestinationDocumentID = 0,
        //            SourceDocumentID = Convert.ToInt64(a["DocumentTypeID"])
        //        }).ToList();

        //    List<CommonDocument> commonDocs = (from s in dtLoanTypeDocs.AsEnumerable()
        //                                       join d in dtDestDocs.AsEnumerable() on s["Name"] equals d["Name"]
        //                                       select new CommonDocument
        //                                       {
        //                                           DocumentName = Convert.ToString(d["Name"]),
        //                                           DestinationDocumentID = Convert.ToInt64(d["DocumentTypeID"]),
        //                                           SourceDocumentID = Convert.ToInt64(s["DocumentTypeID"])
        //                                       }).ToList();


        //    DataTable dtDestLoanType = MigrateTables.GetDestinationDBData(_getLoanType.Replace("{LoanTypeName}", _sourceLoanTypes.Text));

        //    Int64 LoanTypeID = Convert.ToInt64(dtDestLoanType.Rows[0]["LoanTypeID"]);

        //    if (LoanTypeID > 0)
        //    {



        //        #region Loan Document Mapping 

        //        foreach (DifferentDocument dDocs in diffDocs)
        //        {
        //            string InsertDocMasterSQL = _insertDocMaster.Replace("{Name}", dDocs.DocumentName.Replace("'", "''")).Replace("{DisplayName}", dDocs.DocumentName.Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(InsertDocMasterSQL);

        //            DataSet dsSrcDocTables = MigrateTables.GetSourceDBDataSet(_getDocTableMaster.Replace("{DocumentTypeID}", dDocs.SourceDocumentID.ToString()));

        //            DataTable dtDocTable = MigrateTables.GetDestinationDBData(_getDocumentType.Replace("{Name}", dDocs.DocumentName));

        //            Int64 DocumentTypeID = Convert.ToInt64(dtDocTable.Rows[0]["DocumentTypeID"]);

        //            if (dsSrcDocTables.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dsSrcDocTables.Tables[0].Rows)
        //                {
        //                    string fieldSQL = _insertDocFieldMaster.Replace("{DocumentTypeID}", DocumentTypeID.ToString()).Replace("{Name}", dr["Name"].ToString().Replace("'", "''")).Replace("{DisplayName}", dr["DisplayName"].ToString().Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{DocOrderByField}", dr["DocOrderByField"].ToString());

        //                    MigrateTables.NonQueryDestinationDB(fieldSQL);
        //                }

        //            }

        //            if (dsSrcDocTables.Tables[1].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dsSrcDocTables.Tables[1].Rows)
        //                {
        //                    string dTableSQL = _insertDocTableMaster.Replace("{DocumentTypeID}", DocumentTypeID.ToString()).Replace("{TableName}", dr["TableName"].ToString().Replace("'", "''")).Replace("{TableJson}", dr["TableJson"].ToString().Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                    MigrateTables.NonQueryDestinationDB(dTableSQL);
        //                }
        //            }

        //            commonDocs.Add(new CommonDocument() { SourceDocumentID = dDocs.SourceDocumentID, DocumentName = dDocs.DocumentName, DestinationDocumentID = DocumentTypeID });
        //        }

        //        #endregion

        //        #region CheckList Insert

        //        DataTable sChecklist = MigrateTables.GetSourceDBData(_getLoanCheckList.Replace("{LoanTypeID}", sLoanTypeID.ToString()));

        //        Int64 sChecklistID = sChecklist.Rows.Count > 0 ? Convert.ToInt64(sChecklist.Rows[0]["CheckListID"]) : 0;
        //        string sChecklistName = sChecklist.Rows.Count > 0 ? Convert.ToString(sChecklist.Rows[0]["CheckListName"]) : string.Empty;

        //        if (sChecklistID > 0)
        //        {
        //            #region Update Checklist Master

        //            DataTable dRemoveChecklist = MigrateTables.GetDestinationDBData(_getLoanCheckList.Replace("{LoanTypeID}", LoanTypeID.ToString()));

        //            Int64 dChecklistID = dRemoveChecklist.Rows.Count > 0 ? Convert.ToInt64(dRemoveChecklist.Rows[0]["CheckListID"]) : 0;
        //            string dRemoveChecklistName = dRemoveChecklist.Rows.Count > 0 ? Convert.ToString(dRemoveChecklist.Rows[0]["CheckListName"]) : string.Empty;

        //            string updateCheckListMasterSQL = _updateCheckListMaster.Replace("{CheckListID}", dChecklistID.ToString()).Replace("{CheckListName}", sChecklistName.ToString());

        //            MigrateTables.NonQueryDestinationDB(updateCheckListMasterSQL);

        //            #endregion

        //            if (dChecklistID > 0)
        //            {

        //                #region Remove Existing Checklist and StackingOrder

        //                DataTable dChecklistDetailMaster = MigrateTables.GetDestinationDBData(_getLoanCheckListDetailMaster.Replace("{CheckListID}", dChecklistID.ToString()));

        //                foreach (DataRow drCD in dChecklistDetailMaster.Rows)
        //                {
        //                    Int64 dChecklistDetailID = Convert.ToInt64(drCD["CheckListDetailID"]);
        //                    DataTable dRuleMaster = MigrateTables.GetDestinationDBData(_getRuleMaster.Replace("{CheckListDetailID}", dChecklistDetailID.ToString()));

        //                    if (dRuleMaster.Rows.Count > 0)
        //                    {
        //                        string deleteRuleMasterSQL = _deleteRuleMaster.Replace("{CheckListDetailID}", dChecklistDetailID.ToString());

        //                        MigrateTables.NonQueryDestinationDB(deleteRuleMasterSQL);
        //                    }

        //                    string deleteCheckListMasterSQL = _deleteCheckListMaster.Replace("{CheckListDetailID}", dChecklistDetailID.ToString());

        //                    MigrateTables.NonQueryDestinationDB(deleteCheckListMasterSQL);
        //                }

        //                #endregion

        //                DataTable sChecklistDetailMaster = MigrateTables.GetSourceDBData(_getLoanCheckListDetailMaster.Replace("{CheckListID}", sChecklistID.ToString()));

        //                foreach (DataRow drCD in sChecklistDetailMaster.Rows)
        //                {
        //                    Int64 sChecklistDetailID = Convert.ToInt64(drCD["CheckListDetailID"]);
        //                    string sDescription = Convert.ToString(drCD["Description"]);
        //                    string sName = Convert.ToString(drCD["Name"]);
        //                    Int64 sUserID = Convert.ToInt64(drCD["UserID"]);
        //                    Int32 sRule_Type = Convert.ToInt32(drCD["Rule_Type"]);
        //                    Int64 sSequenceID = Convert.ToInt64(drCD["SequenceID"]);

        //                    string insertChecklistDetailSQL = _insertCheckListDetailMaster.Replace("{CheckListID}", dChecklistID.ToString()).Replace("{Description}", sDescription.ToString().Replace("'", "''")).Replace("{Name}", sName.ToString().Replace("'", "''")).Replace("{UserID}", 1.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{Rule_Type}", sRule_Type.ToString()).Replace("{SequenceID}", sSequenceID.ToString());

        //                    MigrateTables.NonQueryDestinationDB(insertChecklistDetailSQL);

        //                    DataTable dChecklistDetail = MigrateTables.GetDestinationDBData(_getLoanCheckListDetail.Replace("{Description}", sDescription).Replace("{CheckListID}", dChecklistID.ToString()));

        //                    Int64 dCheckListDetailID = dChecklistDetail.Rows.Count > 0 ? Convert.ToInt64(dChecklistDetail.Rows[0]["CheckListDetailID"]) : 0;

        //                    if (dCheckListDetailID > 0)
        //                    {
        //                        DataTable sRuleMaster = MigrateTables.GetSourceDBData(_getRuleMaster.Replace("{CheckListDetailID}", sChecklistDetailID.ToString()));

        //                        if (sRuleMaster.Rows.Count > 0)
        //                        {

        //                            string sRuleDescription = Convert.ToString(sRuleMaster.Rows[0]["RuleDescription"]).Replace("'", "''");
        //                            string sRuleJson = Convert.ToString(sRuleMaster.Rows[0]["RuleJson"]).Replace("'", "''");
        //                            string sDocumentType = Convert.ToString(sRuleMaster.Rows[0]["DocumentType"]);
        //                            string sActiveDocumentType = Convert.ToString(sRuleMaster.Rows[0]["ActiveDocumentType"]);

        //                            string insertRuleMasterSQL = _insertRuleMaster.Replace("{CheckListDetailID}", dCheckListDetailID.ToString()).Replace("{RuleDescription}", sRuleDescription.ToString()).Replace("{RuleJson}", sRuleJson.ToString()).Replace("{DocumentType}", string.Empty).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{ActiveDocumentType}", string.Empty);

        //                            MigrateTables.NonQueryDestinationDB(insertRuleMasterSQL);
        //                        }
        //                    }
        //                }

        //                string insertCustReviewLoanCheckMappingSQL = _insertCustReviewLoanCheckMapping.Replace("{CheckListID}", dChecklistID.ToString()).Replace("{LoanTypeID}", LoanTypeID.ToString()).Replace("{CustomerID}", 1.ToString()).Replace("{ReviewTypeID}", 0.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                MigrateTables.NonQueryDestinationDB(insertCustReviewLoanCheckMappingSQL);
        //            }
        //        }

        //        #endregion

        //        #region Stacking Order Insert

        //        DataTable sStackingOrder = MigrateTables.GetSourceDBData(_getLoanStackingOrder.Replace("{LoanTypeID}", sLoanTypeID.ToString()));

        //        Int64 sStackingOrderID = sStackingOrder.Rows.Count > 0 ? Convert.ToInt64(sStackingOrder.Rows[0]["StackingOrderID"]) : 0;
        //        string ssDescription = sStackingOrder.Rows.Count > 0 ? Convert.ToString(sStackingOrder.Rows[0]["Description"]) : string.Empty;

        //        if (sStackingOrderID > 0)
        //        {
        //            DataTable sStackingOrderDetail = MigrateTables.GetSourceDBData(_getStackingOrderDetail.Replace("{StackingOrderID}", sStackingOrderID.ToString()));

        //            string insertStackingOrderSQL = _insertStackingOrder.Replace("{Description}", ssDescription.Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(insertStackingOrderSQL);

        //            DataTable dStackingOrder = MigrateTables.GetDestinationDBData(_getLoanStackingOrderWithDesc);

        //            Int64 dStackingOrderID = dStackingOrder.Rows.Count > 0 ? Convert.ToInt64(dStackingOrder.Rows[0]["StackingOrderID"]) : 0;

        //            if (dStackingOrderID > 0)
        //            {

        //                DataTable dStackDetail = new DataTable();

        //                dStackDetail.Columns.Add(new DataColumn("StackingOrderID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("DocumentTypeID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("SequenceID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("Active", typeof(bool)));
        //                dStackDetail.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
        //                dStackDetail.Columns.Add(new DataColumn("ModifiedOn", typeof(DateTime)));

        //                foreach (DataRow drStack in sStackingOrderDetail.Rows)
        //                {
        //                    CommonDocument doc = commonDocs.Where(c => c.SourceDocumentID == Convert.ToInt64(drStack["DocumentTypeID"])).FirstOrDefault();

        //                    if (doc != null)
        //                    {
        //                        DataRow dr = dStackDetail.NewRow();
        //                        Int64 DocumentTypeID = doc.DestinationDocumentID;
        //                        dr["StackingOrderID"] = dStackingOrderID;
        //                        dr["DocumentTypeID"] = DocumentTypeID;
        //                        dr["SequenceID"] = Convert.ToInt64(drStack["SequenceID"]);
        //                        dr["Active"] = true;
        //                        dr["CreatedOn"] = DateTime.Now;
        //                        dr["ModifiedOn"] = DateTime.Now;
        //                        dStackDetail.Rows.Add(dr);
        //                    }
        //                }

        //                if (dStackDetail.Rows.Count > 0)
        //                    MigrateTables.DestinationBulkInsert("[IL].[StackingOrderDetailMasters]", dStackDetail);


        //                string insertCustReviewLoanStackMappingSQL = _insertCustReviewLoanStackMapping.Replace("{StackingOrderID}", dStackingOrderID.ToString()).Replace("{LoanTypeID}", LoanTypeID.ToString()).Replace("{CustomerID}", 1.ToString()).Replace("{ReviewTypeID}", 0.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                MigrateTables.NonQueryDestinationDB(insertCustReviewLoanStackMappingSQL);
        //            }
        //        }


        //        #endregion

        //        _plsWaitLabel.Visible = false;
        //    }
        //    else
        //    {
        //        _createNewLoanType();
        //    }
        //}

        //private void _createNewLoanType()
        //{
        //    _plsWaitLabel.Visible = true;

        //    Int64 sLoanTypeID = Convert.ToInt64(_sourceLoanTypes.SelectedValue);

        //    DataTable dtLoanTypeDocs = MigrateTables.GetSourceDBData(_getLoanTypeDocs.Replace("{LOANTYPEID}", Convert.ToString(sLoanTypeID)));
        //    DataTable dtDestDocs = MigrateTables.GetDestinationDBData(_getDocMaster);

        //    List<string> docName = dtDestDocs.AsEnumerable().Select(s => s["Name"].ToString().ToLower().Trim()).ToList<string>();

        //    List<DifferentDocument> diffDocs = dtLoanTypeDocs.AsEnumerable().Where(d => !docName.Contains(d["Name"].ToString().ToLower().Trim()))
        //        .Select(a => new DifferentDocument
        //        {
        //            DocumentName = Convert.ToString(a["Name"]),
        //            DestinationDocumentID = 0,
        //            SourceDocumentID = Convert.ToInt64(a["DocumentTypeID"])
        //        }).ToList();

        //    List<CommonDocument> commonDocs = (from s in dtLoanTypeDocs.AsEnumerable()
        //                                       join d in dtDestDocs.AsEnumerable() on s["Name"] equals d["Name"]
        //                                       select new CommonDocument
        //                                       {
        //                                           DocumentName = Convert.ToString(d["Name"]),
        //                                           DestinationDocumentID = Convert.ToInt64(d["DocumentTypeID"]),
        //                                           SourceDocumentID = Convert.ToInt64(s["DocumentTypeID"])
        //                                       }).ToList();


        //    DataTable dtDestLoanType = MigrateTables.GetDestinationDBData(_getLoanType.Replace("{LoanTypeName}", _sourceLoanTypes.Text));

        //    string loanTypeName = dtDestLoanType.Rows.Count == 0 ? $"{_sourceLoanTypes.Text}" : dtDestLoanType.Rows.Count == 1 ? $"{_sourceLoanTypes.Text}_New" : $"{_sourceLoanTypes.Text}_New_{dtDestLoanType.Rows.Count.ToString()}";

        //    string insertLoanTypeSQL = _insertLoanTypeMaster.Replace("{LoanTypeName}", loanTypeName.Replace("'", "''")).Replace("{Active}", 1.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //    MigrateTables.NonQueryDestinationDB(insertLoanTypeSQL);

        //    dtDestLoanType = MigrateTables.GetDestinationDBData(_getLoanType.Replace("{LoanTypeName}", loanTypeName));

        //    Int64 LoanTypeID = Convert.ToInt64(dtDestLoanType.Rows[0]["LoanTypeID"]);

        //    if (LoanTypeID > 0)
        //    {
        //        #region Loan Document Mapping 

        //        foreach (DifferentDocument dDocs in diffDocs)
        //        {
        //            string InsertDocMasterSQL = _insertDocMaster.Replace("{Name}", dDocs.DocumentName.Replace("'", "''")).Replace("{DisplayName}", dDocs.DocumentName.Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(InsertDocMasterSQL);

        //            DataSet dsSrcDocTables = MigrateTables.GetSourceDBDataSet(_getDocTableMaster.Replace("{DocumentTypeID}", dDocs.SourceDocumentID.ToString()));

        //            DataTable dtDocTable = MigrateTables.GetDestinationDBData(_getDocumentType.Replace("{Name}", dDocs.DocumentName));

        //            Int64 DocumentTypeID = Convert.ToInt64(dtDocTable.Rows[0]["DocumentTypeID"]);

        //            if (dsSrcDocTables.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dsSrcDocTables.Tables[0].Rows)
        //                {
        //                    string fieldSQL = _insertDocFieldMaster.Replace("{DocumentTypeID}", DocumentTypeID.ToString()).Replace("{Name}", dr["Name"].ToString().Replace("'", "''")).Replace("{DisplayName}", dr["DisplayName"].ToString().Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{DocOrderByField}", dr["DocOrderByField"].ToString());

        //                    MigrateTables.NonQueryDestinationDB(fieldSQL);
        //                }

        //            }

        //            if (dsSrcDocTables.Tables[1].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dsSrcDocTables.Tables[1].Rows)
        //                {
        //                    string dTableSQL = _insertDocTableMaster.Replace("{DocumentTypeID}", DocumentTypeID.ToString()).Replace("{TableName}", dr["TableName"].ToString().Replace("'", "''")).Replace("{TableJson}", dr["TableJson"].ToString().Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                    MigrateTables.NonQueryDestinationDB(dTableSQL);
        //                }
        //            }

        //            commonDocs.Add(new CommonDocument() { SourceDocumentID = dDocs.SourceDocumentID, DocumentName = dDocs.DocumentName, DestinationDocumentID = DocumentTypeID });
        //        }


        //        foreach (CommonDocument cDocs in commonDocs)
        //        {
        //            string insertCustLoanDocMappingSQL = _insertCustLoanDocMapping.Replace("{CustomerID}", 1.ToString()).Replace("{LoanTypeID}", LoanTypeID.ToString()).Replace("{DocumentTypeID}", cDocs.DestinationDocumentID.ToString()).Replace("{Active}", 1.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(insertCustLoanDocMappingSQL);
        //        }

        //        #endregion

        //        #region CheckList Insert

        //        DataTable sChecklist = MigrateTables.GetSourceDBData(_getLoanCheckList.Replace("{LoanTypeID}", sLoanTypeID.ToString()));

        //        Int64 sChecklistID = sChecklist.Rows.Count > 0 ? Convert.ToInt64(sChecklist.Rows[0]["CheckListID"]) : 0;
        //        string sChecklistName = sChecklist.Rows.Count > 0 ? Convert.ToString(sChecklist.Rows[0]["CheckListName"]) : string.Empty;

        //        if (sChecklistID > 0)
        //        {
        //            string insertCheckListMasterSQL = _insertCheckListMaster.Replace("{CheckListName}", sChecklistName.Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(insertCheckListMasterSQL);

        //            DataTable dChecklist = MigrateTables.GetDestinationDBData(_getLoanCheckListMasterWithName);

        //            Int64 dChecklistID = dChecklist.Rows.Count > 0 ? Convert.ToInt64(dChecklist.Rows[0]["CheckListID"]) : 0;

        //            if (dChecklistID > 0)
        //            {
        //                DataTable sChecklistDetailMaster = MigrateTables.GetSourceDBData(_getLoanCheckListDetailMaster.Replace("{CheckListID}", sChecklistID.ToString()));

        //                foreach (DataRow drCD in sChecklistDetailMaster.Rows)
        //                {
        //                    Int64 sChecklistDetailID = Convert.ToInt64(drCD["CheckListDetailID"]);
        //                    string sDescription = Convert.ToString(drCD["Description"]);
        //                    string sName = Convert.ToString(drCD["Name"]);
        //                    Int64 sUserID = Convert.ToInt64(drCD["UserID"]);
        //                    Int32 sRule_Type = Convert.ToInt32(drCD["Rule_Type"]);
        //                    Int64 sSequenceID = Convert.ToInt64(drCD["SequenceID"]);

        //                    string insertChecklistDetailSQL = _insertCheckListDetailMaster.Replace("{CheckListID}", dChecklistID.ToString()).Replace("{Description}", sDescription.ToString().Replace("'", "''")).Replace("{Name}", sName.ToString().Replace("'", "''")).Replace("{UserID}", 1.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{Rule_Type}", sRule_Type.ToString()).Replace("{SequenceID}", sSequenceID.ToString());

        //                    MigrateTables.NonQueryDestinationDB(insertChecklistDetailSQL);

        //                    DataTable dChecklistDetail = MigrateTables.GetDestinationDBData(_getLoanCheckListDetail.Replace("{Description}", sDescription).Replace("{CheckListID}", dChecklistID.ToString()));

        //                    Int64 dCheckListDetailID = dChecklistDetail.Rows.Count > 0 ? Convert.ToInt64(dChecklistDetail.Rows[0]["CheckListDetailID"]) : 0;

        //                    if (dCheckListDetailID > 0)
        //                    {
        //                        DataTable sRuleMaster = MigrateTables.GetSourceDBData(_getRuleMaster.Replace("{CheckListDetailID}", sChecklistDetailID.ToString()));

        //                        if (sRuleMaster.Rows.Count > 0)
        //                        {

        //                            string sRuleDescription = Convert.ToString(sRuleMaster.Rows[0]["RuleDescription"]).Replace("'", "''");
        //                            string sRuleJson = Convert.ToString(sRuleMaster.Rows[0]["RuleJson"]).Replace("'", "''");
        //                            string sDocumentType = Convert.ToString(sRuleMaster.Rows[0]["DocumentType"]);
        //                            string sActiveDocumentType = Convert.ToString(sRuleMaster.Rows[0]["ActiveDocumentType"]);

        //                            string insertRuleMasterSQL = _insertRuleMaster.Replace("{CheckListDetailID}", dCheckListDetailID.ToString()).Replace("{RuleDescription}", sRuleDescription.ToString()).Replace("{RuleJson}", sRuleJson.ToString()).Replace("{DocumentType}", string.Empty).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString()).Replace("{ActiveDocumentType}", string.Empty);

        //                            MigrateTables.NonQueryDestinationDB(insertRuleMasterSQL);
        //                        }
        //                    }
        //                }

        //                string insertCustReviewLoanCheckMappingSQL = _insertCustReviewLoanCheckMapping.Replace("{CheckListID}", dChecklistID.ToString()).Replace("{LoanTypeID}", LoanTypeID.ToString()).Replace("{CustomerID}", 1.ToString()).Replace("{ReviewTypeID}", 0.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                MigrateTables.NonQueryDestinationDB(insertCustReviewLoanCheckMappingSQL);
        //            }
        //        }

        //        #endregion

        //        #region Stacking Order Insert

        //        DataTable sStackingOrder = MigrateTables.GetSourceDBData(_getLoanStackingOrder.Replace("{LoanTypeID}", sLoanTypeID.ToString()));

        //        Int64 sStackingOrderID = sStackingOrder.Rows.Count > 0 ? Convert.ToInt64(sStackingOrder.Rows[0]["StackingOrderID"]) : 0;
        //        string ssDescription = sStackingOrder.Rows.Count > 0 ? Convert.ToString(sStackingOrder.Rows[0]["Description"]) : string.Empty;

        //        if (sStackingOrderID > 0)
        //        {
        //            DataTable sStackingOrderDetail = MigrateTables.GetSourceDBData(_getStackingOrderDetail.Replace("{StackingOrderID}", sStackingOrderID.ToString()));

        //            string insertStackingOrderSQL = _insertStackingOrder.Replace("{Description}", ssDescription.Replace("'", "''")).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //            MigrateTables.NonQueryDestinationDB(insertStackingOrderSQL);

        //            DataTable dStackingOrder = MigrateTables.GetDestinationDBData(_getLoanStackingOrderWithDesc);

        //            Int64 dStackingOrderID = dStackingOrder.Rows.Count > 0 ? Convert.ToInt64(dStackingOrder.Rows[0]["StackingOrderID"]) : 0;

        //            if (dStackingOrderID > 0)
        //            {

        //                DataTable dStackDetail = new DataTable();

        //                dStackDetail.Columns.Add(new DataColumn("StackingOrderID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("DocumentTypeID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("SequenceID", typeof(Int64)));
        //                dStackDetail.Columns.Add(new DataColumn("Active", typeof(bool)));
        //                dStackDetail.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
        //                dStackDetail.Columns.Add(new DataColumn("ModifiedOn", typeof(DateTime)));

        //                foreach (DataRow drStack in sStackingOrderDetail.Rows)
        //                {
        //                    CommonDocument doc = commonDocs.Where(c => c.SourceDocumentID == Convert.ToInt64(drStack["DocumentTypeID"])).FirstOrDefault();

        //                    if (doc != null)
        //                    {
        //                        DataRow dr = dStackDetail.NewRow();
        //                        Int64 DocumentTypeID = doc.DestinationDocumentID;
        //                        dr["StackingOrderID"] = dStackingOrderID;
        //                        dr["DocumentTypeID"] = DocumentTypeID;
        //                        dr["SequenceID"] = Convert.ToInt64(drStack["SequenceID"]);
        //                        dr["Active"] = true;
        //                        dr["CreatedOn"] = DateTime.Now;
        //                        dr["ModifiedOn"] = DateTime.Now;
        //                        dStackDetail.Rows.Add(dr);
        //                    }
        //                }

        //                if (dStackDetail.Rows.Count > 0)
        //                    MigrateTables.DestinationBulkInsert("[IL].[StackingOrderDetailMasters]", dStackDetail);


        //                string insertCustReviewLoanStackMappingSQL = _insertCustReviewLoanStackMapping.Replace("{StackingOrderID}", dStackingOrderID.ToString()).Replace("{LoanTypeID}", LoanTypeID.ToString()).Replace("{CustomerID}", 1.ToString()).Replace("{ReviewTypeID}", 0.ToString()).Replace("{CreatedOn}", DateTime.Now.ToString()).Replace("{ModifiedOn}", DateTime.Now.ToString());

        //                MigrateTables.NonQueryDestinationDB(insertCustReviewLoanStackMappingSQL);
        //            }
        //        }


        //        #endregion

        //        _plsWaitLabel.Visible = false;
        //    }
        //}
    }
}
