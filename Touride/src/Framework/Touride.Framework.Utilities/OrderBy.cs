using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Touride.Framework.Utilities
{
    public static class OrderBy
    {
        public static IEnumerable<TEntity> CustumOrderBy<TEntity>(IEnumerable<TEntity> source,
            string orderByProperty, bool desc)
        {
            if (source == null) return null;
            orderByProperty = orderByProperty == "İd" ? "Id" : orderByProperty;
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            if (property == null) { return source; }
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command,
                new[] { type, property.PropertyType },
                source.AsQueryable().Expression,
                Expression.Quote(orderByExpression));
            return source.AsQueryable().Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
