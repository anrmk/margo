using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface ITodoManager: IEntityManager<TodoEntity> {
        Task<TodoEntity> FindInclude(Guid id);
        Task<List<TodoEntity>> FindAll();
        Task<List<TodoEntity>> FindAll(Guid[] ids);
    }

    public class TodoManager: AsyncEntityManager<TodoEntity>, ITodoManager {

        public TodoManager(IApplicationContext context) : base(context) { }

        public async Task<TodoEntity> FindInclude(Guid id) {
            return await DbSet
                .Include(x => x.User)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<List<TodoEntity>> FindAll() {
            return await DbSet.ToListAsync();
        }

        public async Task<List<TodoEntity>> FindAll(Guid[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }
    }
}
