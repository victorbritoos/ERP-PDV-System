namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository específico para Tabelas de Preço
/// </summary>
public class PriceTableRepository : GenericRepository<PriceTable>, IPriceTableRepository
{
    public PriceTableRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PriceTable?> GetByNameAsync(string name, Guid companyId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pt => pt.Name == name && pt.CompanyId == companyId && pt.IsActive);
    }

    public async Task<PriceTable?> GetDefaultAsync(Guid companyId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pt => pt.IsDefault && pt.CompanyId == companyId && pt.IsActive);
    }

    public async Task<IEnumerable<PriceTable>> GetAllActiveAsync(Guid companyId)
    {
        return await _dbSet
            .Where(pt => pt.CompanyId == companyId && pt.IsActive)
            .OrderBy(pt => pt.Priority)
            .ToListAsync();
    }
}

public interface IPriceTableRepository : IGenericRepository<PriceTable>
{
    Task<PriceTable?> GetByNameAsync(string name, Guid companyId);
    Task<PriceTable?> GetDefaultAsync(Guid companyId);
    Task<IEnumerable<PriceTable>> GetAllActiveAsync(Guid companyId);
}
