namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository específico para Produtos
/// </summary>
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Product?> GetBySkuAsync(string sku, Guid companyId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Supplier)
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.Sku == sku && p.CompanyId == companyId);
    }

    public async Task<Product?> GetByBarcodeAsync(string barcode, Guid companyId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.Barcode == barcode && p.CompanyId == companyId && p.IsActive);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, Guid companyId, int page = 1, int pageSize = 10)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId && p.CompanyId == companyId && p.IsActive)
            .Include(p => p.Category)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10)
    {
        return await _dbSet
            .Where(p => (p.Name.Contains(searchTerm) || p.Sku.Contains(searchTerm) || p.Barcode!.Contains(searchTerm))
                && p.CompanyId == companyId
                && p.IsActive)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product?> GetBySkuAsync(string sku, Guid companyId);
    Task<Product?> GetByBarcodeAsync(string barcode, Guid companyId);
    Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, Guid companyId, int page = 1, int pageSize = 10);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10);
}
