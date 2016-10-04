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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.Exceptions;
using Newtonsoft.Json;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Data Provider for connecting to Twitter service.
    /// </summary>
    public class TwitterDataProvider : DataProviderBase<TwitterDataConfig, Tweet>
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string BaseUrl = "https://api.twitter.com/1.1";
        private const string OAuthBaseUrl = "https://api.twitter.com/oauth";
        private const string PublishUrl = "https://upload.twitter.com/1.1";

        /// <summary>
        /// Base Url for service.
        /// </summary>
        private readonly TwitterOAuthTokens tokens;

        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly PasswordVault vault;

        /// <summary>
        /// Gets or sets logged in user information.
        /// </summary>
        public string UserScreenName { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider is already logged in
        /// </summary>
        public bool LoggedIn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        public TwitterDataProvider(TwitterOAuthTokens tokens)
        {
            this.tokens = tokens;
            vault = new PasswordVault();
        }

        /// <summary>
        /// Retrieve user data.
        /// </summary>
        /// <param name="screenName">User screen name or null for current logged user</param>
        /// <returns>Returns user data.</returns>
        public async Task<TwitterUser> GetUserAsync(string screenName = null)
        {
            string rawResult = null;
            try
            {
                var userScreenName = screenName ?? UserScreenName;
                var uri = new Uri($"{BaseUrl}/users/show.json?screen_name={userScreenName}");

                TwitterOAuthRequest request = new TwitterOAuthRequest();
                rawResult = await request.ExecuteGetAsync(uri, tokens);
                return JsonConvert.DeserializeObject<TwitterUser>(rawResult);
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
            catch
            {
                if (!string.IsNullOrEmpty(rawResult))
                {
                    var errors = JsonConvert.DeserializeObject<TwitterErrors>(rawResult);

                    throw new TwitterException { Errors = errors };
                }

                throw;
            }
        }

        /// <summary>
        /// Retrieve user timeline data with specific parser.
        /// </summary>
        /// <typeparam name="TSchema">Strong type for results.</typeparam>
        /// <param name="screenName">User screen name.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="parser">Specific results parser.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<TSchema>> GetUserTimeLineAsync<TSchema>(string screenName, int maxRecords, IParser<TSchema> parser)
            where TSchema : SchemaBase
        {
            string rawResult = null;
            try
            {
                var uri = new Uri($"{BaseUrl}/statuses/user_timeline.json?screen_name={screenName}&count={maxRecords}&include_rts=1");

                TwitterOAuthRequest request = new TwitterOAuthRequest();
                rawResult = await request.ExecuteGetAsync(uri, tokens);

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
            catch
            {
                if (!string.IsNullOrEmpty(rawResult))
                {
                    var errors = JsonConvert.DeserializeObject<TwitterErrors>(rawResult);

                    throw new TwitterException { Errors = errors };
                }

                throw;
            }
        }

        /// <summary>
        /// Search for specific hash tag with specific parser.
        /// </summary>
        /// <typeparam name="TSchema">Strong type for results.</typeparam>
        /// <param name="hashTag">Hash tag.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="parser">Specific results parser.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<TSchema>> SearchAsync<TSchema>(string hashTag, int maxRecords, IParser<TSchema> parser)
            where TSchema : SchemaBase
        {
            try
            {
                var uri = new Uri($"{BaseUrl}/search/tweets.json?q={Uri.EscapeDataString(hashTag)}&count={maxRecords}");
                TwitterOAuthRequest request = new TwitterOAuthRequest();
                var rawResult = await request.ExecuteGetAsync(uri, tokens);

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

        private PasswordCredential PasswordCredential
        {
            get
            {
                // Password vault remains when app is uninstalled so checking the local settings value
                if (ApplicationData.Current.LocalSettings.Values["TwitterScreenName"] == null)
                {
                    return null;
                }

                var passwordCredentials = vault.RetrieveAll();
                var temp = passwordCredentials.FirstOrDefault(c => c.Resource == "TwitterAccessToken");

                if (temp == null)
                {
                    return null;
                }

                return vault.Retrieve(temp.Resource, temp.UserName);
            }
        }

        /// <summary>
        /// Log user in to Twitter.
        /// </summary>
        /// <returns>Boolean indicating login success.</returns>
        public async Task<bool> LoginAsync()
        {
            var twitterCredentials = PasswordCredential;
            if (twitterCredentials != null)
            {
                tokens.AccessToken = twitterCredentials.UserName;
                tokens.AccessTokenSecret = twitterCredentials.Password;
                UserScreenName = ApplicationData.Current.LocalSettings.Values["TwitterScreenName"].ToString();
                LoggedIn = true;
                return true;
            }

            if (await InitializeRequestAccessTokensAsync(tokens.CallbackUri) == false)
            {
                LoggedIn = false;
                return false;
            }

            string requestToken = tokens.RequestToken;
            string twitterUrl = $"{OAuthBaseUrl}/authorize?oauth_token={requestToken}";

            Uri startUri = new Uri(twitterUrl);
            Uri endUri = new Uri(tokens.CallbackUri);

            var result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);

            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    LoggedIn = true;
                    return await ExchangeRequestTokenForAccessTokenAsync(result.ResponseData);
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("WAB failed, message={0}", result.ResponseErrorDetail.ToString());
                    LoggedIn = false;
                    return false;
                case WebAuthenticationStatus.UserCancel:
                    Debug.WriteLine("WAB user aborted.");
                    LoggedIn = false;
                    return false;
            }

            LoggedIn = false;
            return false;
        }

        /// <summary>
        /// Log user out of Twitter.
        /// </summary>
        public void Logout()
        {
            var twitterCredentials = PasswordCredential;
            if (twitterCredentials != null)
            {
                vault.Remove(twitterCredentials);
                ApplicationData.Current.LocalSettings.Values["TwitterScreenName"] = null;
                UserScreenName = null;
            }

            LoggedIn = false;
        }

        /// <summary>
        /// Tweets a status update.
        /// </summary>
        /// <param name="tweet">Tweet text.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Success or failure.</returns>
        public async Task<bool> TweetStatusAsync(string tweet, params IRandomAccessStream[] pictures)
        {
            try
            {
                var media_ids = string.Empty;

                if (pictures != null && pictures.Length > 0)
                {
                    var ids = new List<string>();
                    foreach (var picture in pictures)
                    {
                        ids.Add(await UploadPictureAsync(picture));
                    }

                    media_ids = "&media_ids=" + string.Join(",", ids);
                }

                var uri = new Uri($"{BaseUrl}/statuses/update.json?status={Uri.EscapeDataString(tweet)}{media_ids}");

                TwitterOAuthRequest request = new TwitterOAuthRequest();
                await request.ExecutePostAsync(uri, tokens);

                return true;
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

        /// <summary>
        /// Publish a picture to Twitter user's medias.
        /// </summary>
        /// <param name="stream">Picture stream.</param>
        /// <returns>Media ID</returns>
        public async Task<string> UploadPictureAsync(IRandomAccessStream stream)
        {
            var uri = new Uri($"{PublishUrl}/media/upload.json");

            // Get picture data
            var fileBytes = new byte[stream.Size];

            using (DataReader reader = new DataReader(stream))
            {
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(fileBytes);
            }

            string boundary = DateTime.Now.Ticks.ToString("x");

            TwitterOAuthRequest request = new TwitterOAuthRequest();
            return await request.ExecutePostMultipartAsync(uri, tokens, boundary, fileBytes);
        }

        /// <summary>
        /// Returns parser implementation for specified configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        /// <returns>Strongly typed parser.</returns>
        protected override IParser<Tweet> GetDefaultParser(TwitterDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }

            switch (config.QueryType)
            {
                case TwitterQueryType.Search:
                    return new TwitterSearchParser();
                case TwitterQueryType.Home:
                case TwitterQueryType.User:
                default:
                    return new TweetParser();
            }
        }

        /// <summary>
        /// Wrapper around REST API for making data request.
        /// </summary>
        /// <typeparam name="TSchema">Schema to use</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper limit for records returned.</param>
        /// <param name="parser">IParser implementation for interpreting results.</param>
        /// <returns>Strongly typed list of results.</returns>
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

        /// <summary>
        /// Check validity of configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        protected override void ValidateConfig(TwitterDataConfig config)
        {
            if (config?.Query == null && config?.QueryType != TwitterQueryType.Home)
            {
                throw new ConfigParameterNullException("Query");
            }

            if (tokens == null)
            {
                throw new ConfigParameterNullException("Tokens");
            }

            if (string.IsNullOrEmpty(tokens.ConsumerKey))
            {
                throw new OAuthKeysNotPresentException("ConsumerKey");
            }

            if (string.IsNullOrEmpty(tokens.ConsumerSecret))
            {
                throw new OAuthKeysNotPresentException("ConsumerSecret");
            }
        }

        /// <summary>
        /// Extract requested token from the REST API response string.
        /// </summary>
        /// <param name="getResponse">REST API response string.</param>
        /// <param name="tokenType">Token type to retrieve.</param>
        /// <returns>Required token.</returns>
        private static string ExtractTokenFromResponse(string getResponse, TwitterOAuthTokenType tokenType)
        {
            string requestOrAccessToken = null;
            string requestOrAccessTokenSecret = null;
            string oauth_verifier = null;
            string oauth_callback_confirmed = null;
            string screen_name = null;
            string[] keyValPairs = getResponse.Split('&');

            for (int i = 0; i < keyValPairs.Length; i++)
            {
                string[] splits = keyValPairs[i].Split('=');
                switch (splits[0])
                {
                    case "screen_name":
                        screen_name = splits[1];
                        break;
                    case "oauth_token":
                        requestOrAccessToken = splits[1];
                        break;
                    case "oauth_token_secret":
                        requestOrAccessTokenSecret = splits[1];
                        break;
                    case "oauth_callback_confirmed":
                        oauth_callback_confirmed = splits[1];
                        break;
                    case "oauth_verifier":
                        oauth_verifier = splits[1];
                        break;
                }
            }

            switch (tokenType)
            {
                case TwitterOAuthTokenType.OAuthRequestOrAccessToken:
                    return requestOrAccessToken;
                case TwitterOAuthTokenType.OAuthRequestOrAccessTokenSecret:
                    return requestOrAccessTokenSecret;
                case TwitterOAuthTokenType.OAuthVerifier:
                    return oauth_verifier;
                case TwitterOAuthTokenType.ScreenName:
                    return screen_name;
                case TwitterOAuthTokenType.OAuthCallbackConfirmed:
                    return oauth_callback_confirmed;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get home time line data.
        /// </summary>
        /// <typeparam name="TSchema">Strong typed result.</typeparam>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="parser">Specific result parser.</param>
        /// <returns>Return strong typed list of results.</returns>
        private async Task<IEnumerable<TSchema>> GetHomeTimeLineAsync<TSchema>(int maxRecords, IParser<TSchema> parser)
            where TSchema : SchemaBase
        {
            try
            {
                var uri = new Uri($"{BaseUrl}/statuses/home_timeline.json?count={maxRecords}");

                TwitterOAuthRequest request = new TwitterOAuthRequest();
                var rawResult = await request.ExecuteGetAsync(uri, tokens);

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

        /// <summary>
        /// Package up token request.
        /// </summary>
        /// <param name="twitterCallbackUrl">Callback Uri.</param>
        /// <returns>Success or failure.</returns>
        private async Task<bool> InitializeRequestAccessTokensAsync(string twitterCallbackUrl)
        {
            var twitterUrl = $"{OAuthBaseUrl}/request_token";

            string nonce = GetNonce();
            string timeStamp = GetTimeStamp();
            string sigBaseStringParams = GetSignatureBaseStringParams(tokens.ConsumerKey, nonce, timeStamp, "oauth_callback=" + Uri.EscapeDataString(twitterCallbackUrl));
            string sigBaseString = "GET&" + Uri.EscapeDataString(twitterUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);
            string signature = GetSignature(sigBaseString, tokens.ConsumerSecret);

            twitterUrl += "?" + sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature);

            string getResponse;

            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            using (HttpClient httpClient = new HttpClient(handler))
            {
                try
                {
                    getResponse = await httpClient.GetStringAsync(new Uri(twitterUrl));
                }
                catch (HttpRequestException hre)
                {
                    Debug.WriteLine("HttpClient call failed trying to retrieve Twitter Request Tokens.  Message: {0}", hre.Message);
                    return false;
                }
            }

            var callbackConfirmed = ExtractTokenFromResponse(getResponse, TwitterOAuthTokenType.OAuthCallbackConfirmed);
            if (Convert.ToBoolean(callbackConfirmed) != true)
            {
                return false;
            }

            tokens.RequestToken = ExtractTokenFromResponse(getResponse, TwitterOAuthTokenType.OAuthRequestOrAccessToken);
            tokens.RequestTokenSecret = ExtractTokenFromResponse(getResponse, TwitterOAuthTokenType.OAuthRequestOrAccessTokenSecret);

            return true;
        }

        /// <summary>
        /// Build signature base string.
        /// </summary>
        /// <param name="consumerKey">Consumer Key.</param>
        /// <param name="nonce">Nonce.</param>
        /// <param name="timeStamp">Timestamp.</param>
        /// <param name="additionalParameters">Any additional parameter name/values that need appending to base string.</param>
        /// <returns>Signature base string.</returns>
        private string GetSignatureBaseStringParams(string consumerKey, string nonce, string timeStamp, string additionalParameters = "")
        {
            string sigBaseStringParams = additionalParameters;
            sigBaseStringParams += "&" + "oauth_consumer_key=" + consumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + nonce;
            sigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
            sigBaseStringParams += "&" + "oauth_timestamp=" + timeStamp;
            sigBaseStringParams += "&" + "oauth_version=1.0";

            return sigBaseStringParams;
        }

        /// <summary>
        /// Extract and initialize access tokens.
        /// </summary>
        /// <param name="webAuthResultResponseData">WAB data containing appropriate tokens.</param>
        /// <returns>Success or failure.</returns>
        private async Task<bool> ExchangeRequestTokenForAccessTokenAsync(string webAuthResultResponseData)
        {
            string responseData = webAuthResultResponseData.Substring(webAuthResultResponseData.IndexOf("oauth_token"));
            string requestToken = ExtractTokenFromResponse(responseData, TwitterOAuthTokenType.OAuthRequestOrAccessToken);

            // Ensure requestToken matches accessToken per Twitter documentation.
            if (requestToken != tokens.RequestToken)
            {
                return false;
            }

            string oAuthVerifier = ExtractTokenFromResponse(responseData, TwitterOAuthTokenType.OAuthVerifier);

            string twitterUrl = $"{OAuthBaseUrl}/access_token";

            string timeStamp = GetTimeStamp();
            string nonce = GetNonce();

            string sigBaseStringParams = GetSignatureBaseStringParams(tokens.ConsumerKey, nonce, timeStamp, "oauth_token=" + requestToken);

            string sigBaseString = "POST&";
            sigBaseString += Uri.EscapeDataString(twitterUrl) + "&" + Uri.EscapeDataString(sigBaseStringParams);

            string signature = GetSignature(sigBaseString, tokens.ConsumerSecret);

            StringContent httpContent = new StringContent("oauth_verifier=" + oAuthVerifier, System.Text.Encoding.UTF8);
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            string authorizationHeaderParams = "oauth_consumer_key=\"" + tokens.ConsumerKey + "\", oauth_nonce=\"" + nonce + "\", oauth_signature_method=\"HMAC-SHA1\", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\", oauth_timestamp=\"" + timeStamp + "\", oauth_token=\"" + Uri.EscapeDataString(requestToken) + "\", oauth_version=\"1.0\"";

            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            string response;
            using (HttpClient httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authorizationHeaderParams);
                var httpResponseMessage = await httpClient.PostAsync(new Uri(twitterUrl), httpContent);
                response = await httpResponseMessage.Content.ReadAsStringAsync();
            }

            var screenName = ExtractTokenFromResponse(response, TwitterOAuthTokenType.ScreenName);
            var accessToken = ExtractTokenFromResponse(response, TwitterOAuthTokenType.OAuthRequestOrAccessToken);
            var accessTokenSecret = ExtractTokenFromResponse(response, TwitterOAuthTokenType.OAuthRequestOrAccessTokenSecret);

            UserScreenName = screenName;
            tokens.AccessToken = accessToken;
            tokens.AccessTokenSecret = accessTokenSecret;

            var passwordCredential = new PasswordCredential("TwitterAccessToken", accessToken, accessTokenSecret);
            ApplicationData.Current.LocalSettings.Values["TwitterScreenName"] = screenName;
            vault.Add(passwordCredential);

            return true;
        }

        /// <summary>
        /// Generate nonce.
        /// </summary>
        /// <returns>Nonce.</returns>
        private string GetNonce()
        {
            Random rand = new Random();
            int nonce = rand.Next(1000000000);
            return nonce.ToString();
        }

        /// <summary>
        /// Generate timestamp.
        /// </summary>
        /// <returns>Timestamp.</returns>
        private string GetTimeStamp()
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return Math.Round(sinceEpoch.TotalSeconds).ToString();
        }

        /// <summary>
        /// Generate request signature.
        /// </summary>
        /// <param name="sigBaseString">Base string.</param>
        /// <param name="consumerSecretKey">Consumer secret key.</param>
        /// <returns>Signature.</returns>
        private string GetSignature(string sigBaseString, string consumerSecretKey)
        {
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(consumerSecretKey + "&", BinaryStringEncoding.Utf8);
            MacAlgorithmProvider hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey macKey = hmacSha1Provider.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(sigBaseString, BinaryStringEncoding.Utf8);
            IBuffer signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            string signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);

            return signature;
        }
    }
}
