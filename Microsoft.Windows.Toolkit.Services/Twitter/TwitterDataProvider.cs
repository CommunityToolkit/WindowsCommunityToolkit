using Microsoft.Windows.Toolkit.Services.Core;
using Microsoft.Windows.Toolkit.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    [ConnectedServiceProvider("Twitter", "https://dev.twitter.com/web/sign-in")]
    public class TwitterDataProvider : DataProviderBase<TwitterDataConfig, TwitterSchema>
    {
        private TwitterOAuthTokens _tokens;
        private const string BaseUrl = "https://api.twitter.com/1.1";

        public TwitterDataProvider(TwitterOAuthTokens tokens)
        {
            _tokens = tokens;
        }

        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TwitterDataConfig config, int maxRecords, IParser<TSchema> parser)
        {
            IEnumerable<TSchema> items;
            switch (config.QueryType)
            {
                case TwitterQueryType.User:
                    items = await GetUserTimeLineAsync(config.Query, maxRecords, parser);
                    break;
                case TwitterQueryType.Search:
                    items = await SearchAsync(config.Query, maxRecords, parser);
                    break;
                case TwitterQueryType.Home:
                default:
                    items = await GetHomeTimeLineAsync(maxRecords, parser);
                    break;
            }

            return items;
        }

        protected override IParser<TwitterSchema> GetDefaultParserInternal(TwitterDataConfig config)
        {
            switch (config.QueryType)
            {
                case TwitterQueryType.Search:
                    return new TwitterSearchParser();
                case TwitterQueryType.Home:
                case TwitterQueryType.User:
                default:
                    return new TwitterTimelineParser();
            }
        }

        public async Task<IEnumerable<TwitterSchema>> GetUserTimeLineAsync(string screenName, int maxRecords)
        {
            return await GetUserTimeLineAsync(screenName, maxRecords, new TwitterTimelineParser());
        }

        public async Task<IEnumerable<TSchema>> GetUserTimeLineAsync<TSchema>(string screenName, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            try
            {
                var uri = new Uri($"{BaseUrl}/statuses/user_timeline.json?screen_name={screenName}&count={maxRecords}&include_rts=1");

                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _tokens);

                var result = parser.Parse(rawResult);
                return result
                        .Take(maxRecords)
                        .ToList();
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new UserNotFoundException(screenName);
                    }
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }

        public async Task<IEnumerable<TwitterSchema>> SearchAsync(string hashTag, int maxRecords)
        {
            return await SearchAsync(hashTag, maxRecords, new TwitterSearchParser());
        }

        public async Task<IEnumerable<TSchema>> SearchAsync<TSchema>(string hashTag, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            try
            {
                var uri = new Uri($"{BaseUrl}/search/tweets.json?q={Uri.EscapeDataString(hashTag)}&count={maxRecords}");
                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _tokens);

                var result = parser.Parse(rawResult);
                return result
                        .Take(maxRecords)
                        .ToList();
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }

        protected override void ValidateConfig(TwitterDataConfig config)
        {
            if (config.Query == null && config.QueryType != TwitterQueryType.Home)
            {
                throw new ConfigParameterNullException("Query");
            }
            if (_tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }
            if (string.IsNullOrEmpty(_tokens.ConsumerKey))
            {
                throw new OAuthKeysNotPresentException("ConsumerKey");
            }
            if (string.IsNullOrEmpty(_tokens.ConsumerSecret))
            {
                throw new OAuthKeysNotPresentException("ConsumerSecret");
            }
            if (string.IsNullOrEmpty(_tokens.AccessToken))
            {
                throw new OAuthKeysNotPresentException("AccessToken");
            }
            if (string.IsNullOrEmpty(_tokens.AccessTokenSecret))
            {
                throw new OAuthKeysNotPresentException("AccessTokenSecret");
            }
        }

        private async Task<IEnumerable<TSchema>> GetHomeTimeLineAsync<TSchema>(int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            try
            {
                var uri = new Uri($"{BaseUrl}/statuses/home_timeline.json?count={maxRecords}");

                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _tokens);

                return parser.Parse(rawResult);
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }
    }

    internal class OAuthRequest
    {
        public async Task<string> ExecuteAsync(Uri requestUri, TwitterOAuthTokens tokens)
        {
            string result = string.Empty;
            var request = CreateRequest(requestUri, tokens);
            var response = await request.GetResponseAsync();
            var responseStream = GetResponseStream(response);

            using (StreamReader sr = new StreamReader(responseStream))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        private static Stream GetResponseStream(WebResponse response)
        {
            var encoding = response.Headers["content-encoding"];

            if (encoding != null && encoding == "gzip")
            {
                return new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                return response.GetResponseStream();
            }
        }

        private static WebRequest CreateRequest(Uri requestUri, TwitterOAuthTokens tokens)
        {
            var requestBuilder = new OAuthRequestBuilder(requestUri, tokens);

            var request = (HttpWebRequest)WebRequest.Create(requestBuilder.EncodedRequestUri);

            request.UseDefaultCredentials = true;
            request.Method = OAuthRequestBuilder.Verb;
            request.Headers["Authorization"] = requestBuilder.AuthorizationHeader;

            return request;
        }
    }

    internal class OAuthRequestBuilder
    {
        public const string Realm = "Twitter API";
        public const string Verb = "GET";

        public Uri EncodedRequestUri { get; private set; }
        public Uri RequestUriWithoutQuery { get; private set; }
        public IEnumerable<OAuthParameter> QueryParams { get; private set; }
        public OAuthParameter Version { get; private set; }
        public OAuthParameter Nonce { get; private set; }
        public OAuthParameter Timestamp { get; private set; }
        public OAuthParameter SignatureMethod { get; private set; }
        public OAuthParameter ConsumerKey { get; private set; }
        public OAuthParameter ConsumerSecret { get; private set; }
        public OAuthParameter Token { get; private set; }
        public OAuthParameter TokenSecret { get; private set; }
        public OAuthParameter Signature
        {
            get
            {
                return new OAuthParameter("oauth_signature", GenerateSignature());
            }
        }
        public string AuthorizationHeader
        {
            get
            {
                return GenerateAuthorizationHeader();
            }
        }

        public OAuthRequestBuilder(Uri requestUri, TwitterOAuthTokens tokens)
        {
            RequestUriWithoutQuery = new Uri(requestUri.AbsoluteWithoutQuery());

            QueryParams = requestUri.GetQueryParams()
                                        .Select(p => new OAuthParameter(p.Key, p.Value))
                                        .ToList();

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

        private static Uri GetEncodedUri(Uri requestUri, IEnumerable<OAuthParameter> parameters)
        {
            StringBuilder requestParametersBuilder = new StringBuilder(requestUri.AbsoluteWithoutQuery());
            if (parameters.Count() > 0)
            {
                requestParametersBuilder.Append("?");

                foreach (var queryParam in parameters)
                {
                    requestParametersBuilder.AppendFormat("{0}&", queryParam.ToString());
                }

                requestParametersBuilder.Remove(requestParametersBuilder.Length - 1, 1);
            }

            return new Uri(requestParametersBuilder.ToString());
        }

        private static string GenerateNonce()
        {
            return new Random()
                        .Next(123400, int.MaxValue)
                        .ToString("X", CultureInfo.InvariantCulture);
        }

        private static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
        }

        private string GenerateSignature()
        {
            string signatureBaseString = string.Format(
                CultureInfo.InvariantCulture,
                "GET&{0}&{1}",
                OAuthEncoder.UrlEncode(RequestUriWithoutQuery.Normalize()),
                OAuthEncoder.UrlEncode(GetSignParameters()));

            string key = string.Format(
                CultureInfo.InvariantCulture,
                "{0}&{1}",
                OAuthEncoder.UrlEncode(ConsumerSecret.Value),
                OAuthEncoder.UrlEncode(TokenSecret.Value));

            return OAuthEncoder.GenerateHash(signatureBaseString, key);
        }

        private string GenerateAuthorizationHeader()
        {
            StringBuilder authHeaderBuilder = new StringBuilder();

            authHeaderBuilder.AppendFormat("OAuth realm=\"{0}\",", Realm);
            authHeaderBuilder.Append(string.Join(",", GetAuthHeaderParameters().OrderBy(p => p.Key).Select(p => p.ToString(true)).ToArray()));
            authHeaderBuilder.AppendFormat(",{0}", Signature.ToString(true));

            return authHeaderBuilder.ToString();
        }

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

    internal static class OAuthUriExtensions
    {
        public static IDictionary<string, string> GetQueryParams(this Uri uri)
        {
            var result = new Dictionary<string, string>();

            foreach (Match item in Regex.Matches(uri.Query, @"(?<key>[^&?=]+)=(?<value>[^&?=]+)"))
            {
                result.Add(item.Groups["key"].Value, item.Groups["value"].Value);
            }

            return result;
        }

        public static string AbsoluteWithoutQuery(this Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri.AbsoluteUri;
            }
            return uri.AbsoluteUri.Replace(uri.Query, string.Empty);
        }

        public static string Normalize(this Uri uri)
        {
            var result = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", uri.Scheme, uri.Host));
            if (!((uri.Scheme == "http" && uri.Port == 80) || (uri.Scheme == "https" && uri.Port == 443)))
            {
                result.Append(string.Concat(":", uri.Port));
            }
            result.Append(uri.AbsolutePath);

            return result.ToString();
        }
    }

    internal class OAuthParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public OAuthParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool withQuotes)
        {
            string format = null;
            if (withQuotes)
            {
                format = "{0}=\"{1}\"";
            }
            else
            {
                format = "{0}={1}";
            }
            return string.Format(CultureInfo.InvariantCulture, format, OAuthEncoder.UrlEncode(Key), OAuthEncoder.UrlEncode(Value));
        }
    }

    internal static class OAuthEncoder
    {
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var result = Uri.EscapeDataString(value);

            // UrlEncode escapes with lowercase characters (e.g. %2f) but oAuth needs %2F
            result = Regex.Replace(result, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());

            // these characters are not escaped by UrlEncode() but needed to be escaped
            result = result
                        .Replace("(", "%28")
                        .Replace(")", "%29")
                        .Replace("$", "%24")
                        .Replace("!", "%21")
                        .Replace("*", "%2A")
                        .Replace("'", "%27");

            // these characters are escaped by UrlEncode() but will fail if unescaped!
            result = result.Replace("%7E", "~");

            return result;
        }

        public static string UrlEncode(IEnumerable<OAuthParameter> parameters)
        {
            string rawUrl = string.Join("&", parameters.OrderBy(p => p.Key).Select(p => p.ToString()).ToArray());
            return UrlEncode(rawUrl);
        }

        public static string GenerateHash(string input, string key)
        {
            //MacAlgorithmProvider mac = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            //IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            //CryptographicKey cryptoKey = mac.CreateKey(keyMaterial);
            //IBuffer hash = CryptographicEngine.Sign(cryptoKey, CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8));
            //return CryptographicBuffer.EncodeToBase64String(hash);
            return String.Empty;
        }
    }
}
