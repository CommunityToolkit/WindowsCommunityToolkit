using System;

namespace Microsoft.Windows.Toolkit.Services.Exceptions
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