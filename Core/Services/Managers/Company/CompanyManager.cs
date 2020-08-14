using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ICompanyManager: IEntityManager<CompanyEntity> {
        Task<CompanyEntity> FindInclude(Guid id);
        Task<List<CompanyEntity>> FindAll(bool ignoreQueryFilters = false);
        Task<List<CompanyEntity>> FindAll(Guid[] ids);
        Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId);
    }

    public class CompanyManager: AsyncEntityManager<CompanyEntity>, ICompanyManager {
        public CompanyManager(IApplicationContext context) : base(context) {
        }

        public async Task<CompanyEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.Sections)
                    .ThenInclude(x => x.Fields)
                .Include(x => x.Data)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(bool ignoreQueryFilters) {
            return await (ignoreQueryFilters
                ? DbSet.IgnoreQueryFilters()
                : DbSet).ToListAsync();
        }

        public async Task<List<CompanyEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<List<AspNetUserGrantEntity>> FindAllGrantsByUser(string userId) {
            var companies = await FindAll(true);
            var companyGrants =
                await DbSet.SelectMany(x =>
                    x.Grants.Select(x =>
                        new AspNetUserGrantEntity {
                            Id = x.Id,
                            EntityId = x.EntityId,
                            Company = x.Company,
                            IsGranted = x.IsGranted,
                            UserId = x.UserId
                        })
                    .Where(z => z.UserId == userId))
                .IgnoreQueryFilters()
                .ToDictionaryAsync(x => x.EntityId, x => x);

            var result = new List<AspNetUserGrantEntity>();
            foreach(var company in companies) {
                if(companyGrants.ContainsKey(company.Id)) {
                    result.Add(companyGrants[company.Id]);
                } else {
                    result.Add(new AspNetUserGrantEntity {
                        EntityId = company.Id,
                        Company = company,
                        IsGranted = true,
                        UserId = userId
                    });
                }
            }

            return result;
        }
    }
}
