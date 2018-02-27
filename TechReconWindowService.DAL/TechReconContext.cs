using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReconWindowService.BAL.Helpers;
using TechReconWindowService.DAL.Interfaces;

namespace TechReconWindowService.DAL
{

    public partial class TechReconContext : DbContext, IUnitOfWork
    {
        static TechReconContext()
        {
            Database.SetInitializer<TechReconContext>(null);
        }
        public TechReconContext()
            : base("TechReconEntities")
        {

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            // Configuration.MergeOption = MergeOption.NoTracking;
          //  var adapter = (IObjectContextAdapter)this;
            //var objectContext = adapter.ObjectContext;
           // objectContext.CommandTimeout = 120;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            throw new UnintentionalCodeFirstException();
        }

        public DbSet<rptDBAuditTrail> DBAudits { get; set; }
        public override int SaveChanges()
        {
            throw new InvalidOperationException("User ID must be provided");
        }
        private List<rptDBAuditTrail> GetAuditRecordsForChange(DbEntityEntry dbEntry, string UserId)
        {
            List<rptDBAuditTrail> result = new List<rptDBAuditTrail>();
            try
            {
                DateTime changeTime = DateTime.Now;

                System.ComponentModel.DataAnnotations.Schema.TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.Schema.TableAttribute;

                string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;

                var keyNames = dbEntry.Entity.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false).Count() > 0).ToList();

                string keyName = keyNames[0].Name;
                if (dbEntry.State == System.Data.Entity.EntityState.Deleted)
                {
                    result.Add(new rptDBAuditTrail()
                    {
                        ItbId = Guid.NewGuid(),
                        userid = UserId,
                        eventdateutc = changeTime,
                        eventtype = "D", // Deleted
                        tablename = tableName,
                        recordid = dbEntry.GetDatabaseValues().GetValue<object>(keyName).ToString(),
                        columnname = "*ALL",
                        newvalue = "yes",
                    }
                        );
                }
                else if (dbEntry.State == System.Data.Entity.EntityState.Modified)
                {
                    foreach (string propertyName in dbEntry.OriginalValues.PropertyNames)
                    {
                        var gf = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString();
                        var ga = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString();
                        if (gf != ga)
                        {
                            result.Add(new rptDBAuditTrail()
                            {
                                ItbId = Guid.NewGuid(),
                                userid = UserId,
                                eventdateutc = changeTime,
                                eventtype = "M",    // Modified
                                tablename = tableName,
                                recordid = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                                columnname = propertyName,
                                originalvalue = dbEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                                newvalue = dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                            }
                                );
                        }
                        
                    }
                }
              
            }
            catch
            {
            }

            return result;
        }

        public async Task<int> Commit(int userid, string UserId)
        {
           
            try
            {

                // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
                foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == System.Data.Entity.EntityState.Deleted || p.State == System.Data.Entity.EntityState.Modified))
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (rptDBAuditTrail x in GetAuditRecordsForChange(ent, UserId))
                    {
                        this.DBAudits.Add(x);
                    }
                }
              //   var gh = base.SaveChanges();
               //  return gh;
                // Call the original SaveChanges(), which will save both the changes made and the audit records
               return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                try
                {
                    foreach (var entityEntry in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
                    {
                        if (entityEntry.Entity != null)
                        {
                            this.Entry(entityEntry.Entity).State = EntityState.Detached;
                        }
                    }
                }
                catch { }
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat(" Property: {0},  Error: {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();

                        LogManager.SaveLog("An error occured in TechReconContext.cs: " + sb);
                    }

                }

                //throw new DbEntityValidationException(
                //    "Entity Validation Failed - errors follow:\n" +
                //    sb.ToString(), ex
                //    ); // Add the original exception as the innerException








                return 0;
            }

            //return 0;
        }

        public int CommitNonAsync(int userid, string UserName)
        {
           
            try
            {

                // Get all Added/Deleted/Modified entities (not Unmodified or Detached)
                foreach (var ent in this.ChangeTracker.Entries().Where(p => p.State == System.Data.Entity.EntityState.Deleted || p.State == System.Data.Entity.EntityState.Modified))
                {
                    // For each changed record, get the audit record entries and add them
                    foreach (rptDBAuditTrail x in GetAuditRecordsForChange(ent, UserName))
                    {
                        this.DBAudits.Add(x);
                    }
                }

                // var gh = base.SaveChanges();
                // return gh;
                // Call the original SaveChanges(), which will save both the changes made and the audit records
                return base.SaveChanges();




            }
            catch (DbEntityValidationException ex)
            {
                try
                {
                    foreach (var entityEntry in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
                    {
                        if (entityEntry.Entity != null)
                        {
                            this.Entry(entityEntry.Entity).State = EntityState.Detached;
                        }
                    }
                }
                catch { }

                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                        LogManager.SaveLog("An error occured in TechReconContext.cs: " + sb);
                    }

                    
                }

                return 0;

                //throw new DbEntityValidationException(
                //    "Entity Validation Failed - errors follow:\n" +
                //    sb.ToString(), ex
                //    ); // Add the original exception as the innerException
            }

            //return 0;
        }
    }
}
