namespace StockAvaibleTest_API.Common
{
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string message, string errorCode = "BUSINESS_ERROR") 
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message) 
            : base(message, "NOT_FOUND")
        {
        }
    }

    public class ValidationException : BusinessException
    {
        public ValidationException(string message) 
            : base(message, "VALIDATION_ERROR")
        {
        }
    }

    public class InsufficientStockException : BusinessException
    {
        public InsufficientStockException(string message) 
            : base(message, "INSUFFICIENT_STOCK")
        {
        }
    }
}
