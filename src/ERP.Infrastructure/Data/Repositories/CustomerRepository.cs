namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository específico para Clientes
/// </summary>
public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByDocumentAsync(string document, Guid companyId)
    {
        return await _dbSet
            .Include(c => c.DefaultPriceTable)
            .Include(c => c.PriceTables)
            .FirstOrDefaultAsync(c => c.Document == document && c.CompanyId == companyId && c.IsActive);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10)
    {
        return await _dbSet
            .Where(c => (c.Name.Contains(searchTerm) || c.Document!.Contains(searchTerm))
                && c.CompanyId == companyId
                && c.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByDocumentAsync(string document, Guid companyId);
    Task<IEnumerable<Customer>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10);
}
