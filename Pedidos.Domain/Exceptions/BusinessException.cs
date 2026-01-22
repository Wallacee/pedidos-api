namespace Pedidos.Domain.Exceptions
{
    public abstract class BusinessException : Exception
    {
        public string ErrorCode { get; }

        protected BusinessException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }

}
