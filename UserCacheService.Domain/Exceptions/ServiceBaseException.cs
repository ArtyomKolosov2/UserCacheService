using UserCacheService.Domain.Error;

namespace UserCacheService.Domain.Exceptions;

public abstract class ServiceBaseException : Exception
{
    public string ErrorMessage { get; set; }
    
    public int ErrorId { get; set; } 
    
    protected ServiceBaseException(string errorMessage, ErrorCode errorCode) : base(errorMessage)
    {
        ErrorMessage = errorMessage;
        ErrorId = (int)errorCode;
    }
}