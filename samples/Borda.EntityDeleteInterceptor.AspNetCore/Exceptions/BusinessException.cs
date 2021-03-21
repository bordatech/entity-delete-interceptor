using System;

namespace Borda.EntityDeleteInterceptor.AspNetCore.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }
    }
}