// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.Uwp.Services.OAuth;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// OAuth request builder.
    /// </summary>
    internal class TwitterOAuthRequestBuilder
    {
        /// <summary>
        /// Realm for request.
        /// </summary>
        public const string Realm = "Twitter API";

        /// <summary>
        /// Gets or sets HTTP verb for request.
        /// </summary>
        public string Verb { get; set; }

        /// <summary>
        /// Gets encoded Request Uri.
        /// </summary>
        public Uri EncodedRequestUri { get; private set; }

        /// <summary>
        /// Gets request Uri without query.
        /// </summary>
        public Uri RequestUriWithoutQuery { get; private set; }

        /// <summary>
        /// Gets list of query parameters.
        /// </summary>
        public IEnumerable<OAuthParameter> QueryParams { get; private set; }

        /// <summary>
        /// Gets version.
        /// </summary>
        public OAuthParameter Version { get; private set; }

        /// <summary>
        /// Gets nonce.
        /// </summary>
        public OAuthParameter Nonce { get; private set; }

        /// <summary>
        /// Gets timestamp.
        /// </summary>
        public OAuthParameter Timestamp { get; private set; }

        /// <summary>
        /// Gets signature method.
        /// </summary>
        public OAuthParameter SignatureMethod { get; private set; }

        /// <summary>
        /// Gets consumer key.
        /// </summary>
        public OAuthParameter ConsumerKey { get; private set; }

        /// <summary>
        /// Gets consumer secret.
        /// </summary>
        public OAuthParameter ConsumerSecret { get; private set; }

        /// <summary>
        /// Gets access token.
        /// </summary>
        public OAuthParameter Token { get; private set; }

        /// <summary>
        /// Gets access token secret.
        /// </summary>
        public OAuthParameter TokenSecret { get; private set; }

        /// <summary>
        /// Gets signature getter.
        /// </summary>
        public OAuthParameter Signature => new OAuthParameter("oauth_signature", GenerateSignature());

        /// <summary>
        /// Gets authorization header getter.
        /// </summary>
        public string AuthorizationHeader => GenerateAuthorizationHeader();

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterOAuthRequestBuilder"/> class.
        /// Authorization request builder.
        /// </summary>
        /// <param name="requestUri">Request Uri.</param>
        /// <param name="tokens">Tokens to form request.</param>
        /// <param name="method">Method to use with request.</param>
        public TwitterOAuthRequestBuilder(Uri requestUri, TwitterOAuthTokens tokens, string method = "GET")
        {
            Verb = method;

            RequestUriWithoutQuery = new Uri(requestUri.AbsoluteWithoutQuery());

            if (!string.IsNullOrEmpty(requestUri.Query))
            {
                QueryParams = requestUri.GetQueryParams()
                    .Select(p => new OAuthParameter(p.Key, Uri.UnescapeDataString(p.Value)))
                    .ToList();
            }
            else
            {
                QueryParams = new List<OAuthParameter>();
            }

            EncodedRequestUri = GetEncodedUri(requestUri, QueryParams);

            Version = new OAuthParameter("oauth_version", "1.0");
            Nonce = new OAuthParameter("oauth_nonce", GenerateNonce());
            Timestamp = new OAuthParameter("oauth_timestamp", GenerateTimeStamp());
            SignatureMethod = new OAuthParameter("oauth_signature_method", "HMAC-SHA1");
            ConsumerKey = new OAuthParameter("oauth_consumer_key", tokens.ConsumerKey);
            ConsumerSecret = new OAuthParameter("oauth_consumer_secret", tokens.ConsumerSecret);
            Token = new OAuthParameter("oauth_token", tokens.AccessToken);
            TokenSecret = new OAuthParameter("oauth_token_secret", tokens.AccessTokenSecret);
        }

        /// <summary>
        /// Get encoded Uri.
        /// </summary>
        /// <param name="requestUri">Request uri.</param>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Encoded Uri.</returns>
        private static Uri GetEncodedUri(Uri requestUri, IEnumerable<OAuthParameter> parameters)
        {
            StringBuilder requestParametersBuilder = new StringBuilder(requestUri.AbsoluteWithoutQuery());
            var oAuthParameters = parameters as OAuthParameter[] ?? parameters.ToArray();
            if (oAuthParameters.Any())
            {
                requestParametersBuilder.Append("?");

                foreach (var queryParam in oAuthParameters)
                {
                    requestParametersBuilder.AppendFormat("{0}&", queryParam.ToString());
                }

                requestParametersBuilder.Remove(requestParametersBuilder.Length - 1, 1);
            }

            return new Uri(requestParametersBuilder.ToString());
        }

        /// <summary>
        /// Generate nonce.
        /// </summary>
        /// <returns>String nonce.</returns>
        private static string GenerateNonce()
        {
            return new Random()
                        .Next(123400, int.MaxValue)
                        .ToString("X", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generate timestamp string.
        /// </summary>
        /// <returns>Timestamp string.</returns>
        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Generate signature.
        /// </summary>
        /// <returns>Generated signature string.</returns>
        private string GenerateSignature()
        {
            string signatureBaseString = string.Format(
                CultureInfo.InvariantCulture,
                "{2}&{0}&{1}",
                OAuthEncoder.UrlEncode(RequestUriWithoutQuery.Normalize()),
                OAuthEncoder.UrlEncode(GetSignParameters()),
                Verb);

            string key = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}",
                OAuthEncoder.UrlEncode(ConsumerSecret.Value),
                OAuthEncoder.UrlEncode(TokenSecret.Value));

            return OAuthEncoder.GenerateHash(signatureBaseString, key);
        }

        /// <summary>
        /// Generate authorization header.
        /// </summary>
        /// <returns>Generated authorizatin header string.</returns>
        private string GenerateAuthorizationHeader()
        {
            StringBuilder authHeaderBuilder = new StringBuilder();

            authHeaderBuilder.AppendFormat("OAuth realm=\"{0}\",", Realm);
            authHeaderBuilder.Append(string.Join(",", GetAuthHeaderParameters().OrderBy(p => p.Key).Select(p => p.ToString(true)).ToArray()));
            authHeaderBuilder.AppendFormat(",{0}", Signature.ToString(true));

            return authHeaderBuilder.ToString();
        }

        /// <summary>
        /// Get list of sign parameters.
        /// </summary>
        /// <returns>List of sign parameters.</returns>
        private IEnumerable<OAuthParameter> GetSignParameters()
        {
            foreach (var queryParam in QueryParams)
            {
                yield return queryParam;
            }

            yield return Version;
            yield return Nonce;
            yield return Timestamp;
            yield return SignatureMethod;
            yield return ConsumerKey;
            yield return Token;
        }

        /// <summary>
        /// Get list of auth header parameters.
        /// </summary>
        /// <returns>List of auth header paramters.</returns>
        private IEnumerable<OAuthParameter> GetAuthHeaderParameters()
        {
            yield return Version;
            yield return Nonce;
            yield return Timestamp;
            yield return SignatureMethod;
            yield return ConsumerKey;
            yield return Token;
        }
    }
}