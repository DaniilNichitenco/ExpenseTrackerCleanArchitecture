using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpenseTracker.Core.Application.Interfaces;
using ExpenseTracker.Core.Domain.Entities;
using ExpenseTracker.Infrastructure.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace ExpenseTracker.Infrastructure.Repository.API
{
    public class ReadGenericRepository<TEntity> : IReadGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ExpenseTrackerDbContext _context;

        public ReadGenericRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> GetByIds(ICollection<Guid> ids)
        {
            if (ids == null || ids.Any() == false)
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }

            return Get().Where(QueryExtensions.ContainsById<TEntity>(ids));
        }

        public IQueryable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] paths)
        {
            IQueryable<TEntity> queryable = Get();
            if (paths != null)
            {
                queryable = paths.Aggregate(queryable, (current, path) => current.Include(path));
            }

            return queryable;
        }

        public IQueryable<TEntity> GetWithIncludeOptimized(params Expression<Func<TEntity, object>>[] paths)
        {
            // IncludeOptimized requires caching
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (paths != null)
            {
                queryable = paths.Aggregate(queryable, (current, path) => current.IncludeOptimized(path));
            }

            return queryable;
        }

        public IQueryable<TEntity> GetWithInclude(IEnumerable<string> excludedProperties, params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return GetWithExcludeInclude(excludedProperties, false, includedPaths);
        }

        public IQueryable<TEntity> GetWithIncludeOptimized(IEnumerable<string> excludedProperties, params Expression<Func<TEntity, object>>[] includedPaths)
        {
            return GetWithExcludeInclude(excludedProperties, true, includedPaths);
        }

        public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] paths)
        {
            IQueryable<TEntity> withInclude = GetWithInclude(paths);
            return withInclude.Where(predicate);
        }

        private IQueryable<TEntity> GetWithExcludeInclude(IEnumerable<string> excludedProperties, bool useIncludeOptimized, params Expression<Func<TEntity, object>>[] includedPaths)
        {
            IQueryable<TEntity> queryable = _context.Set<TEntity>();
            if (!useIncludeOptimized)
            {
                queryable = queryable.AsNoTracking();
            }

            if (includedPaths != null)
            {
                queryable = includedPaths.Aggregate(queryable, (current, path) =>
                {
                    var methodCallExpression = path.Body as MethodCallExpression;
                    var memberExpression = path.Body as MemberExpression;
                    var pathString = "";
                    if (memberExpression != null)
                    {
                        pathString = GetIncludePath(memberExpression);
                    }
                    else if (methodCallExpression != null)
                    {
                        pathString = GetIncludePath(methodCallExpression);
                    }

                    if (excludedProperties == null || !excludedProperties.Any(x => pathString.StartsWith(x)))
                    {
                        return useIncludeOptimized ? current.IncludeOptimized(path) : current.Include(path);
                    }
                    return current;
                });
            }

            return queryable;
        }

        private static string GetIncludePath(MemberExpression memberExpression)
        {
            var path = "";
            if (memberExpression.Expression is MemberExpression expression)
            {
                path = GetIncludePath(expression) + ".";
            }
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            return path + propertyInfo.Name;
        }

        private static string GetIncludePath(MethodCallExpression methodCallExpression)
        {
            var path = "";
            var addDot = false;
            foreach (var argument in methodCallExpression.Arguments)
            {
                if (argument is MemberExpression expression)
                {
                    path += (addDot ? "." : "") + GetIncludePath(expression);
                    addDot = true;
                }
                else if (argument is LambdaExpression lambdaExpression)
                {
                    if (lambdaExpression.Body is MethodCallExpression expressionBody)
                    {
                        path += (addDot ? "." : "") + GetIncludePath(expressionBody);
                        addDot = true;
                    }
                    else if (lambdaExpression.Body is MemberExpression memberExpression)
                    {
                        path += (addDot ? "." : "") + GetIncludePath(memberExpression);
                        addDot = true;
                    }
                }
            }

            return path;
        }
    }
}