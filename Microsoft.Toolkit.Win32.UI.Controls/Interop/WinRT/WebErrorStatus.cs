// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>Defines errors encountered during operations involving web services, such as authentication, proxy configuration, and destination URIs.</summary>
    /// <remarks>Copy from <see cref="Windows.Web.WebErrorStatus"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.WebErrorStatus"/>
    public enum WebErrorStatus
    {
#pragma warning disable 1591
        Unknown = 0,
        CertificateCommonNameIsIncorrect = 1,
        CertificateExpired = 2,
        CertificateContainsErrors = 3,
        CertificateRevoked = 4,
        CertificateIsInvalid = 5,
        ServerUnreachable = 6,
        Timeout = 7,
        ErrorHttpInvalidServerResponse = 8,
        ConnectionAborted = 9,
        ConnectionReset = 10,
        Disconnected = 11,
        HttpToHttpsOnRedirection = 12,
        HttpsToHttpOnRedirection = 13,
        CannotConnect = 14,
        HostNameNotResolved = 15,
        OperationCanceled = 16,
        RedirectFailed = 17,
        UnexpectedStatusCode = 18,
        UnexpectedRedirection = 19,
        UnexpectedClientError = 20,
        UnexpectedServerError = 21,
        InsufficientRangeSupport = 22,
        MissingContentLengthSupport = 23,
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Satisfiable")]
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,
#pragma warning restore 1591
    }
}