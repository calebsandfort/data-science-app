using Microsoft.EntityFrameworkCore;
using DataScienceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataScienceApp.Framework
{
    public static class ExtensionMethods
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

        public static void Clear<T>(this DbSet<T> dbSet, IQueryable<T> iQueryable) where T : class
        {
            dbSet.RemoveRange(iQueryable);
        }

        public static void Merge<T>(this List<T> masterList, List<T> listToMerge) where T : EntityBase
        {
            foreach (T itemToMerge in listToMerge)
            {
                if (!masterList.Contains(itemToMerge))
                {
                    masterList.Add(itemToMerge);
                }
            }
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, String sortProperty, String sortDirection)
        {
            return source.OrderBy(sortProperty, sortDirection == "desc");
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
