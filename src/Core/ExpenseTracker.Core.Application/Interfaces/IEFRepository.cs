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
        Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        List<TEntity> List();
        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Read();
        Task<int> SaveChangesAsync();
    }
}
