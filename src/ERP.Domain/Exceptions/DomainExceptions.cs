namespace ERP.Domain.Exceptions;

/// <summary>
/// Exceção customizada para erros de negócio
/// </summary>
public class BusinessException : Exception
{
    public string ErrorCode { get; set; }

    public BusinessException(string message, string errorCode = "BUSINESS_ERROR") : base(message)
    {
        ErrorCode = errorCode;
    }
}

/// <summary>
/// Exceção para recursos não encontrados
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>
/// Exceção para erros de validação
/// </summary>
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors) : base("Validation failed")
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }
}

/// <summary>
/// Exceção para acesso negado
/// </summary>
public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message = "Access denied") : base(message) { }
}
