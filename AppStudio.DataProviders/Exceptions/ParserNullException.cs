using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class ParserNullException : Exception
    {
        public ParserNullException()
        {
        }

        public ParserNullException(string message) 
            : base(message)
        {
        }

        public ParserNullException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}