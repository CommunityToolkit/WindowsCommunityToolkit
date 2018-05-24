// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Defines errors encountered during operations involving web services, such as authentication, proxy configuration, and destination URIs.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.WebErrorStatus"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.WebErrorStatus" />
    public enum WebErrorStatus
    {
        /// <summary>
        /// An unknown error has occurred.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The SSL certificate common name does not match the web address.
        /// </summary>
        CertificateCommonNameIsIncorrect = 1,

        /// <summary>
        /// The SSL certificate has expired.
        /// </summary>
        CertificateExpired = 2,

        /// <summary>
        /// The SSL certificate contains errors.
        /// </summary>
        CertificateContainsErrors = 3,

        /// <summary>
        /// The SSL certificate has been revoked.
        /// </summary>
        CertificateRevoked = 4,

        /// <summary>
        /// The SSL certificate is invalid.
        /// </summary>
        CertificateIsInvalid = 5,

        /// <summary>
        /// The server is not responding.
        /// </summary>
        ServerUnreachable = 6,

        /// <summary>
        /// The connection has timed out.
        /// </summary>
        Timeout = 7,

        /// <summary>
        /// The server returned an invalid or unrecognized response.
        /// </summary>
        ErrorHttpInvalidServerResponse = 8,

        /// <summary>
        /// The connection was aborted.
        /// </summary>
        ConnectionAborted = 9,

        /// <summary>
        /// The connection was reset.
        /// </summary>
        ConnectionReset = 10,

        /// <summary>
        /// The connection was ended.
        /// </summary>
        Disconnected = 11,

        /// <summary>
        /// Redirected from a location to a secure location.
        /// </summary>
        HttpToHttpsOnRedirection = 12,

        /// <summary>
        /// Redirected from a secure location to an unsecure location.
        /// </summary>
        HttpsToHttpOnRedirection = 13,

        /// <summary>
        /// Cannot connect to destination.
        /// </summary>
        CannotConnect = 14,

        /// <summary>
        /// Could not resolve provided host name.
        /// </summary>
        HostNameNotResolved = 15,

        /// <summary>
        /// The operation was canceled.
        /// </summary>
        OperationCanceled = 16,

        /// <summary>
        /// The request redirect failed.
        /// </summary>
        RedirectFailed = 17,

        /// <summary>
        /// An unexpected status code indicating a failure was received.
        /// </summary>
        UnexpectedStatusCode = 18,

        /// <summary>
        /// A request was unexpectedly redirected.
        /// </summary>
        UnexpectedRedirection = 19,

        /// <summary>
        /// An unexpected client-side error has occurred.
        /// </summary>
        UnexpectedClientError = 20,

        /// <summary>
        /// An unexpected server-side error has occurred.
        /// </summary>
        UnexpectedServerError = 21,

        /// <summary>
        /// The request does not support the range.
        /// </summary>
        InsufficientRangeSupport = 22,

        /// <summary>
        /// The request is mising the file size.
        /// </summary>
        MissingContentLengthSupport = 23,

        /// <summary>
        /// The requested URL represents a high level grouping of which lower level selections need to be made.
        /// </summary>
        MultipleChoices = 300,

        /// <summary>
        /// This and all future requests should be directed to the given URI.
        /// </summary>
        MovedPermanently = 301,

        /// <summary>
        /// The resource was found but is available in a location different from the one included in the request.
        /// </summary>
        Found = 302,

        /// <summary>
        /// The response to the request can be found under another URI using a GET method.
        /// </summary>
        SeeOther = 303,

        /// <summary>
        /// Indicates the resource has not been modified since last requested.
        /// </summary>
        NotModified = 304,

        /// <summary>
        /// The requested resource must be accessed through the proxy given by the Location field.
        /// </summary>
        UseProxy = 305,

        /// <summary>
        /// The requested resource resides temporarily under a different URI.
        /// </summary>
        TemporaryRedirect = 307,

        /// <summary>
        /// The request cannot be fulfilled due to bad syntax.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// Authentication has failed or credentials have not yet been provided.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// Reserved.
        /// </summary>
        PaymentRequired = 402,

        /// <summary>
        /// The server has refused the request.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// The requested resource could not be found but may be available again in the future.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// A request was made of a resource using a request method not supported by that resource.
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// The requested resource is only capable of generating content not acceptable according to the Accept headers sent in the request.
        /// </summary>
        NotAcceptable = 406,

        /// <summary>
        /// The client must first authenticate itself with the proxy.
        /// </summary>
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// The server timed out waiting for the request.
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// Indicates that the request could not be processed because of conflict in the request.
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// Indicates that the resource requested is no longer available and will not be available again.
        /// </summary>
        Gone = 410,

        /// <summary>
        /// The request did not specify the length of its content, which is required by the requested resource.
        /// </summary>
        LengthRequired = 411,

        /// <summary>
        /// The server does not meet one of the preconditions that the requester put on the request.
        /// </summary>
        PreconditionFailed = 412,

        /// <summary>
        /// The request is larger than the server is willing or able to process.
        /// </summary>
        RequestEntityTooLarge = 413,

        /// <summary>
        /// Provided URI length exceeds the maximum length the server can process.
        /// </summary>
        RequestUriTooLong = 414,

        /// <summary>
        /// The request entity has a media type which the server or resource does not support.
        /// </summary>
        UnsupportedMediaType = 415,

        /// <summary>
        /// The client has asked for a portion of the file, but the server cannot supply that portion.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Satisfiable", Justification="Spelling of WinRT type")]
        RequestedRangeNotSatisfiable = 416,

        /// <summary>
        /// The server cannot meet the requirements of the Expect request-header field.
        /// </summary>
        ExpectationFailed = 417,

        /// <summary>
        /// A generic error message, given when no more specific message is suitable.
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// The server either does not recognize the request method, or it lacks the ability to fulfill the request.
        /// </summary>
        NotImplemented = 501,

        /// <summary>
        /// The server was acting as a gateway or proxy and received an invalid response from the upstream server.
        /// </summary>
        BadGateway = 502,

        /// <summary>
        /// The server is currently unavailable.
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.
        /// </summary>
        GatewayTimeout = 504,

        /// <summary>
        /// The server does not support the HTTP protocol version used in the request.
        /// </summary>
        HttpVersionNotSupported = 505,
    }
}