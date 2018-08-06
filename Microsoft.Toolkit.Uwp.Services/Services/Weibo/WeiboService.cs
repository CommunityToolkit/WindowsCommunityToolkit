using System;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.Services.Weibo
{
    /// <summary>
    /// Class for connecting to Weibo.
    /// </summary>
    public class WeiboService
    {
        /// <summary>
        /// Private field for WeiboDataProvider.
        /// </summary>
        private WeiboDataProvider weiboDataProvider;

        /// <summary>
        /// Field for tracking oAuthTokens.
        /// </summary>
        private WeiboOAuthTokens tokens;

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool isInitialized;

        public WeiboService()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static WeiboService instance;

        public static WeiboService Instance => instance ?? (instance = new WeiboService());

        public long? Uid => Provider.Uid;

        public bool Initialize(string appKey, string appSecret, string redirectUri)
        {
            if (string.IsNullOrEmpty(appKey))
            {
                throw new ArgumentNullException(nameof(appKey));
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentNullException(nameof(appSecret));
            }

            if (string.IsNullOrEmpty(redirectUri))
            {
                throw new ArgumentNullException(nameof(redirectUri));
            }

            WeiboOAuthTokens oAuthTokens = new WeiboOAuthTokens
            {
                AppKey = appKey,
                AppSecret = appSecret,
                RedirectUri = redirectUri
            };

            return Initialize(oAuthTokens);
        }

        public bool Initialize(WeiboOAuthTokens oAuthTokens)
        {
            if (oAuthTokens == null)
            {
                throw new ArgumentNullException(nameof(oAuthTokens));
            }

            tokens = oAuthTokens;
            isInitialized = true;

            weiboDataProvider = null;

            return true;
        }

        public WeiboDataProvider Provider
        {
            get
            {
                if (!isInitialized)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return weiboDataProvider ?? (weiboDataProvider = new WeiboDataProvider(tokens));
            }
        }

        public async Task<WeiboUser> GetUserAsync(string screenName = null)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.GetUserAsync(screenName);
            }

            bool isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetUserAsync(screenName);
            }

            return null;
        }

        public Task<bool> LoginAsync()
        {
            return Provider.LoginAsync();
        }

        public void Logout()
        {
            Provider.Logout();
        }
    }
}