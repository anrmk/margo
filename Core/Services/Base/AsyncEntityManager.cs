using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Core.Context;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Base {
    public abstract class AsyncEntityManager<T>: IEntityManager<T> where T : class {
        protected IApplicationContext _context;

        public bool ShareContext { get; set; } = false;

        protected AsyncEntityManager(IApplicationContext context) {
            _context = context;
        }

        public AsyncEntityManager() {
        }

        protected DbSet<T> DbSet => _context.Set<T>();

        public virtual Task<int> Count => DbSet.CountAsync();

        public async Task<IQueryable<T>> All() {
            try {
                var x = (await DbSet.ToListAsync()).AsQueryable();
                return x;
            } catch(Exception exception) {
                throw exception;
            }
        }

        public virtual async Task<IQueryable<T>> Filter(Expression<Func<T, bool>> predicate) {
            return (await DbSet.Where(predicate).ToListAsync()).AsQueryable<T>();
        }

        public async Task<bool> Contains(Expression<Func<T, bool>> predicate) {
            return await DbSet.CountAsync(predicate) > 0;
        }

        public virtual async Task<T> Find(params object[] keys) {
            return await DbSet.FindAsync(keys);
        }

        public virtual async Task<T> Find(Expression<Func<T, bool>> predicate) {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<T>> Create(IEnumerable<T> l) {
            foreach(var t in l) {
                var entry = _context.Entry(t);
                DbSet.Attach(t);
                entry.State = EntityState.Added;
            }

            if(!ShareContext)
                await _context.SaveChangesAsync();
            return l;
        }

        public virtual async Task<T> Create(T t) {
            try {
                var entry = DbSet.Add(t);
                entry.State = EntityState.Added;
                if(!ShareContext)
                    await _context.SaveChangesAsync();

                return entry.Entity;
            } catch(Exception ex) {
                throw ex;
            }
        }

        public virtual async Task<int> Delete(T t) {
            DbSet.Remove(t);
            if(!ShareContext)
                return await _context.SaveChangesAsync();
            return 0;
        }

        public virtual async Task<int> Delete(IEnumerable<T> l) {
            DbSet.RemoveRange(l);

            if(!ShareContext)
                return await _context.SaveChangesAsync();
            return 0;
        }

        public virtual async Task<IEnumerable<T>> Update(IEnumerable<T> l) {
            foreach(var t in l) {
                var entry = _context.Entry(t);
                DbSet.Attach(t);
                entry.State = EntityState.Modified;
            }

            if(!ShareContext)
                await _context.SaveChangesAsync();
            return l;
        }

        public virtual async Task<T> Update(T t) {
            var entry = _context.Entry(t);
            DbSet.Attach(t);
            entry.State = EntityState.Modified;
            if(!ShareContext)
                await _context.SaveChangesAsync();
            return t;
        }

        public virtual async Task<int> Delete(Expression<Func<T, bool>> predicate) {
            var objects = await Filter(predicate);
            foreach(var obj in objects)
                DbSet.Remove(obj);
            if(!ShareContext)
                return await _context.SaveChangesAsync();
            return 0;
        }

        public async Task<IQueryable<T>> Filter<Key>(Expression<Func<T, bool>> where, int offset, int limit) {
            int skipCount = offset * limit;
            var query = where is null ? DbSet.AsQueryable() : DbSet.Where(where).AsQueryable();
            query = skipCount == 0 ? query.Take(limit) : query.Skip(skipCount).Take(limit);

            return (await query.ToListAsync()).AsQueryable();
        }

        public async Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, Expression<Func<T, string>> order, bool descSort, int? offset, int? limit, params string[] properties) {
            var query = where is null ? DbSet.AsQueryable() : DbSet.Where(where).AsQueryable();
            int count = await query.CountAsync();

            if(order is null) {
                query = query.Skip(offset ?? 0);
            } else {
                query = descSort ? query.OrderByDescending(order).Skip(offset ?? 0) : query.OrderBy(order).Skip(offset ?? 0);
            }

            foreach(var prop in properties)
                query = query.Include(prop);

            query = limit.HasValue ? query.Take(limit.Value) : query;
            var result = await query.ToListAsync();
            return new Tuple<List<T>, int>(result, count);
        }

        public async Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, Expression<Func<T, string>> order, int? offset, int? limit, params string[] properties) {
            return await Pager<Key>(where, order, false, offset, limit, properties);
        }
    }
}
