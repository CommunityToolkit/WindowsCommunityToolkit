using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    /// <summary>
    /// Data Provider for connecting to Weibo service.
    /// </summary>
    public class WeiboDataProvider
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string BaseUrl = "https://api.weibo.com/2";

        private const string OAuthBaseUrl = "https://api.weibo.com/oauth2";

        private static HttpClient _client;

        /// <summary>
        /// Base Url for service.
        /// </summary>
        private readonly WeiboOAuthTokens _tokens;

        /// <summary>
        /// Password vault used to store access token
        /// </summary>
        private readonly PasswordVault _vault;

        public long? Uid { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider is already logged in
        /// </summary>
        public bool LoggedIn { get; private set; }

        public WeiboDataProvider(WeiboOAuthTokens tokens)
        {
            _tokens = tokens;
            _vault = new PasswordVault();

            if (_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                _client = new HttpClient(handler);
            }
        }

        public async Task<WeiboUser> GetUserAsync(string screenName = null)
        {
            string rawResult = null;
            try
            {
                string userScreenName = screenName ?? Uid?.ToString();
                Uri uri = new Uri($"{BaseUrl}/users/show.json?screen_name={userScreenName}");

                WeiboOAuthRequest request = new WeiboOAuthRequest();
                rawResult = await request.ExecuteGetAsync(uri, _tokens);
                return JsonConvert.DeserializeObject<WeiboUser>(rawResult);
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
                    WeiboError error = JsonConvert.DeserializeObject<WeiboError>(rawResult);

                    throw new WeiboException { Error = error };
                }

                throw;
            }
        }

        private PasswordCredential PasswordCredential
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values["WeiboUid"] == null)
                {
                    return null;
                }

                IReadOnlyList<PasswordCredential> passwordCredentials = _vault.RetrieveAll();
                PasswordCredential temp = passwordCredentials.FirstOrDefault(c => c.Resource == "Weibo");

                if (temp == null)
                {
                    return null;
                }

                return _vault.Retrieve(temp.Resource, temp.UserName);
            }
        }

        public async Task<bool> LoginAsync()
        {
            PasswordCredential weiboCredentials = PasswordCredential;
            if (weiboCredentials != null)
            {
                _tokens.AccessToken = weiboCredentials.Password;
                Uid = long.Parse(ApplicationData.Current.LocalSettings.Values["WeiboUid"].ToString());
                LoggedIn = true;
                return true;
            }

            string appKey = _tokens.AppKey;
            string redirectUri = _tokens.RedirectUri;
            string weiboUrl = $"{OAuthBaseUrl}/authorize?client_id={appKey}&redirect_uri={redirectUri}";

            Uri startUri = new Uri(weiboUrl);
            Uri endUri = new Uri(redirectUri);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);

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
        /// Log user out of Weibo.
        /// </summary>
        public void Logout()
        {
            PasswordCredential weiboCredentials = PasswordCredential;
            if (weiboCredentials != null)
            {
                _vault.Remove(weiboCredentials);
                ApplicationData.Current.LocalSettings.Values["WeiboUid"] = null;
                Uid = null;
            }

            LoggedIn = false;
        }

        private async Task<bool> ExchangeRequestTokenForAccessTokenAsync(string webAuthResultResponseData)
        {
            string code = new WwwFormUrlDecoder(new Uri(webAuthResultResponseData, UriKind.Absolute).Query).GetFirstValueByName("code");

            string weiboUrl = $"{OAuthBaseUrl}/access_token";

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData["client_id"] = _tokens.AppKey;
            postData["client_secret"] = _tokens.AppSecret;
            postData["grant_type"] = "authorization_code";
            postData["code"] = code;
            postData["redirect_uri"] = _tokens.RedirectUri;

            FormUrlEncodedContent postContent = new FormUrlEncodedContent(postData);

            string data = null;

            using (HttpResponseMessage response = await _client.PostAsync(weiboUrl, postContent).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            JObject jObject = JObject.Parse(data);

            string accessToken = jObject["access_token"].ToObject<string>();
            long uid = jObject["uid"].ToObject<long>();

            Uid = uid;
            _tokens.AccessToken = accessToken;

            PasswordCredential passwordCredential = new PasswordCredential("Weibo", "AccessToken", accessToken);
            ApplicationData.Current.LocalSettings.Values["WeiboUid"] = uid;
            _vault.Add(passwordCredential);

            return true;
        }
    }
}