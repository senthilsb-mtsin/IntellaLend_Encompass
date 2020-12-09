using IntellaLend.Model;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MTSEntityDataAccess
{
    //public class DBConnect : DbContext
    //{
    //    public static string TableSchema { get; set; }
    //    public const string MTSSchema = "IL";

    //   // public DBConnect() { }

    //    public DBConnect(string tableSchema)
    //    {
    //        TableSchema = tableSchema;
    //    }

    //    #region User Tables

    //    public DbSet<User> Users { get; set; }
    //    public DbSet<UserAddressDetail> UserAddressDetail { get; set; }
    //    public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
    //    public DbSet<CustomAddressDetail> CustomAddressDetail { get; set; }
    //    public DbSet<UserSecurityQuestion> UserSecurityQuestion { get; set; }

    //    #endregion

    //    #region Role Tables

    //    public DbSet<RoleMaster> Roles { get; set; }
    //    public DbSet<MenuMaster> Menus { get; set; }
    //    public DbSet<AccessURL> AccessURLs { get; set; }
    //    public DbSet<RoleMenuMapping> RoleMenuMapping { get; set; }

    //    #endregion

    //    #region Loan Tables

    //    public DbSet<Loan> Loan { get; set; }
    //    public DbSet<LoanDetail> LoanDetail { get; set; }
    //    public DbSet<LoanImage> LoanImage { get; set; }
    //    public DbSet<LoanSearch> LoanSearch { get; set; }

    //    #endregion

    //    #region Customer Master Tables

    //    public DbSet<CustomerMaster> CustomerMaster { get; set; }
    //    public DbSet<ReviewTypeMaster> ReviewTypeMaster { get; set; }
    //    public DbSet<LoanTypeMaster> LoanTypeMaster { get; set; }
    //    public DbSet<DocumentTypeMaster> DocumentTypeMaster { get; set; }
    //    public DbSet<DocumentFieldMaster> DocumentFieldMaster { get; set; }
    //    public DbSet<CheckListMaster> CheckListMaster { get; set; }
    //    public DbSet<CheckListDetailMaster> CheckListDetailMaster { get; set; }
    //    public DbSet<RuleMaster> RuleMaster { get; set; }
    //    public DbSet<StackingOrderMaster> StackingOrderMaster { get; set; }
    //    public DbSet<StackingOrderDetailMaster> StackingOrderDetailMaster { get; set; }

    //    #endregion

    //    #region Mapping Tables

    //    public DbSet<CustLoanMapping> CustLoanMapping { get; set; }
    //    public DbSet<CustLoanReviewMapping> CustLoanReviewMapping { get; set; }
    //    public DbSet<CustLoanDocMapping> CustLoanDocMapping { get; set; }
    //    public DbSet<CustLoanReviewCheckMapping> CustLoanReviewCheckMapping { get; set; }
    //    public DbSet<CustLoanReviewStackMapping> CustLoanReviewStackMapping { get; set; }

    //    #endregion

    //    #region IntellaLend Tables

    //    public DbSet<WorkFlowStatusMaster> WorkFlowStatusMaster { get; set; }
    //    public DbSet<TenantMaster> TenantMaster { get; set; }
    //    public DbSet<ServiceConfig> ServiceConfig { get; set; }
    //    public DbSet<EmailMaster> EmailMaster { get; set; }
    //    public DbSet<AppConfig> AppConfig { get; set; }


    //    #endregion

    //    #region Master Tables

    //    public DbSet<SecurityQuestionMasters> SecurityQuestionMasters { get; set; }

    //    #endregion

    //    #region Audit Loan Tables

    //    public DbSet<AuditLoan> AuditLoan { get; set; }
    //    public DbSet<AuditLoanDetail> AuditLoanDetail { get; set; }
    //    public DbSet<AuditLoanMissingDoc> AuditLoanMissingDoc { get; set; }
    //    public DbSet<AuditLoanSearch> AuditLoanSearch { get; set; }

    //    #endregion

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        #region User Tables

    //        modelBuilder.Entity<User>()
    //           .ToTable("Users", schema: TableSchema);

    //        modelBuilder.Entity<UserAddressDetail>()
    //           .ToTable("UserAddressDetails", schema: TableSchema);

    //        modelBuilder.Entity<UserRoleMapping>()
    //           .ToTable("UserRoleMappings", schema: TableSchema);

    //        modelBuilder.Entity<CustomAddressDetail>()
    //           .ToTable("CustomAddressDetails", schema: TableSchema);

    //        modelBuilder.Entity<UserSecurityQuestion>()
    //           .ToTable("UserSecurityQuestion", schema: TableSchema);

    //        #endregion

    //        #region Role Tables

    //        modelBuilder.Entity<RoleMaster>()
    //           .ToTable("RoleMasters", schema: TableSchema);

    //        modelBuilder.Entity<MenuMaster>()
    //           .ToTable("MenuMasters", schema: TableSchema);

    //        modelBuilder.Entity<AccessURL>()
    //           .ToTable("AccessURLs", schema: TableSchema);

    //        modelBuilder.Entity<RoleMenuMapping>()
    //           .ToTable("RoleMenuMappings", schema: TableSchema);

    //        #endregion

    //        #region Loan Tables

    //        modelBuilder.Entity<Loan>()
    //           .ToTable("Loans", schema: TableSchema);

    //        modelBuilder.Entity<LoanDetail>()
    //           .ToTable("LoanDetails", schema: TableSchema);

    //        modelBuilder.Entity<LoanImage>()
    //           .ToTable("LoanImages", schema: TableSchema);

    //        modelBuilder.Entity<LoanSearch>()
    //           .ToTable("LoanSearch", schema: TableSchema);

    //        #endregion

    //        #region Customer Master Tables

    //        modelBuilder.Entity<CustomerMaster>()
    //           .ToTable("CustomerMasters", schema: TableSchema);

    //        modelBuilder.Entity<ReviewTypeMaster>()
    //           .ToTable("ReviewTypeMasters", schema: TableSchema);

    //        modelBuilder.Entity<LoanTypeMaster>()
    //           .ToTable("LoanTypeMasters", schema: TableSchema);

    //        modelBuilder.Entity<DocumentTypeMaster>()
    //           .ToTable("DocumentTypeMasters", schema: TableSchema);

    //        modelBuilder.Entity<DocumentFieldMaster>()
    //           .ToTable("DocumentFieldMasters", schema: TableSchema);

    //        modelBuilder.Entity<CheckListMaster>()
    //           .ToTable("CheckListMasters", schema: TableSchema);

    //        modelBuilder.Entity<CheckListDetailMaster>()
    //           .ToTable("CheckListDetailMasters", schema: TableSchema);


    //        modelBuilder.Entity<RuleMaster>()
    //           .ToTable("RuleMasters", schema: TableSchema);

    //        modelBuilder.Entity<StackingOrderMaster>()
    //           .ToTable("StackingOrderMasters", schema: TableSchema);

    //        modelBuilder.Entity<StackingOrderDetailMaster>()
    //           .ToTable("StackingOrderDetailMasters", schema: TableSchema);

    //        #endregion

    //        #region Mapping Tables

    //        modelBuilder.Entity<CustLoanMapping>()
    //           .ToTable("CustLoanMapping", schema: TableSchema);

    //        modelBuilder.Entity<CustLoanReviewMapping>()
    //           .ToTable("CustLoanReviewMapping", schema: TableSchema);

    //        modelBuilder.Entity<CustLoanDocMapping>()
    //           .ToTable("CustLoanDocMapping", schema: TableSchema);

    //        modelBuilder.Entity<CustLoanReviewCheckMapping>()
    //           .ToTable("CustLoanReviewCheckMapping", schema: TableSchema);

    //        modelBuilder.Entity<CustLoanReviewStackMapping>()
    //           .ToTable("CustLoanReviewStackMapping", schema: TableSchema);

    //        #endregion

    //        #region IntellaLend Tables

    //        modelBuilder.Entity<WorkFlowStatusMaster>()
    //               .ToTable("WorkFlowStatusMaster", schema: MTSSchema);

    //        modelBuilder.Entity<TenantMaster>()
    //           .ToTable("TenantMasters", schema: MTSSchema);

    //        modelBuilder.Entity<ServiceConfig>()
    //           .ToTable("ServiceConfigs", schema: MTSSchema);

    //        modelBuilder.Entity<EmailMaster>()
    //           .ToTable("EmailMaster", schema: MTSSchema);

    //        modelBuilder.Entity<AppConfig>()
    //           .ToTable("AppConfig", schema: MTSSchema);

    //        #endregion

    //        #region Master Tables

    //        modelBuilder.Entity<SecurityQuestionMasters>()
    //           .ToTable("SecurityQuestionMasters", schema: TableSchema);

    //        #endregion

    //        #region Audit Loan Tables

    //        modelBuilder.Entity<AuditLoan>()
    //                .ToTable("AuditLoan", schema: TableSchema);

    //        modelBuilder.Entity<AuditLoanDetail>()
    //                .ToTable("AuditLoanDetails", schema: TableSchema);

    //        modelBuilder.Entity<AuditLoanMissingDoc>()
    //                .ToTable("AuditLoanMissingDoc", schema: TableSchema);

    //        modelBuilder.Entity<AuditLoanSearch>()
    //                .ToTable("AuditLoanSearch", schema: TableSchema);

    //        #endregion
    //    }

    //}


    public class DBConnect : DbContext, IDbModelCacheKeyProvider
    {
        #region Constructor

        public string TenantSchema { get; private set; }
        public string SystemSchema { get; private set; }


        public DBConnect()
        {
            Database.SetInitializer<DBConnect>(null);
        }

        public DBConnect(string TenantSchema)
            : base(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString)
        {
            Int32 _timeOut = 1200;
            Int32.TryParse(ConfigurationManager.AppSettings["DBConnectionTimeOut"], out _timeOut);
            Database.SetInitializer<DBConnect>(null);
            this.TenantSchema = TenantSchema;
            this.SystemSchema = "IL";
            this.Database.CommandTimeout = _timeOut;
            //  this.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            //  DbInterception.Add(new NoLockInterceptor());
        }

        //public DbSet<Entity> Entities { get; set; }

        //public override DbSet<TEntity> Set<TEntity>()
        //{
        //    return new DbSetProxy<TEntity>(base.Set<TEntity>());
        //}

        #endregion

        #region DataSet Properties

        #region User Tables

        public DbSet<User> Users { get { return this.Set<User>(); } }
        public DbSet<UserAddressDetail> UserAddressDetail { get { return this.Set<UserAddressDetail>(); } }
        public DbSet<UserRoleMapping> UserRoleMapping { get { return this.Set<UserRoleMapping>(); } }
        public DbSet<CustomAddressDetail> CustomAddressDetail { get { return this.Set<CustomAddressDetail>(); } }
        public DbSet<UserSecurityQuestion> UserSecurityQuestion { get { return this.Set<UserSecurityQuestion>(); } }
        public DbSet<UserSession> UserSession { get { return this.Set<UserSession>(); } }
        public DbSet<KPIGoalConfig> KPIGoalConfig { get { return this.Set<KPIGoalConfig>(); } }
        public DbSet<AuditUserRequest> AuditUserRequest { get { return this.Set<AuditUserRequest>(); } }
        public DbSet<KpiUserGroupConfig> KpiUserGroupConfig { get { return this.Set<KpiUserGroupConfig>(); } }
        public DbSet<AuditKpiGoalConfig> AuditKpiGoalConfig { get { return this.Set<AuditKpiGoalConfig>(); } }
        public DbSet<AuditUserKpiGoalConfig> AuditUserKpiGoalConfig { get { return this.Set<AuditUserKpiGoalConfig>(); } }
        public DbSet<KPIConfigStaging> KPIConfigStaging { get { return this.Set<KPIConfigStaging>(); } }
        public DbSet<UserPassword> UserPassword { get { return this.Set<UserPassword>(); } }
        #endregion

        #region Report Tables
        public DbSet<ReportMaster> ReportMaster { get { return this.Set<ReportMaster>(); } }
        public DbSet<ReportConfig> ReportConfig { get { return this.Set<ReportConfig>(); } }
        #endregion

        #region Role Tables

        public DbSet<RoleMaster> Roles { get { return this.Set<RoleMaster>(); } }

        public DbSet<ADGroupMasters> ADGroupMasters { get { return this.Set<ADGroupMasters>(); } }
        public DbSet<MenuMaster> Menus { get { return this.Set<MenuMaster>(); } }
        public DbSet<AccessURL> AccessURLs { get { return this.Set<AccessURL>(); } }
        public DbSet<RoleMenuMapping> RoleMenuMapping { get { return this.Set<RoleMenuMapping>(); } }
        public DbSet<MenuGroupMaster> MenuGroupMaster { get { return this.Set<MenuGroupMaster>(); } }

        #endregion

        #region Loan Tables

        public DbSet<Loan> Loan { get { return this.Set<Loan>(); } }
        public DbSet<LoanDetail> LoanDetail { get { return this.Set<LoanDetail>(); } }
        public DbSet<LoanImage> LoanImage { get { return this.Set<LoanImage>(); } }
        public DbSet<LoanSearch> LoanSearch { get { return this.Set<LoanSearch>(); } }
        public DbSet<LoanPDF> LoanPDF { get { return this.Set<LoanPDF>(); } }
        public DbSet<PurgeStaging> PurgeStaging { get { return this.Set<PurgeStaging>(); } }
        public DbSet<PurgeStagingDetails> PurgeStagingDetails { get { return this.Set<PurgeStagingDetails>(); } }
        public DbSet<LoanReverification> LoanReverification { get { return this.Set<LoanReverification>(); } }
        public DbSet<IDCFields> IDCFields { get { return this.Set<IDCFields>(); } }
        public DbSet<EmailTracker> EmailTracker { get { return this.Set<EmailTracker>(); } }
        public DbSet<ImportStagings> ImportStaging { get { return this.Set<ImportStagings>(); } }
        public DbSet<LoanReporting> LoanReporting { get { return this.Set<LoanReporting>(); } }
        public DbSet<LoanStipulation> LoanStipulation { get { return this.Set<LoanStipulation>(); } }
        public DbSet<LoanLOSFields> LoanLOSFields { get { return this.Set<LoanLOSFields>(); } }
        public DbSet<EncompassDownloadExceptions> EncompassDownloadExceptions { get { return this.Set<EncompassDownloadExceptions>(); } }
        public DbSet<ELoanAttachmentUpload> ELoanAttachmentUpload { get { return this.Set<ELoanAttachmentUpload>(); } }
        public DbSet<EUploadStaging> EUploadStaging { get { return this.Set<EUploadStaging>(); } }

        public DbSet<EDownloadStaging> EDownloadStaging { get { return this.Set<EDownloadStaging>(); } }
        public DbSet<LOSExportFileStaging> LOSExportFileStaging { get { return this.Set<LOSExportFileStaging>(); } }

        public DbSet<LOSExportFileStagingDetail> LOSExportFileStagingDetail { get { return this.Set<LOSExportFileStagingDetail>(); } }

        #endregion

        #region Customer Master Tables

        public DbSet<CustomerMaster> CustomerMaster { get { return this.Set<CustomerMaster>(); } }
        public DbSet<ReviewTypeMaster> ReviewTypeMaster { get { return this.Set<ReviewTypeMaster>(); } }
        public DbSet<LoanTypeMaster> LoanTypeMaster { get { return this.Set<LoanTypeMaster>(); } }
        public DbSet<ReviewPriorityMaster> ReviewPriorityMaster { get { return this.Set<ReviewPriorityMaster>(); } }
        public DbSet<SystemReviewTypeMaster> SystemReviewTypeMaster { get { return this.Set<SystemReviewTypeMaster>(); } }
        public DbSet<SystemLoanTypeMaster> SystemLoanTypeMaster { get { return this.Set<SystemLoanTypeMaster>(); } }
        public DbSet<DocumentTypeMaster> DocumentTypeMaster { get { return this.Set<DocumentTypeMaster>(); } }
        public DbSet<DocumentFieldMaster> DocumentFieldMaster { get { return this.Set<DocumentFieldMaster>(); } }
        public DbSet<CheckListMaster> CheckListMaster { get { return this.Set<CheckListMaster>(); } }
        public DbSet<CheckListDetailMaster> CheckListDetailMaster { get { return this.Set<CheckListDetailMaster>(); } }
        public DbSet<RuleDocumentTables> RuleDocumentTables { get { return this.Set<RuleDocumentTables>(); } }
        public DbSet<RuleMaster> RuleMaster { get { return this.Set<RuleMaster>(); } }
        public DbSet<StackingOrderMaster> StackingOrderMaster { get { return this.Set<StackingOrderMaster>(); } }
        public DbSet<StackingOrderDetailMaster> StackingOrderDetailMaster { get { return this.Set<StackingOrderDetailMaster>(); } }
        public DbSet<StackingOrderGroupmasters> StackingOrderGroupmasters { get { return this.Set<StackingOrderGroupmasters>(); } }
        public DbSet<CustomerConfig> CustomerConfig { get { return this.Set<CustomerConfig>(); } }
        public DbSet<QCIQConnectionString> QCIQConnectionString { get { return this.Set<QCIQConnectionString>(); } }
        public DbSet<DocumetTypeTables> DocumetTypeTables { get { return this.Set<DocumetTypeTables>(); } }
        public DbSet<LoanChecklistAudit> LoanChecklistAudit { get { return this.Set<LoanChecklistAudit>(); } }
        public DbSet<CategoryLists> CategoryLists { get { return this.Set<CategoryLists>(); } }
        public DbSet<LoanEvaluatedResult> LoanEvaluatedResult { get { return this.Set<LoanEvaluatedResult>(); } }

        #endregion

        #region Mapping Tables

        public DbSet<CustReviewMapping> CustReviewMapping { get { return this.Set<CustReviewMapping>(); } }
        public DbSet<CustReviewLoanMapping> CustReviewLoanMapping { get { return this.Set<CustReviewLoanMapping>(); } }
        public DbSet<LOSImportStaging> LOSImportStaging { get { return this.Set<LOSImportStaging>(); } }
        public DbSet<LOSLoanDetails> LOSLoanDetails { get { return this.Set<LOSLoanDetails>(); } }
        public DbSet<LOSDocument> LOSDocument { get { return this.Set<LOSDocument>(); } }
        public DbSet<LOSDocumentFields> LOSDocumentFields { get { return this.Set<LOSDocumentFields>(); } }
        public DbSet<CustLoanDocMapping> CustLoanDocMapping { get { return this.Set<CustLoanDocMapping>(); } }
        public DbSet<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get { return this.Set<CustReviewLoanCheckMapping>(); } }
        public DbSet<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get { return this.Set<CustReviewLoanStackMapping>(); } }
        public DbSet<RetainUpdateStaging> RetainUpdateStaging { get { return this.Set<RetainUpdateStaging>(); } }
        public DbSet<RetainUpdateStagingDetails> RetainUpdateStagingDetails { get { return this.Set<RetainUpdateStagingDetails>(); } }
        public DbSet<CustReviewLoanUploadPath> CustReviewLoanUploadPath { get { return this.Set<CustReviewLoanUploadPath>(); } }
        #endregion

        #region IntellaLend Tables

        public DbSet<WorkFlowStatusMaster> WorkFlowStatusMaster { get { return this.Set<WorkFlowStatusMaster>(); } }
        public DbSet<TenantMaster> TenantMaster { get { return this.Set<TenantMaster>(); } }
        public DbSet<ServiceConfig> ServiceConfig { get { return this.Set<ServiceConfig>(); } }
        public DbSet<EmailMaster> EmailMaster { get { return this.Set<EmailMaster>(); } }
        public DbSet<AppConfig> AppConfig { get { return this.Set<AppConfig>(); } }
        public DbSet<RequestResponseLogging> RequestResponseLogging { get { return this.Set<RequestResponseLogging>(); } }
        public DbSet<SMTPDETAILS> SMTPDETAILS { get { return this.Set<SMTPDETAILS>(); } }
        public DbSet<DocumentTables> DocumentTables { get { return this.Set<DocumentTables>(); } }
        public DbSet<ExportProcessingQueue> ExportProcessingQueue { get { return this.Set<ExportProcessingQueue>(); } }
        public DbSet<EncompassFields> EncompassFields { get { return this.Set<EncompassFields>(); } }
        public DbSet<LOSLoanTapeFields> LOSLoanTapeFields { get { return this.Set<LOSLoanTapeFields>(); } }
        public DbSet<LoanTapeDefinition> LoanTapeDefinitions { get { return this.Set<LoanTapeDefinition>(); } }
        public DbSet<InvestorStipulation> InvestorStipulations { get { return this.Set<InvestorStipulation>(); } }
        public DbSet<IntellaAndEncompassFetchFields> IntellaAndEncompassFetchFields { get { return this.Set<IntellaAndEncompassFetchFields>(); } }
        public DbSet<EncompassParkingSpot> EncompassParkingSpot { get { return this.Set<EncompassParkingSpot>(); } }
        public DbSet<PasswordPolicy> PasswordPolicy { get { return this.Set<PasswordPolicy>(); } }
        public DbSet<ELoanAttachmentDownload> ELoanAttachmentDownload { get { return this.Set<ELoanAttachmentDownload>(); } }


        #endregion

        #region Master Tables
        public DbSet<SecurityQuestionMasters> SecurityQuestionMasters { get { return this.Set<SecurityQuestionMasters>(); } }
        public DbSet<MenuAccessUrl> MenuAccessUrl { get { return this.Set<MenuAccessUrl>(); } }
        #endregion

        #region Audit Loan Tables

        public DbSet<AuditIDCFields> AuditIDCFields { get { return this.Set<AuditIDCFields>(); } }
        public DbSet<AuditLoan> AuditLoan { get { return this.Set<AuditLoan>(); } }
        public DbSet<AuditLoanDetail> AuditLoanDetail { get { return this.Set<AuditLoanDetail>(); } }
        public DbSet<AuditLoanMissingDoc> AuditLoanMissingDoc { get { return this.Set<AuditLoanMissingDoc>(); } }
        public DbSet<AuditLoanSearch> AuditLoanSearch { get { return this.Set<AuditLoanSearch>(); } }
        public DbSet<AuditDescriptionConfig> AuditDescriptionConfig { get { return this.Set<AuditDescriptionConfig>(); } }
        public DbSet<EncompassConfig> EncompassConfig { get { return this.Set<EncompassConfig>(); } }
        public DbSet<EncompassAccessToken> EncompassAccessToken { get { return this.Set<EncompassAccessToken>(); } }

        #endregion

        #region BoxAPI

        public DbSet<BoxUserToken> BoxUserToken { get { return this.Set<BoxUserToken>(); } }
        public DbSet<BoxDownloadQueue> BoxDownloadQueue { get { return this.Set<BoxDownloadQueue>(); } }
        public DbSet<BoxDownloadException> BoxDownloadException { get { return this.Set<BoxDownloadException>(); } }
        #endregion

        #region Re-Verification

        public DbSet<ReverificationTemplate> ReverificationTemplate { get { return this.Set<ReverificationTemplate>(); } }
        public DbSet<ReverificationMaster> ReverificationMaster { get { return this.Set<ReverificationMaster>(); } }
        public DbSet<CustReverificationDocMapping> CustReverificationDocMapping { get { return this.Set<CustReverificationDocMapping>(); } }
        public DbSet<CustReviewLoanReverifyMapping> CustReviewLoanReverifyMapping { get { return this.Set<CustReviewLoanReverifyMapping>(); } }
        public DbSet<SystemReverificationMasters> SystemReverificationMasters { get { return this.Set<SystemReverificationMasters>(); } }
        public DbSet<SystemReverificationTemplate> SystemReverificationTemplate { get { return this.Set<SystemReverificationTemplate>(); } }

        #endregion

        #region Loan Export Monitor

        public DbSet<LoanJobExport> LoanJobExport { get { return this.Set<LoanJobExport>(); } }
        public DbSet<LoanJobExportDetail> LoanJobExportDetail { get { return this.Set<LoanJobExportDetail>(); } }
        #endregion

        #region StoredProcedure
        public DbSet<DBReportResultModel> ReportResultModel { get; set; }
        #endregion
        #endregion

        #region Overrides

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            if (this.TenantSchema != null)
            {
                #region User Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<User>()
                   .ToTable("Users");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<UserAddressDetail>()
                   .ToTable("UserAddressDetails");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<UserRoleMapping>()
                   .ToTable("UserRoleMappings");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustomAddressDetail>()
                   .ToTable("CustomAddressDetails");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<UserSecurityQuestion>()
                   .ToTable("UserSecurityQuestion");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<UserSession>()
                   .ToTable("UserSession");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditUserRequest>()
                   .ToTable("AuditUserRequest");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<KPIGoalConfig>()
                    .ToTable("KPIGoalConfig");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<UserPassword>()
                   .ToTable("UserPassword");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<KpiUserGroupConfig>().ToTable("KpiUserGroupConfig");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditKpiGoalConfig>().ToTable("AuditKpiGoalConfig");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditUserKpiGoalConfig>().ToTable("AuditUserKpiGoalConfig");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<KPIConfigStaging>().ToTable("KPIConfigStaging");
                #endregion

                #region Role Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RoleMaster>()
                   .ToTable("RoleMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ADGroupMasters>()
                   .ToTable("ADGroupMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<MenuMaster>()
                   .ToTable("MenuMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AccessURL>()
                   .ToTable("AccessURLs");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RoleMenuMapping>()
                   .ToTable("RoleMenuMappings");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<MenuGroupMaster>()
                   .ToTable("MenuGroupMasters");

                #endregion

                #region Loan Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<Loan>()
                   .ToTable("Loans");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanDetail>()
                   .ToTable("LoanDetails");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanImage>()
                   .ToTable("LoanImages");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanSearch>()
                   .ToTable("LoanSearch");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanPDF>()
                   .ToTable("LoanPDF");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<PurgeStaging>()
                    .ToTable("PurgeStaging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<PurgeStagingDetails>()
                    .ToTable("PurgeStagingDetails");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanReverification>()
                    .ToTable("LoanReverification");
                //DocumetTypeTables
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EmailTracker>()
                   .ToTable("EmailTracker");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<DocumetTypeTables>()
                   .ToTable("DocumetTypeTables");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<IDCFields>()
                  .ToTable("IDCFields");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanReporting>()
                 .ToTable("LoanReporting");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanStipulation>()
                .ToTable("LoanStipulation");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanLOSFields>()
                .ToTable("LoanLOSFields");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ELoanAttachmentDownload>()
                              .ToTable("ELoanAttachmentDownload");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSExportFileStaging>()
                              .ToTable("LOSExportFileStaging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSExportFileStagingDetail>()
                              .ToTable("LOSExportFileStagingDetail");
                #endregion

                #region Report Tables
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReportMaster>()
                .ToTable("ReportMaster");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReportConfig>()
                   .ToTable("ReportConfig");
                #endregion

                #region Customer Master Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustomerMaster>()
                   .ToTable("CustomerMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReviewTypeMaster>()
                   .ToTable("ReviewTypeMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReviewTypeMaster>()
                  .ToTable("ReviewTypeMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanTypeMaster>()
                   .ToTable("LoanTypeMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanEvaluatedResult>()
                    .ToTable("LoanEvaluatedResult");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReviewPriorityMaster>()
                  .ToTable("ReviewPriorityMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<DocumentTypeMaster>()
                   .ToTable("DocumentTypeMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<DocumentFieldMaster>()
                   .ToTable("DocumentFieldMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CheckListMaster>()
                   .ToTable("CheckListMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CheckListDetailMaster>()
                   .ToTable("CheckListDetailMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RuleMaster>()
                   .ToTable("RuleMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<StackingOrderMaster>()
                   .ToTable("StackingOrderMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<StackingOrderDetailMaster>()
                   .ToTable("StackingOrderDetailMasters");


                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<StackingOrderGroupmasters>()
                   .ToTable("StackingOrderGroupmasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustomerConfig>()
                   .ToTable("CustomerConfig");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<QCIQConnectionString>()
                 .ToTable("QCIQConnectionString");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanChecklistAudit>()
                 .ToTable("LoanChecklistAudit");


                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RuleDocumentTables>()
                 .ToTable("RuleDocumentTables");

                #endregion

                #region Mapping Tables
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RetainUpdateStaging>()
                 .ToTable("RetainUpdateStaging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewMapping>()
                   .ToTable("CustReviewMapping");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewLoanMapping>()
                   .ToTable("CustReviewLoanMapping");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustLoanDocMapping>()
                   .ToTable("CustLoanDocMapping");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewLoanCheckMapping>()
                   .ToTable("CustReviewLoanCheckMapping");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewLoanStackMapping>()
                   .ToTable("CustReviewLoanStackMapping");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewLoanUploadPath>()
                  .ToTable("CustReviewLoanUploadPath");

                #endregion

                #region IntellaLend Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<WorkFlowStatusMaster>()
                       .ToTable("WorkFlowStatusMaster");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<IntellaAndEncompassFetchFields>()
                       .ToTable("IntellaAndEncompassFetchFields");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EncompassDownloadExceptions>()
                       .ToTable("EncompassDownloadExceptions");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<TenantMaster>()
                   .ToTable("TenantMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ServiceConfig>()
                   .ToTable("ServiceConfigs");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EmailMaster>()
                   .ToTable("EmailMaster");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AppConfig>()
                   .ToTable("AppConfig");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<RequestResponseLogging>()
                   .ToTable("RequestResponseLogging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ExportProcessingQueue>()
                   .ToTable("ExportProcessingQueue");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EncompassFields>()
                  .ToTable("EncompassFields");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<InvestorStipulation>()
                   .ToTable("InvestorStipulation");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSLoanTapeFields>()
                 .ToTable("LOSLoanTapeFields");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanTapeDefinition>()
                    .ToTable("LoanTapeDefinition");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<PasswordPolicy>()
                  .ToTable("PasswordPolicy");


                #endregion

                #region Master Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<SecurityQuestionMasters>()
                   .ToTable("SecurityQuestionMasters");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<MenuAccessUrl>().ToTable("MenuAccessUrl");

                #endregion

                #region Audit Loan Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditLoan>()
                        .ToTable("AuditLoan");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditIDCFields>()
                       .ToTable("AuditIDCFields");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditLoanDetail>()
                        .ToTable("AuditLoanDetails");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditLoanMissingDoc>()
                        .ToTable("AuditLoanMissingDoc");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditLoanSearch>()
                        .ToTable("AuditLoanSearch");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EncompassConfig>()
                                    .ToTable("EncompassConfig");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EncompassAccessToken>()
                                    .ToTable("EncompassAccessToken");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ELoanAttachmentUpload>()
                                    .ToTable("ELoanAttachmentUpload");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EUploadStaging>()
                             .ToTable("EUploadStaging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EDownloadStaging>()
                             .ToTable("EDownloadStaging");

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<AuditDescriptionConfig>()
                       .ToTable("AuditDescriptionConfig");

                #endregion

                #region BoxAPI
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<BoxUserToken>()
                        .ToTable("BoxUserToken");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<BoxDownloadQueue>()
                       .ToTable("BoxDownloadQueue");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<BoxDownloadException>()
                      .ToTable("BoxDownloadException");

                #endregion

                #region Re-Verification

                if (!this.TenantSchema.Equals(this.SystemSchema))
                {
                    modelBuilder.Entity<SystemReverificationMasters>()
                     .ToTable(String.Format("{0}.ReverificationMasters", this.SystemSchema));

                    modelBuilder.Entity<SystemReverificationTemplate>()
                       .ToTable(String.Format("{0}.ReverificationTemplates", this.SystemSchema));

                    modelBuilder.Entity<DocumentTables>()
                        .ToTable(String.Format("{0}.DocumentTables", this.SystemSchema));

                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReverificationTemplate>()
                     .ToTable("ReverificationTemplates");
                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<ReverificationMaster>()
                           .ToTable("ReverificationMasters");
                }
                else if (this.TenantSchema.Equals(this.SystemSchema))
                {
                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<SystemReverificationTemplate>()
                     .ToTable("ReverificationTemplates");
                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<SystemReverificationMasters>()
                    .ToTable("ReverificationMasters");

                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<DocumentTables>()
                      .ToTable("DocumentTables");

                    modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<EncompassParkingSpot>()
                      .ToTable("EncompassParkingSpot");
                }

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReverificationDocMapping>()
                       .ToTable("CustReverificationDocMapping");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<CustReviewLoanReverifyMapping>()
                       .ToTable("CustReviewLoanReverifyMapping");

                #endregion

                #region System Tables

                if (!this.TenantSchema.Equals(this.SystemSchema))
                {
                    modelBuilder.Entity<SystemReviewTypeMaster>()
                       .ToTable(String.Format("{0}.ReviewTypeMasters", this.SystemSchema));

                    modelBuilder.Entity<SystemLoanTypeMaster>()
                       .ToTable(String.Format("{0}.LoanTypeMasters", this.SystemSchema));

                    modelBuilder.Entity<ImportStagings>()
                      .ToTable(String.Format("{0}.ImportStagings", this.SystemSchema));
                }

                #endregion

                #region Loan Export Monitor Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanJobExport>()
                 .ToTable("LoanJobExport");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LoanJobExportDetail>()
                .ToTable("LoanJobExportDetail");

                #endregion

                #region LOS Tables

                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSImportStaging>()
                      .ToTable("LOSImportStaging");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSDocument>()
                    .ToTable("LOSDocument");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSLoanDetails>()
                    .ToTable("LOSLoanDetails");
                modelBuilder.HasDefaultSchema(this.TenantSchema).Entity<LOSDocumentFields>()
                    .ToTable("LOSDocumentFields");

                #endregion

            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region IDbModelCacheKeyProvider Members

        public string CacheKey
        {
            get { return this.TenantSchema; }
        }

        #endregion
    }

    //public class DbSetProxy<TEntity> : DbSet<TEntity>,
    //                               IQueryable<TEntity>,
    //                               IDbAsyncEnumerable<TEntity>
    //where TEntity : class
    //{
    //    private readonly DbSet<TEntity> set;
    //    private readonly DbQuery<TEntity> query;

    //    private readonly IQueryable<TEntity> intercepted;

    //    public DbSetProxy(DbSet<TEntity> set)
    //        : this(set, set)
    //    {
    //    }

    //    public DbSetProxy(DbSet<TEntity> set, DbQuery<TEntity> query)
    //    {
    //        this.set = set;
    //        this.query = query;

    //        // use NeinLinq or any other LINQ proxy library
    //        intercepted = query.Rewrite(new MyInterceptor());
    //    }
    //    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    //    {
    //        return intercepted.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return intercepted.GetEnumerator();
    //    }

    //    Type IQueryable.ElementType
    //    {
    //        get { return intercepted.ElementType; }
    //    }

    //    Expression IQueryable.Expression
    //    {
    //        get { return intercepted.Expression; }
    //    }

    //    IQueryProvider IQueryable.Provider
    //    {
    //        get { return intercepted.Provider; }
    //    }

    //    IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
    //    {
    //        return ((IDbAsyncEnumerable<TEntity>)intercepted).GetAsyncEnumerator();
    //    }

    //    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
    //    {
    //        return ((IDbAsyncEnumerable<TEntity>)intercepted).GetAsyncEnumerator();
    //    }
    //    public override DbQuery<TEntity> AsNoTracking()
    //    {
    //        return new DbSetProxy<TEntity>(set, query.AsNoTracking());
    //    }

    //    public override DbQuery<TEntity> AsStreaming()
    //    {
    //        return new DbSetProxy<TEntity>(set, query.AsStreaming());
    //    }

    //    public override DbQuery<TEntity> Include(string path)
    //    {
    //        return new DbSetProxy<TEntity>(set, query.Include(path));
    //    }

    //    public override TEntity Add(TEntity entity)
    //    {
    //        return set.Add(entity);
    //    }

    //    public override IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    //    {
    //        return set.AddRange(entities);
    //    }

    //    public override TEntity Attach(TEntity entity)
    //    {
    //        return set.Attach(entity);
    //    }

    //    public override TEntity Create()
    //    {
    //        return set.Create();
    //    }

    //    public override TDerivedEntity Create<TDerivedEntity>()
    //    {
    //        return set.Create<TDerivedEntity>();
    //    }

    //    public override TEntity Find(params object[] keyValues)
    //    {
    //        return set.Find(keyValues);
    //    }

    //    public override Task<TEntity> FindAsync(params object[] keyValues)
    //    {
    //        return set.FindAsync(keyValues);
    //    }

    //    public override Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
    //    {
    //        return set.FindAsync(cancellationToken, keyValues);
    //    }

    //    public override TEntity Remove(TEntity entity)
    //    {
    //        return set.Remove(entity);
    //    }

    //    public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
    //    {
    //        return set.RemoveRange(entities);
    //    }

    //    public override DbSqlQuery<TEntity> SqlQuery(string sql, params object[] parameters)
    //    {
    //        return set.SqlQuery(sql, parameters);
    //    }

    //    public override ObservableCollection<TEntity> Local
    //    {
    //        get { return set.Local; }
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return set.Equals(obj);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return set.GetHashCode();
    //    }

    //    public override string ToString()
    //    {
    //        return set.ToString();
    //    }
    //}

    //internal class NoLockInterceptor : DbCommandInterceptor
    //{
    //    public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
    //    {

    //    }

    //    public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
    //    {

    //    }

    //    public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
    //    {

    //    }

    //    public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
    //    {

    //    }
    //    public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
    //    {

    //    }

    //    public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
    //    {

    //    }

    //}

}
