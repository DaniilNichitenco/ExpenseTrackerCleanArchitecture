using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface IReadGenericRepository<TEntity> : ICovariantReadGenericRepository<TEntity>
        where TEntity : BaseEntity

    {
    IQueryable<TEntity> GetByIds(ICollection<Guid> ids);
    IQueryable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] paths);

    IQueryable<TEntity> GetWithInclude(IEnumerable<string> excludedProperties,
        params Expression<Func<TEntity, object>>[] includedPaths);

    IQueryable<TEntity> GetWithIncludeOptimized(params Expression<Func<TEntity, object>>[] paths);

    IQueryable<TEntity> GetWithIncludeOptimized(IEnumerable<string> excludedProperties,
        params Expression<Func<TEntity, object>>[] includedPaths);

    IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] paths);
    }
}