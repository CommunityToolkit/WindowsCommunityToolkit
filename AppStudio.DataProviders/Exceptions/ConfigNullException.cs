using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class ConfigNullException : Exception
    {
        public ConfigNullException()
        {
        }

        public ConfigNullException(string message) 
            : base(message)
        {
        }

        public ConfigNullException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}