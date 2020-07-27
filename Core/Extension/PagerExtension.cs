using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Extension {
    public class Pager<T> {
        public int RecordsTotal { get; private set; }
        public int RecordsFiltered => RecordsTotal;

        public int StartPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }

        public int Start { get; private set; }
        public int EndPage { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public Pager(IEnumerable<T> list, int totalItems, int? start, int length = 20) {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)length);
            var currentPage = start ?? 1;
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

            RecordsTotal = totalItems;
            Start = currentPage;
            PageSize = length;
            TotalPages = totalPages;
            StartPages = startPage;
            EndPage = endPage;
            Data = list;
        }
    }

    public class PagerFilter {
        public string Search { get; set; }
        // public int Skip { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }
        public List<PagerSortQuery> Sort { get; set; }
    }

    public class PagerSortQuery {
        public bool Desc { get; set; } = false;
        public string Selector { get; set; }
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
