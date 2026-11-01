using System;

namespace PAP.Business.Core
{
    /// <summary>
    /// Error with a message that can be shown to the user as is.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
