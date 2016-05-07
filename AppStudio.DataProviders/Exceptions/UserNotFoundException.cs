using System;

namespace AppStudio.DataProviders.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string screenName)
            : base("User " + screenName + " not found.")
        {
        }

        public UserNotFoundException(string screenName, Exception innerException)
            : base("User " + screenName + " not found.", innerException)
        {
        }
    }
}
