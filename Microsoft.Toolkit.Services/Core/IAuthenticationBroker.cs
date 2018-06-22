using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Core
{
    public interface ISignatureManager
    {
        string GetSignature(string baseSignature, string secret, bool append = false);
    }

    /// <summary>
    /// This gets an Uri value.
    /// </summary>
    public interface IAuthenticationBroker
    {
        /// <summary>
        /// Returns the authentication status, it could be UserCancel, ErrorHttp and Success.
        /// </summary>
        /// <param name="requestUri"> Autorization base url</param>
        /// <param name="callbackUri"> LinkedInOAuthTokens callbackUri</param>
        /// <returns> Returns a status </returns>
        Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri);
    }

    /// <summary>
    /// This interface store the key value
    /// </summary>
    public interface IStorageManager
    {
        /// <summary>
        /// Gets the key value
        /// </summary>
        /// <param name="key"> Token value </param>
        /// <returns> Returns a string value</returns>
        string Get(string key);

        /// <summary>
        /// Sets the key value
        /// </summary>
        /// <param name="key"> Token key </param>
        /// <param name="value"> String value </param>
        void Set(string key, string value);
    }

    /// <summary>
    /// This interface gets a PasswordCredential, store the credential and remove the key.
    /// </summary>
    public interface IPasswordManager
    {
        /// <summary>
        /// Gets the user credentials.
        /// </summary>
        /// <param name="key"> Receive the storage key user and the access token </param>
        /// <returns> Returns user credential.</returns>
        PasswordCredential Get(string key);

        /// <summary>
        /// Store users credential.
        /// </summary>
        /// <param name="resource"> Resource</param>
        /// <param name="credential"> Username and password.</param>
        void Store(string resource, PasswordCredential credential);

        /// <summary>
        /// Remove users credential.
        /// </summary>
        /// <param name="key"> Credential unique key</param>
        void Remove(string key);
    }

    /// <summary>
    /// AuthenticationResult class, parameters: ResponseErrorDetail(uint), ResponseData(string) and ResponseStatus(AuthenticationResultStatus)
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Gets or sets the authentication error detail
        /// </summary>
        public uint ResponseErrorDetail { get; set; }

        /// <summary>
        /// Gets or sets the authentication result data
        /// </summary>
        public string ResponseData { get; set; }

        /// <summary>
        /// Gets or sets the authentication status, could be UserCancel, ErrorHttp and Success.
        /// </summary>
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
