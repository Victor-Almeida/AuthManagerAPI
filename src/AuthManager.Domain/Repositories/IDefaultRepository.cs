using AuthManager.Domain.Primitives;

namespace AuthManager.Domain.Repositories;

public interface IDefaultRepository
{
    void Add<TEntity>(TEntity entity) where TEntity : Entity;
    IQueryable<TEntity> Query<TEntity>() where TEntity : Entity;
    void Remove<TEntity>(TEntity entity) where TEntity : Entity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void Update<TEntity>(TEntity entity) where TEntity : Entity;
}