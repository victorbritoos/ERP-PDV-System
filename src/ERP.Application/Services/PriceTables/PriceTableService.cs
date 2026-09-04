namespace ERP.Application.Services.PriceTables;

using AutoMapper;
using ERP.Application.DTOs.PriceTable;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;

/// <summary>
/// Serviço de gerenciamento de tabelas de preço
/// </summary>
public interface IPriceTableService
{
    Task<PriceTableDto> CreateAsync(CreatePriceTableDto dto, Guid companyId, Guid userId);
    Task<PriceTableDto> GetByIdAsync(Guid priceTableId, Guid companyId);
    Task<IEnumerable<PriceTableListDto>> GetAllAsync(Guid companyId);
    Task<PriceTableDto> UpdateAsync(Guid priceTableId, UpdatePriceTableDto dto, Guid companyId, Guid userId);
    Task DeleteAsync(Guid priceTableId, Guid companyId, Guid userId);
}

public class PriceTableService : IPriceTableService
{
    private readonly IPriceTableRepository _repository;
    private readonly IMapper _mapper;

    public PriceTableService(IPriceTableRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PriceTableDto> CreateAsync(CreatePriceTableDto dto, Guid companyId, Guid userId)
    {
        // Validar se tabela com este nome já existe
        var existingTable = await _repository.GetByNameAsync(dto.Name, companyId);
        if (existingTable != null)
            throw new BusinessException($"Tabela de preço '{dto.Name}' já existe", "PRICE_TABLE_ALREADY_EXISTS");

        var priceTable = _mapper.Map<PriceTable>(dto);
        priceTable.CompanyId = companyId;
        priceTable.CreatedBy = userId;
        priceTable.UpdatedBy = userId;

        var created = await _repository.AddAsync(priceTable);
        return _mapper.Map<PriceTableDto>(created);
    }

    public async Task<PriceTableDto> GetByIdAsync(Guid priceTableId, Guid companyId)
    {
        var priceTable = await _repository.GetByIdAsync(priceTableId);
        if (priceTable == null || priceTable.CompanyId != companyId)
            throw new NotFoundException($"Tabela de preço não encontrada");

        return _mapper.Map<PriceTableDto>(priceTable);
    }

    public async Task<IEnumerable<PriceTableListDto>> GetAllAsync(Guid companyId)
    {
        var tables = await _repository.GetAllActiveAsync(companyId);
        return _mapper.Map<IEnumerable<PriceTableListDto>>(tables);
    }

    public async Task<PriceTableDto> UpdateAsync(Guid priceTableId, UpdatePriceTableDto dto, Guid companyId, Guid userId)
    {
        var priceTable = await _repository.GetByIdAsync(priceTableId);
        if (priceTable == null || priceTable.CompanyId != companyId)
            throw new NotFoundException($"Tabela de preço não encontrada");

        _mapper.Map(dto, priceTable);
        priceTable.UpdatedBy = userId;
        priceTable.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(priceTable);
        return _mapper.Map<PriceTableDto>(updated);
    }

    public async Task DeleteAsync(Guid priceTableId, Guid companyId, Guid userId)
    {
        var priceTable = await _repository.GetByIdAsync(priceTableId);
        if (priceTable == null || priceTable.CompanyId != companyId)
            throw new NotFoundException($"Tabela de preço não encontrada");

        priceTable.IsActive = false;
        priceTable.UpdatedBy = userId;
        priceTable.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(priceTable);
    }
}
