using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class OAuthKeysNotPresentException : Exception
    {
        public OAuthKeysNotPresentException()
        {
        }

        public OAuthKeysNotPresentException(string key) 
            : base(string.Format("Open Authentication Key '{0}' not present", key))
        {
        }

        public OAuthKeysNotPresentException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}