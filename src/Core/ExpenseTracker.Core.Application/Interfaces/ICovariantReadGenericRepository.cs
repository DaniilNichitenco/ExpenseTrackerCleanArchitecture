using System.Linq;
using ExpenseTracker.Core.Domain.Entities;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface ICovariantReadGenericRepository<out TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Get();
    }
}