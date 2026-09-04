namespace ERP.Application.Services.Customers;

using AutoMapper;
using ERP.Application.DTOs.Customer;
using ERP.Domain.Entities;
using ERP.Domain.Exceptions;
using ERP.Infrastructure.Data.Repositories;

/// <summary>
/// Serviço de gerenciamento de clientes
/// </summary>
public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto, Guid companyId, Guid userId);
    Task<CustomerDto> GetByIdAsync(Guid customerId, Guid companyId);
    Task<IEnumerable<CustomerListDto>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10);
    Task<IEnumerable<CustomerListDto>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10);
    Task<CustomerDto> UpdateAsync(Guid customerId, UpdateCustomerDto dto, Guid companyId, Guid userId);
    Task DeleteAsync(Guid customerId, Guid companyId, Guid userId);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto, Guid companyId, Guid userId)
    {
        // Validar se cliente com este documento já existe
        if (!string.IsNullOrEmpty(dto.Document))
        {
            var existingCustomer = await _repository.GetByDocumentAsync(dto.Document, companyId);
            if (existingCustomer != null)
                throw new BusinessException($"Cliente com este documento já existe", "CUSTOMER_ALREADY_EXISTS");
        }

        var customer = _mapper.Map<Customer>(dto);
        customer.CompanyId = companyId;
        customer.CreatedBy = userId;
        customer.UpdatedBy = userId;

        var created = await _repository.AddAsync(customer);
        return _mapper.Map<CustomerDto>(created);
    }

    public async Task<CustomerDto> GetByIdAsync(Guid customerId, Guid companyId)
    {
        var customer = await _repository.GetByIdAsync(customerId);
        if (customer == null || customer.CompanyId != companyId)
            throw new NotFoundException($"Cliente não encontrado");

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerListDto>> GetAllAsync(Guid companyId, int page = 1, int pageSize = 10)
    {
        var customers = await _repository.GetAllAsync(companyId, page, pageSize);
        return _mapper.Map<IEnumerable<CustomerListDto>>(customers);
    }

    public async Task<IEnumerable<CustomerListDto>> SearchAsync(string searchTerm, Guid companyId, int page = 1, int pageSize = 10)
    {
        var customers = await _repository.SearchAsync(searchTerm, companyId, page, pageSize);
        return _mapper.Map<IEnumerable<CustomerListDto>>(customers);
    }

    public async Task<CustomerDto> UpdateAsync(Guid customerId, UpdateCustomerDto dto, Guid companyId, Guid userId)
    {
        var customer = await _repository.GetByIdAsync(customerId);
        if (customer == null || customer.CompanyId != companyId)
            throw new NotFoundException($"Cliente não encontrado");

        _mapper.Map(dto, customer);
        customer.UpdatedBy = userId;
        customer.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(updated);
    }

    public async Task DeleteAsync(Guid customerId, Guid companyId, Guid userId)
    {
        var customer = await _repository.GetByIdAsync(customerId);
        if (customer == null || customer.CompanyId != companyId)
            throw new NotFoundException($"Cliente não encontrado");

        customer.IsActive = false;
        customer.UpdatedBy = userId;
        customer.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(customer);
    }
}
