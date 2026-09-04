# Guia de Desenvolvimento

## Configuração do Ambiente

### Pré-requisitos

1. **.NET 8 SDK** - Download em https://dotnet.microsoft.com/download
2. **Visual Studio 2022** - Community versão grátis
3. **SQL Server** - LocalDB (incluído no VS) ou Express
4. **Git** - Para versionamento
5. **Postman/Insomnia** - Para testar API (opcional)

### Configurar User Secrets (para dados sensíveis)

```bash
cd src/ERP.API

# Inicializar user secrets
dotnet user-secrets init

# Adicionar connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\mssqllocaldb;Database=ErpPdvDb;Trusted_Connection=true;TrustServerCertificate=true;"

# Adicionar JWT Secret
dotnet user-secrets set "Jwt:Secret" "sua-chave-super-secreta-com-32-caracteres-minimo"
```

## Estrutura de Código

### Domain (Camada de Domínio)

Contém lógica de negócio pura, independente de frameworks.

```csharp
// Exemplo: Entity
public class Product : BaseEntity
{
    public string Sku { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal StockMinimum { get; set; }
    public decimal StockMaximum { get; set; }
    public decimal CurrentStock { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    
    public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
}

// Exemplo: Value Object
public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    
    public Money(decimal amount, string currency = "BRL")
    {
        Amount = amount;
        Currency = currency;
    }
}

// Exemplo: Enum
public enum MovementType
{
    Entry = 1,
    Exit = 2,
    Adjustment = 3,
    Transfer = 4
}
```

### Application (Camada de Aplicação)

Contém orquestração, validações e DTOs.

```csharp
// Exemplo: DTO
public class CreateProductDto
{
    public string Sku { get; set; }
    public string Barcode { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public decimal CostPrice { get; set; }
}

// Exemplo: Validator
public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU é obrigatório")
            .Length(3, 50).WithMessage("SKU deve ter entre 3 e 50 caracteres");
            
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres");
            
        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithMessage("Preço de custo deve ser maior que 0");
    }
}

// Exemplo: Service
public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto, Guid companyId, Guid userId);
    Task<ProductDto> GetByIdAsync(Guid productId, Guid companyId);
    Task<IEnumerable<ProductDto>> GetAllAsync(Guid companyId, int page, int pageSize);
    Task UpdateAsync(Guid productId, UpdateProductDto dto, Guid companyId, Guid userId);
    Task DeleteAsync(Guid productId, Guid companyId, Guid userId);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    
    public ProductService(IProductRepository repository, IMapper mapper, IAuditService auditService)
    {
        _repository = repository;
        _mapper = mapper;
        _auditService = auditService;
    }
    
    public async Task<ProductDto> CreateAsync(CreateProductDto dto, Guid companyId, Guid userId)
    {
        // Validações
        var existingSku = await _repository.FindBySku(dto.Sku, companyId);
        if (existingSku != null)
            throw new BusinessException("Produto com este SKU já existe");
        
        // Criar entidade
        var product = new Product
        {
            Sku = dto.Sku,
            Name = dto.Name,
            // ... demais campos
        };
        
        // Salvar
        await _repository.AddAsync(product);
        
        // Auditar
        await _auditService.LogAsync(new AuditLog
        {
            EntityName = nameof(Product),
            EntityId = product.Id.ToString(),
            Action = "CREATE",
            NewValues = JsonSerializer.Serialize(product),
            UserId = userId,
            Timestamp = DateTime.UtcNow
        });
        
        return _mapper.Map<ProductDto>(product);
    }
}
```

### Infrastructure (Camada de Infraestrutura)

Implementação de repositories, EF Core, integrações externas.

```csharp
// Exemplo: DbContext
public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductPrice> ProductPrices { get; set; }
    public DbSet<Sale> Sales { get; set; }
    // ... demais DbSets
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configurações de mapeamento
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

// Exemplo: Entity Configuration
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(p => p.Barcode)
            .HasMaxLength(50);
            
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.CostPrice)
            .HasPrecision(18, 2);
        
        builder.HasIndex(p => new { p.CompanyId, p.Sku }).IsUnique();
        builder.HasIndex(p => new { p.CompanyId, p.Barcode }).IsUnique();
        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.IsActive);
        
        builder.HasMany(p => p.Prices)
            .WithOne(pp => pp.Product)
            .HasForeignKey(pp => pp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

// Exemplo: Repository
public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<Product> FindBySkuAsync(string sku, Guid companyId);
    Task<IEnumerable<Product>> GetAllAsync(Guid companyId, int page, int pageSize);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    
    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<Product> FindBySkuAsync(string sku, Guid companyId)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == sku && p.CompanyId == companyId);
    }
    
    // ... demais métodos
}
```

### API (Camada de API)

Controllers, middlewares, autenticação.

