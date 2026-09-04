namespace ERP.Infrastructure.Data.Repositories;

using ERP.Domain.Entities;
using ERP.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository específico para Preços de Produtos
/// </summary>
public class ProductPriceRepository : GenericRepository<ProductPrice>, IProductPriceRepository
{
    public ProductPriceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProductPrice?> GetCurrentPriceAsync(Guid productId, Guid priceTableId)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .FirstOrDefaultAsync(pp => 
                pp.ProductId == productId && 
                pp.PriceTableId == priceTableId && 
                pp.IsActive &&
                pp.EffectiveFrom <= now &&
                (pp.EffectiveTo == null || pp.EffectiveTo > now));
    }

    public async Task<IEnumerable<ProductPrice>> GetProductPricesAsync(Guid productId)
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(pp => 
                pp.ProductId == productId && 
                pp.IsActive &&
                pp.EffectiveFrom <= now &&
                (pp.EffectiveTo == null || pp.EffectiveTo > now))
            .Include(pp => pp.PriceTable)
            .ToListAsync();
    }
}

public interface IProductPriceRepository : IGenericRepository<ProductPrice>
{
    Task<ProductPrice?> GetCurrentPriceAsync(Guid productId, Guid priceTableId);
    Task<IEnumerable<ProductPrice>> GetProductPricesAsync(Guid productId);
}
