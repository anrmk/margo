using System;
using System.Linq.Expressions;

namespace Core.Services.Business {
    public abstract class BaseBusinessManager {
        public static Expression<Func<TSource, string>> GetExpression<TSource>(string propertyName) {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(string));
            return Expression.Lambda<Func<TSource, string>>(conversion, param);
        }
    }
}
