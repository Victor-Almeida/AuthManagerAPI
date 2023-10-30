using AuthManager.Domain.Primitives;
using AuthManager.Domain.Repositories;
using AuthManager.Persistence.Contexts;

namespace AuthManager.Persistence.Repositories;

internal class DefaultRepository : IDefaultRepository
{
    private readonly DefaultDbContext _context;

    public DefaultRepository(DefaultDbContext context)
    {
        _context = context;
    }

    public void Add<TEntity>(TEntity entity) where TEntity : Entity => _context.Set<TEntity>().Add(entity);
    public IQueryable<TEntity> Query<TEntity>() where TEntity : Entity => _context.Set<TEntity>();
    public void Remove<TEntity>(TEntity entity) where TEntity : Entity => _context.Set<TEntity>().Remove(entity);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
    public void Update<TEntity>(TEntity entity) where TEntity : Entity => _context.Set<TEntity>().Update(entity);
}