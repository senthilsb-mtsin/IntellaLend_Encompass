using IL.EntityModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTSEntityDataAccess
{
    public class DBConnect : DbContext
    {
        public static string TableSchema { get; set; }
        public const string MTSSchema = "IL";

        public DBConnect() { }

        public DBConnect(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #region User Tables

        public DbSet<User> Users { get; set; }
        public DbSet<UserAddressDetail> UserAddressDetail { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<CustomAddressDetail> CustomAddressDetail { get; set; }
        public DbSet<UserSecurityQuestion> UserSecurityQuestion { get; set; }

        #endregion

        #region Role Tables

        public DbSet<RoleMaster> Roles { get; set; }
        public DbSet<MenuMaster> Menus { get; set; }
        public DbSet<AccessURL> AccessURLs { get; set; }
        public DbSet<RoleMenuMapping> RoleMenuMapping { get; set; }

        #endregion

        #region Loan Tables

        public DbSet<Loan> Loan { get; set; }
        public DbSet<LoanDetail> LoanDetail { get; set; }
        public DbSet<LoanImage> LoanImage { get; set; }

        #endregion

        #region Customer Master Tables

        public DbSet<CustomerMaster> CustomerMaster { get; set; }
        public DbSet<ReviewTypeMaster> ReviewTypeMaster { get; set; }
        public DbSet<LoanTypeMaster> LoanTypeMaster { get; set; }
        public DbSet<DocumentTypeMaster> DocumentTypeMaster { get; set; }
        public DbSet<DocumentFieldMaster> DocumentFieldMaster { get; set; }
        public DbSet<CheckListMaster> CheckListMaster { get; set; }
        public DbSet<CheckListDetailMaster> CheckListDetailMaster { get; set; }
        public DbSet<RuleMaster> RuleMaster { get; set; }
        public DbSet<StackingOrderMaster> StackingOrderMaster { get; set; }
        public DbSet<StackingOrderDetailMaster> StackingOrderDetailMaster { get; set; }

        #endregion

        #region Mapping Tables

        public DbSet<CustReviewMapping> CustReviewMapping { get; set; }
        public DbSet<CustReviewLoanMapping> CustReviewLoanMapping { get; set; }
        public DbSet<CustReviewLoanDocMapping> CustReviewLoanDocMapping { get; set; }
        public DbSet<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get; set; }
        public DbSet<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get; set; }

        #endregion

        #region IntellaLend Tables

        public DbSet<WorkFlowStatusMaster> WorkFlowStatusMaster { get; set; }
        public DbSet<TenantMaster> TenantMaster { get; set; }
        public DbSet<ServiceConfig> ServiceConfig { get; set; }
        public DbSet<EmailMaster> EmailMaster { get; set; }

        #endregion

        #region Master Tables

        public DbSet<SecurityQuestionMasters> SecurityQuestionMasters { get; set; }

        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User Tables

            modelBuilder.Entity<User>()
               .ToTable("Users", schema: TableSchema);

            modelBuilder.Entity<UserAddressDetail>()
               .ToTable("UserAddressDetails", schema: TableSchema);

            modelBuilder.Entity<UserRoleMapping>()
               .ToTable("UserRoleMappings", schema: TableSchema);

            modelBuilder.Entity<CustomAddressDetail>()
               .ToTable("CustomAddressDetails", schema: TableSchema);

            modelBuilder.Entity<UserSecurityQuestion>()
               .ToTable("UserSecurityQuestion", schema: TableSchema);

            #endregion

            #region Role Tables

            modelBuilder.Entity<RoleMaster>()
               .ToTable("RoleMasters", schema: TableSchema);

            modelBuilder.Entity<MenuMaster>()
               .ToTable("MenuMasters", schema: TableSchema);

            modelBuilder.Entity<AccessURL>()
               .ToTable("AccessURLs", schema: TableSchema);

            modelBuilder.Entity<RoleMenuMapping>()
               .ToTable("RoleMenuMappings", schema: TableSchema);

            #endregion

            #region Loan Tables

            modelBuilder.Entity<Loan>()
               .ToTable("Loans", schema: TableSchema);

            modelBuilder.Entity<LoanDetail>()
               .ToTable("LoanDetails", schema: TableSchema);

            modelBuilder.Entity<LoanImage>()
               .ToTable("LoanImages", schema: TableSchema);

            #endregion

            #region Customer Master Tables

            modelBuilder.Entity<CustomerMaster>()
               .ToTable("CustomerMasters", schema: TableSchema);

            modelBuilder.Entity<ReviewTypeMaster>()
               .ToTable("ReviewTypeMasters", schema: TableSchema);

            modelBuilder.Entity<LoanTypeMaster>()
               .ToTable("LoanTypeMasters", schema: TableSchema);

            modelBuilder.Entity<DocumentTypeMaster>()
               .ToTable("DocumentTypeMasters", schema: TableSchema);

            modelBuilder.Entity<DocumentFieldMaster>()
               .ToTable("DocumentFieldMasters", schema: TableSchema);

            modelBuilder.Entity<CheckListMaster>()
               .ToTable("CheckListMasters", schema: TableSchema);

            modelBuilder.Entity<CheckListDetailMaster>()
               .ToTable("CheckListDetailMasters", schema: TableSchema);


            modelBuilder.Entity<RuleMaster>()
               .ToTable("RuleMasters", schema: TableSchema);

            modelBuilder.Entity<StackingOrderMaster>()
               .ToTable("StackingOrderMasters", schema: TableSchema);

            modelBuilder.Entity<StackingOrderDetailMaster>()
               .ToTable("StackingOrderDetailMasters", schema: TableSchema);

            #endregion

            #region Mapping Tables

            modelBuilder.Entity<CustReviewMapping>()
               .ToTable("CustReviewMapping", schema: TableSchema);

            modelBuilder.Entity<CustReviewLoanMapping>()
               .ToTable("CustReviewLoanMapping", schema: TableSchema);

            modelBuilder.Entity<CustReviewLoanDocMapping>()
               .ToTable("CustReviewLoanDocMapping", schema: TableSchema);

            modelBuilder.Entity<CustReviewLoanCheckMapping>()
               .ToTable("CustReviewLoanCheckMapping", schema: TableSchema);

            modelBuilder.Entity<CustReviewLoanStackMapping>()
               .ToTable("CustReviewLoanStackMapping", schema: TableSchema);

            #endregion

            #region IntellaLend Tables

            modelBuilder.Entity<WorkFlowStatusMaster>()
                   .ToTable("WorkFlowStatusMaster", schema: MTSSchema);

            modelBuilder.Entity<TenantMaster>()
               .ToTable("TenantMasters", schema: MTSSchema);

            modelBuilder.Entity<ServiceConfig>()
               .ToTable("ServiceConfigs", schema: MTSSchema);

            modelBuilder.Entity<EmailMaster>()
               .ToTable("EmailMaster", schema: MTSSchema);

            #endregion

            #region Master Tables

            modelBuilder.Entity<SecurityQuestionMasters>()
               .ToTable("SecurityQuestionMasters", schema: TableSchema);

            #endregion
        }

    }
}
