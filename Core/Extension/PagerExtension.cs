using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extension {
    public class Pager<T> {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int? PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public Pager(IEnumerable<T> list, int totalItems, int? page, int? pageSize) {
            var t = (decimal)totalItems / (decimal)(pageSize ?? totalItems);
            var totalPages = (int)Math.Ceiling(t);
            var currentPage = page ?? 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if(startPage <= 0) {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if(endPage > totalPages) {
                endPage = totalPages;
                if(endPage > 10) {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            Items = list;
        }
    }

    public class PagerFilter {
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public bool RandomSort { get; set; } = false;
    }

    public static class SortExtension {
        public static IQueryable<TEntity> OrderByDynamic<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc) {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
