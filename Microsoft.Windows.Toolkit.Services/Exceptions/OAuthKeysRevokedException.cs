using System;

namespace Microsoft.Windows.Toolkit.Services.Exceptions
{
    public class OAuthKeysRevokedException : Exception
    {
        public OAuthKeysRevokedException()
        {
        }

        public OAuthKeysRevokedException(string message)
            : base(message)
        {
        }

        public OAuthKeysRevokedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