```csharp
// Exemplo: Controller
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;
    
    public ProductsController(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        try
        {
            var companyId = User.FindFirst("CompanyId");
            var product = await _service.GetByIdAsync(id, Guid.Parse(companyId.Value));
            return Ok(product);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    [HttpPost]
    [Authorize(Policy = "Products.Create")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        var companyId = User.FindFirst("CompanyId");
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        
        var product = await _service.CreateAsync(dto, Guid.Parse(companyId.Value), Guid.Parse(userId.Value));
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
    
    [HttpPut("{id}")]
    [Authorize(Policy = "Products.Edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        var companyId = User.FindFirst("CompanyId");
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        
        await _service.UpdateAsync(id, dto, Guid.Parse(companyId.Value), Guid.Parse(userId.Value));
        return NoContent();
    }
}

// Exemplo: Global Exception Handler Middleware
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }
    
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new { message = "An error occurred. Please try again later." };
        
        switch (exception)
        {
            case BusinessException ex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new { message = ex.Message };
                break;
                
            case NotFoundException ex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { message = ex.Message };
                break;
                
            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response = new { message = "Access denied" };
                break;
                
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }
        
        return context.Response.WriteAsJsonAsync(response);
    }
}
```

## Padrões de Código

### Naming Conventions

```csharp
// Classes
public class ProductService { }
public interface IProductRepository { }

// Métodos
public async Task<Product> GetByIdAsync(Guid id) { }
public void ValidatePrice(decimal price) { }

// Constantes
private const int MaxProductNameLength = 200;

// Variáveis privadas
private readonly IProductRepository _repository;
private int _count = 0;
```

### Async/Await

Todas operações I/O devem ser assíncronas:

```csharp
// ✅ Correto
public async Task<Product> GetAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ Evitar
public Product Get(Guid id)
{
    return _repository.GetById(id).Result; // Pode causar deadlock
}
```

### Transações

Operações críticas devem usar transações:

```csharp
using (var transaction = await _context.Database.BeginTransactionAsync())
{
    try
    {
        await _repository.AddAsync(sale);
        await _repository.UpdateStockAsync(product, -quantity);
        await _repository.AddCashMovementAsync(cashMovement);
        
        await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

### DTOs vs Entities

```csharp
// ❌ Nunca retornar entity diretamente
public IActionResult GetProduct(Guid id)
{
    return Ok(_repository.GetById(id)); // ERRADO
}

// ✅ Sempre mapear para DTO
public IActionResult GetProduct(Guid id)
{
    var product = _repository.GetById(id);
    return Ok(_mapper.Map<ProductDto>(product)); // CORRETO
}
```

## Testes

### Testes Unitários

```csharp
public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _service;
    
    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new ProductService(_repositoryMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_WithValidDto_ShouldCreateProduct()
    {
        // Arrange
        var dto = new CreateProductDto { Name = "Test", Sku = "SKU123" };
        var product = new Product { Id = Guid.NewGuid(), Name = "Test" };
        
        _repositoryMock.Setup(r => r.FindBySkuAsync(It.IsAny<string>(), It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);
        
        // Act
        var result = await _service.CreateAsync(dto, Guid.NewGuid(), Guid.NewGuid());
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}
```

### Testes de Integração

```csharp
public class ProductIntegrationTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", GenerateToken());
    }
    
    [Fact]
    public async Task CreateProduct_WithValidData_ShouldReturn201()
    {
        var dto = new CreateProductDto { Name = "Test Product", Sku = "SKU123" };
        var content = new StringContent(
            JsonSerializer.Serialize(dto),
            Encoding.UTF8,
            "application/json");
        
        var response = await _client.PostAsync("/api/products", content);
        
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }
    
    public async Task DisposeAsync()
    {
        _factory?.Dispose();
        _client?.Dispose();
    }
}
```

## Git Workflow

### Branches

```
main (produção)
  ↑
develop (desenvolvimento)
  ↑
feature/nome-da-funcionalidade
hotfix/nome-do-bug
```

### Commits

```bash
# Formato
git commit -m "[TIPO] descrição breve"

# Tipos
feat:    Nova funcionalidade
fix:     Correção de bug
refactor:Reorganização de código
test:    Adição/alteração de testes
docs:    Atualização de documentação
style:   Formatação de código
chore:   Tarefas auxiliares

# Exemplos
git commit -m "feat: adicionar motor de preços"
git commit -m "fix: corrigir cálculo de estoque"
git commit -m "test: adicionar testes de autenticação"
```

## Variáveis de Ambiente

### Development

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ErpPdvDb_Dev;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Secret": "sua-chave-super-secreta-com-32-caracteres-minimo",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

### Production

Usar User Secrets ou variáveis de ambiente do servidor.

## Checklist de Desenvolvimento

Antes de fazer commit:

- [ ] Código compilado sem erros
- [ ] Testes passando
- [ ] Sem warnings (ou documentados)
- [ ] Code review realizado
- [ ] Documentação atualizada
- [ ] Commit message descritivo
- [ ] Branch atualizada com `develop`

---

**Última atualização**: Setembro 2026
