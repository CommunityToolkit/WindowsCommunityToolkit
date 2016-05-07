using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class ConfigParameterNullException : Exception
    {
        public ConfigParameterNullException()
        {
        }

        public ConfigParameterNullException(string parameter) 
            : base(string.Format("The parameter '{0}' in config is null.", parameter))
        {
        }

        public ConfigParameterNullException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}