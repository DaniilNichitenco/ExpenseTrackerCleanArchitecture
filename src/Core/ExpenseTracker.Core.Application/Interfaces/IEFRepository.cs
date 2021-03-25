using ExpenseTracker.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Application.Interfaces
{
    public interface IEFRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity GetById(long id);
        Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken);
        List<TEntity> List();
        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Read();

    }
}
