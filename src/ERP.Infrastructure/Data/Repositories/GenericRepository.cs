namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Common;
using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Implementação genérica do padrão Repository
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10)
    {
        return await _dbSet
            .Where(e => e.CompanyId == companyId && e.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task<int> GetCountAsync(Guid companyId)
    {
        return await _dbSet.CountAsync(e => e.CompanyId == companyId && e.IsActive);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        // Soft delete
        entity.IsActive = false;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task HardDeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
