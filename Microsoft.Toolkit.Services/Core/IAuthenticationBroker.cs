using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Core
{
    public interface IStorageManager
    {
        string Get(string key);
        void Set(string key, string value);
    }

    public interface IPasswordManager
    {
        PasswordCredential Get(string key);
        void Store(string resource, PasswordCredential credential);
        void Remove(string key);
    }

    public class PasswordCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


    /// <summary>
    /// TODO
    /// </summary>
    public interface IAuthenticationBroker
    {
        Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri);
    }

    /// <summary>
    /// TODO
    /// </summary>
    public class AuthenticationResult
    {
        public uint ResponseErrorDetail { get; set; }

        public string ResponseData { get; set; }

        public AuthenticationResultStatus ResponseStatus { get; set; }

    }

    /// <summary>
    /// Contains the status of the authentication operation
    /// </summary>
    public enum AuthenticationResultStatus
    {
        /// <summary>
        /// The operation succeeded, and the response data is available.
        /// </summary>
        Success,

        /// <summary>
        /// The operation was canceled by the user
        /// </summary>
        UserCancel,

        /// <summary>
        /// The operation failed because a specific HTTP error was returned, for example 404
        /// </summary>
        ErrorHttp
    }
}
