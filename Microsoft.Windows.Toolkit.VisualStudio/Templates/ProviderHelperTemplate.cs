using AppStudio.DataProviders.$ServiceInstance.Name$;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $ProjectDefaultNamespace$
{
    public class $ServiceInstance.Name$ProviderHelper
    {
        private $ServiceInstance.Name$ProviderHelper()
        {

        }

        private static $ServiceInstance.Name$ProviderHelper _instance;

        public static $ServiceInstance.Name$ProviderHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new $ServiceInstance.Name$ProviderHelper());
            }
        }

        public $ServiceInstance.Name$DataProvider GetProvider()
        {
    // TODO these values differ per provider - needs abstracting
             return new $ServiceInstance.Name$DataProvider(new $ServiceInstance.Name$OAuthTokens
            {
                AppId = "$ConsumerKey$",
                AppSecret = "$ConsumerSecret$"
                //,AccessToken = "$AccessToken$",
                //AccessTokenSecret = "$AccessTokenSecret$"
            }); ;
        }

        public async Task<List<$ServiceInstance.Name$Schema>> RequestAsync()
        {

            List<$ServiceInstance.Name$Schema> queryResults = new List<$ServiceInstance.Name$Schema> ();

            // TODO these values differ per provider - needs abstracting
            $ServiceInstance.Name$DataConfig config = new $ServiceInstance.Name$DataConfig
            {
                UserId = "8195378771"
            };

            var results = await GetProvider().LoadDataAsync(config);
            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }
    }
}