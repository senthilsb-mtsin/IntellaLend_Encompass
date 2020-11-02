using System.Data.Entity;

namespace DataImport
{
    public partial class REPORTDB : DbContext
    {
        public REPORTDB()
            : base("name=REPORTDB")
        {
        }

        public virtual DbSet<BATCH> BATCHES { get { return this.Set<BATCH>(); } }
        public virtual DbSet<CONFIGURATION> CONFIGURATIONS { get { return this.Set<CONFIGURATION>(); } }
        public virtual DbSet<FIELD> FIELDS { get { return this.Set<FIELD>(); } }
        public virtual DbSet<PAGE> PAGES { get { return this.Set<PAGE>(); } }


        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            #region User Tables

            modelBuilder.Entity<BATCH>()
               .ToTable("BATCHES");

            modelBuilder.Entity<CONFIGURATION>()
               .ToTable("CONFIGURATIONS");

            modelBuilder.Entity<FIELD>()
               .ToTable("FIELDS");

            modelBuilder.Entity<PAGE>()
               .ToTable("PAGES");


            #endregion

            base.OnModelCreating(modelBuilder);
        }





    }
}
