using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpenseTracker.Core.Domain.Entities;
using LinqKit;

namespace ExpenseTracker.Infrastructure.API.Extensions
{
    public static class QueryExtensions
    {
        public static Expression<Func<TEntity, bool>> ContainsById<TEntity>(ICollection<Guid> ids, int cachableLevel = 20)
            where TEntity : BaseEntity
        {
            if (ids.Any() == false)
            {
                throw new Exception($"${nameof(ids)} must not be empty");
            }

            Expression<Func<TEntity, bool>> predicate;

            if (ids.Count == 1)
            {
                Guid id = ids.First();
                predicate = entity => entity.Id == id;
            }
            else if (ids.Count > cachableLevel)
            {
                predicate = entity => ids.Contains(entity.Id);
            }
            else
            {
                predicate = PredicateBuilder.New<TEntity>();

                foreach (Guid id in ids)
                {
                    Guid temp = id;
                    predicate = predicate.Or(c => c.Id == temp);
                }
            }

            return predicate;
        }

        public static IQueryable<TEntity> WhereContainsById<TEntity>(this IQueryable<TEntity> source,
            ICollection<Guid> collection) where TEntity : BaseEntity
        {
            return source.WhereContainsBy(x => x.Id, collection);
        }

        public static IQueryable<TEntity> WhereNotContainsById<TEntity>(this IQueryable<TEntity> source,
          ICollection<Guid> collection) where TEntity : BaseEntity
        {
            return source.WhereNotContainsBy(x => x.Id, collection);
        }

        public static IQueryable<TEntity> WhereContainsBy<TEntity, TProperty>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> propertyExpression, ICollection<TProperty> collection) where TEntity : class
        {
            return source.ApplyContainsExpression(propertyExpression, collection, ContainsMode.In);
        }

        public static IQueryable<TEntity> WhereNotContainsBy<TEntity, TProperty>(this IQueryable<TEntity> source,
           Expression<Func<TEntity, TProperty>> propertyExpression, ICollection<TProperty> collection) where TEntity : class
        {
            return source.ApplyContainsExpression(propertyExpression, collection, ContainsMode.NotIn);
        }

