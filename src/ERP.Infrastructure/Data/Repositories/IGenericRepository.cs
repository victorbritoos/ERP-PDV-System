namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Common;

/// <summary>
/// Interface genérica para repositories
/// </summary>
/// <typeparam name="TEntity">Tipo da entidade</typeparam>
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10);
    Task<int> GetCountAsync(Guid companyId);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task HardDeleteAsync(TEntity entity);
}
