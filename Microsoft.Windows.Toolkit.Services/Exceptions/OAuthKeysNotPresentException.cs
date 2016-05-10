using System;

namespace Microsoft.Windows.Toolkit.Services.Exceptions
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