        public static IQueryable<TResult> SelectManyWhereContainsById<TEntity, TItem, TResult>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, IEnumerable<TItem>>> collectionExpression,
            ICollection<Guid> collection,
            Expression<Func<TEntity, TItem, TResult>> mapper)
            where TItem : BaseEntity
        {
            return source.SelectManyWhereContainsBy(collectionExpression, x => x.Id, collection, mapper);
        }

        public static IQueryable<TResult> SelectManyWhereContainsBy<TEntity, TItem, TProperty, TResult>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, IEnumerable<TItem>>> collectionExpression,
            Expression<Func<TItem, TProperty>> propertyExpression,
            ICollection<TProperty> collection,
            Expression<Func<TEntity, TItem, TResult>> selector)
            where TItem : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!collection.Any())
                throw new ArgumentException($"{collection} should contains at least 1 element", nameof(collection));

            var expression = ContainsExpressionBuilder.BuildWhereContainsExpression(collectionExpression, propertyExpression, collection, ContainsMode.In);
            return source.SelectMany(expression, selector);
        }

    }

    public enum ContainsMode
    {
        In,
        NotIn
    }

    public static class ContainsExpressionBuilder
    {
        public static IQueryable<TEntity> ApplyContainsExpression<TEntity, TProperty>(this IQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> propertyExpression, ICollection<TProperty> collection, ContainsMode containsMode, int cachableLevel = 20)
            where TEntity : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (!collection.Any())
            {
                if (containsMode == ContainsMode.In)
                {
                    throw new Exception($"${nameof(collection)} must not be empty");
                }
                else
                {
                    return source;
                }
            }

            Expression<Func<TEntity, bool>> expression = BuildContainsExpression(propertyExpression, collection, containsMode, cachableLevel);
            return source.Where(expression);
        }

        public static Expression<Func<TEntity, bool>> BuildContainsExpression<TEntity, TProperty>(
           Expression<Func<TEntity, TProperty>> propertyExpression, ICollection<TProperty> collection, ContainsMode containsMode, int cachableLevel = 20)
           where TEntity : class
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (!(propertyExpression.Body is MemberExpression))
                throw new ArgumentException(
                    "Property expression should be correct and type of property and collection should be equals. When property types is nullable, collection type also shoud be nullable.",
                    nameof(propertyExpression));

            ParameterExpression parameterExpression = propertyExpression.Parameters[0];
            var memberExpression = (MemberExpression)propertyExpression.Body;

            if (collection.Count > cachableLevel)
            {
                return containsMode == ContainsMode.In
                    ? CreateContainsExpression<TEntity, TProperty>(parameterExpression, memberExpression, collection)
                    : CreateNotContainsExpression<TEntity, TProperty>(parameterExpression, memberExpression, collection);
            }

            return containsMode == ContainsMode.In
                ? CreateOrExpression<TEntity, TProperty>(parameterExpression, memberExpression, collection)
                : CreateAndExpression<TEntity, TProperty>(parameterExpression, memberExpression, collection);
        }

        public static Expression<Func<TEntity, IEnumerable<TItem>>> BuildWhereContainsExpression<TEntity, TItem, TProperty>(
            Expression<Func<TEntity, IEnumerable<TItem>>> collectionAccessorExpression,
            Expression<Func<TItem, TProperty>> propertyExpression,
            ICollection<TProperty> collection,
            ContainsMode containsMode,
            int cachableLevel = 20)
            where TItem : class
        {
            if (collectionAccessorExpression == null)
                throw new ArgumentNullException(nameof(collectionAccessorExpression));

            // x.values
            Expression collectionAccessor = collectionAccessorExpression.Body;

            // y => y.property == value op y.property == value2 ...
            Expression<Func<TItem, bool>> innerExpression = BuildContainsExpression(propertyExpression, collection, containsMode, cachableLevel);

            // x.values.Where(y => y.property == value op y.property == value2 ..)
            MethodCallExpression memberCall = Expression.Call(null, GetGenericMethodInfo<TItem>("Where", 2), collectionAccessor, innerExpression);

            // x => x.values.Where(y => y.property == value op y.property == value2 ..)
            return Expression.Lambda<Func<TEntity, IEnumerable<TItem>>>(memberCall, collectionAccessorExpression.Parameters);
        }

        private static Expression<Func<TEntity, bool>> CreateOrExpression<TEntity, TProperty>(ParameterExpression parameterExpression, MemberExpression memberExpression, ICollection<TProperty> collection)
        {
            Expression orExpression = null;

            foreach (TProperty item in collection)
            {
                Expression<Func<TProperty>> idLambda = () => item;

                BinaryExpression equalExpression = Expression.Equal(memberExpression, idLambda.Body);

                orExpression = orExpression != null ? Expression.OrElse(orExpression, equalExpression) : equalExpression;
            }

            return Expression.Lambda<Func<TEntity, bool>>(orExpression, parameterExpression);
        }

        private static Expression<Func<TEntity, bool>> CreateAndExpression<TEntity, TProperty>(ParameterExpression parameterExpression, MemberExpression memberExpression, ICollection<TProperty> collection)
        {
            Expression andExpression = null;

            foreach (TProperty item in collection)
            {
                Expression<Func<TProperty>> idLambda = () => item;

                BinaryExpression notEqualExpression = Expression.NotEqual(memberExpression, idLambda.Body);

                andExpression = andExpression != null ? Expression.And(andExpression, notEqualExpression) : notEqualExpression;
            }

            return Expression.Lambda<Func<TEntity, bool>>(andExpression, parameterExpression);
        }

        private static Expression<Func<TEntity, bool>> CreateContainsExpression<TEntity, TProperty>(
            ParameterExpression parameterExpression, MemberExpression memberExpression, ICollection<TProperty> collection)
        {
            ConstantExpression constantExpression = Expression.Constant(collection);
            MethodInfo containsMethodInfo = typeof(ICollection<TProperty>).GetMethod(nameof(ICollection<TProperty>.Contains));

            MethodCallExpression callExpression = Expression.Call(constantExpression, containsMethodInfo, memberExpression);

            return Expression.Lambda<Func<TEntity, bool>>(callExpression, parameterExpression);
        }

        private static Expression<Func<TEntity, bool>> CreateNotContainsExpression<TEntity, TProperty>(
          ParameterExpression parameterExpression, MemberExpression memberExpression, ICollection<TProperty> collection)
        {
            ConstantExpression constantExpression = Expression.Constant(collection);
            MethodInfo containsMethodInfo = typeof(ICollection<TProperty>).GetMethod(nameof(ICollection<TProperty>.Contains));

            MethodCallExpression callExpression = Expression.Call(constantExpression, containsMethodInfo, memberExpression);
            Expression notContainsExpression = Expression.Not(callExpression);

            return Expression.Lambda<Func<TEntity, bool>>(notContainsExpression, parameterExpression);
        }

        private static MethodInfo GetGenericMethodInfo<TEntity>(string methodName, int paramNumber)
        {
            var methods = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var method = methods.First(m => m.Name == methodName && m.GetParameters().Count() == paramNumber);
            return method.MakeGenericMethod(typeof(TEntity));
        }
    }
}