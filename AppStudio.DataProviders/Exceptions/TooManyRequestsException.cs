using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException()
        {
        }

        public TooManyRequestsException(string message)
            : base(message)
        {
        }

        public TooManyRequestsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
