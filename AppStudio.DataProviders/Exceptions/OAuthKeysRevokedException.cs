using System;

namespace AppStudio.DataProviders.Exceptions
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
