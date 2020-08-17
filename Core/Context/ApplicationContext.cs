using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Core.Data.Entities;
using Core.Extension;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;


using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Context {
    public interface IApplicationContext {
        Database ApplicationDatabase { get; }
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }

    public class ApplicationContext: IdentityDbContext<AspNetUserEntity>, IApplicationContext {
        private readonly IConfiguration _configuration;
        private readonly ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region DbSet
        public DbSet<AspNetUserRequestEntity> UserRequests { get; set; }

        //public DbSet<LogEntity> Logs { get; set; }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CompanyDataEntity> CompanyData { get; set; }
        //public DbSet<CompanySectionEntity> CompanySections { get; set; }
        //public DbSet<CompanySectionFieldEntity> CompanySectionFields { get; set; }

        //public DbSet<SectionEntity> Sections { get; set; }
        //public DbSet<SectionFieldEntity> SectionFields { get; set; }

        public DbSet<VendorEntity> Vendors { get; set; }
        //public DbSet<VendorSectionEntity> VendorSections { get; set; }
        public DbSet<VendorFieldEntity> VendorSectionFields { get; set; }
        //public DbSet<VendorAddressEntity> VendorAddress { get; set; }
        //public DbSet<VendorMediaEntity> VendorMedias { get; set; }

        public DbSet<UccountEntity> Uccounts { get; set; }
        //public DbSet<UccountSectionEntity> UccountSections { get; set; }
        //public DbSet<UccountSectionFieldEntity> UccountSectionFields { get; set; }

        public DbSet<UccountServiceEntity> Services { get; set; }

        public DbSet<PersonEntity> Persons { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CategoryFieldEntity> CategoryFields { get; set; }

        public DbSet<InvoiceEntity> Invoices { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }

        #endregion

        public Database ApplicationDatabase { get; private set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration, IPrincipal principal, IHttpContextAccessor httpContextAccessor) : base(options) {
            _configuration = configuration;
            _principal = principal as ClaimsPrincipal;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity("Core.Data.Entities.AspNetUserGrantEntity", b => {
                b.HasOne("Core.Data.Entities.CompanyEntity", "Company")
                    .WithMany("Grants")
                    .HasForeignKey("EntityId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity<CompanyEntity>().HasQueryFilter(x =>
                !x.Grants.Any(z => z.UserId == _httpContextAccessor.HttpContext.User.GetUserId())
                || x.Grants.SingleOrDefault(z => z.UserId == _httpContextAccessor.HttpContext.User.GetUserId()).IsGranted);
        }

        public async Task<int> SaveChangesAsync() {
            var modifiedEntries = ChangeTracker.Entries()
              .Where(x => x.Entity is IAuditableEntity
                  && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach(var entry in modifiedEntries) {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if(entity != null) {

                    string identityName = _principal?.Identity.Name ?? "system"; // Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.Now;

                    if(entry.State == EntityState.Added) {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    } else {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }
                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }
            bool saveFailed;
            do {
                saveFailed = false;
                try {
                    return await base.SaveChangesAsync();
                } catch(DbUpdateConcurrencyException e) {
                    saveFailed = true;
                    Console.WriteLine(e.Message);
                    return -1001;
                } catch(DbUpdateException e) {
                    Console.WriteLine(e.Message);
                    saveFailed = true;
                    return -1002;
                } catch(Exception e) {
                    Console.WriteLine(e.Message);
                    saveFailed = true;
                    return -1003;
                }
            } while(saveFailed);
        }
    }
}
