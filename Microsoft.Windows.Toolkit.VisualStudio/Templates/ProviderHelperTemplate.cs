using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppStudio.DataProviders. ???;

namespace $ProjectDefaultNamespace$
{
    public class $Name$ProviderHelper
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
            var tokens = new $ServiceInstance.Name$OAuthTokens();
            
            $TOKEN_PROPERTIES_AND_VALUES$

            return new $ServiceInstance.Name$DataProvider(tokens);
        }

        public $ServiceInstance.Name$DataConfig Config { get; set; }
        public async Task<List<$ServiceInstance.Name$Schema>> RequestAsync()
        {

            List<$ServiceInstance.Name$Schema> queryResults = new List<$ServiceInstance.Name$Schema> ();

            var results = await GetProvider().LoadDataAsync(Config);
            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }
    }
}
