using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Services.Base {
    public interface IEntityManager<T>: IManager where T : class {
        Task<IQueryable<T>> All();
        Task<bool> Contains(Expression<Func<T, bool>> where);
        Task<T> Find(params object[] keys);
        Task<T> Find(Expression<Func<T, bool>> where);
        Task<T> Create(T t);
        Task<IEnumerable<T>> Create(IEnumerable<T> l);
        Task<int> Delete(T t);
        Task<int> Delete(IEnumerable<T> l);
        Task<int> Delete(Expression<Func<T, bool>> where);
        Task<T> Update(T t);
        Task<IEnumerable<T>> Update(IEnumerable<T> l);
        Task<int> Count { get; }
        Task<IQueryable<T>> Filter(Expression<Func<T, bool>> where);
        Task<IQueryable<T>> Filter<Key>(Expression<Func<T, bool>> where, int index, int limit);
        Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, string sort, int start, int length, params string[] properties);
        Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, string sort, bool descSort, int start, int length, params string[] properties);
    }
}
