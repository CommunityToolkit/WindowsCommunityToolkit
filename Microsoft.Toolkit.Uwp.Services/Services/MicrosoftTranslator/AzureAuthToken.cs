// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftTranslator
{
    /// <summary>
    /// Client to call Cognitive Services Azure Auth Token service in order to get an access token.
    /// </summary>
    internal class AzureAuthToken
    {
        /// <summary>
        /// Name of header used to pass the subscription key to the token service
        /// </summary>
        private const string OcpApimSubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";

        /// <summary>
        /// URL of the token service
        /// </summary>
        private static readonly Uri ServiceUrl = new Uri("https://api.cognitive.microsoft.com/sts/v1.0/issueToken");

        /// <summary>
        /// After obtaining a valid token, this class will cache it for this duration.
        /// Use a duration of 8 minutes, which is less than the actual token lifetime of 10 minutes.
        /// </summary>
        private static readonly TimeSpan TokenCacheDuration = new TimeSpan(0, 8, 0);

        private static HttpClient client = new HttpClient();

        private string _storedTokenValue = string.Empty;
        private DateTime _storedTokenTime = DateTime.MinValue;
        private string _subscriptionKey;

        /// <summary>
        /// Gets or sets the Service Subscription Key.
        /// </summary>
        public string SubscriptionKey
        {
            get
            {
                return _subscriptionKey;
            }

            set
            {
                if (_subscriptionKey != value)
                {
                    // If the subscription key is changed, the token is no longer valid.
                    _subscriptionKey = value;
                    _storedTokenValue = string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAuthToken"/> class, that is used to obtain access token
        /// </summary>
        /// <param name="key">Subscription key to use to get an authentication token.</param>
        public AzureAuthToken(string key)
        {
            SubscriptionKey = key;
        }

        /// <summary>
        /// Gets a token for the specified subscription.
        /// </summary>
        /// <returns>The encoded JWT token prefixed with the string "Bearer ".</returns>
        /// <remarks>
        /// This method uses a cache to limit the number of request to the token service.
        /// A fresh token can be re-used during its lifetime of 10 minutes. After a successful
        /// request to the token service, this method caches the access token. Subsequent
        /// invocations of the method return the cached token for the next 8 minutes. After
        /// 8 minutes, a new token is fetched from the token service and the cache is updated.
        /// </remarks>
        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_subscriptionKey))
            {
                throw new ArgumentNullException(nameof(SubscriptionKey), "A subscription key is required. Go to Azure Portal and sign up for Microsoft Translator: https://portal.azure.com/#create/Microsoft.CognitiveServices/apitype/TextTranslation");
            }

            // Re-use the cached token if there is one.
            if ((DateTime.Now - _storedTokenTime) < TokenCacheDuration && !string.IsNullOrWhiteSpace(_storedTokenValue))
            {
                return _storedTokenValue;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl))
            {
                request.Headers.Add(OcpApimSubscriptionKeyHeader, SubscriptionKey);

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(content);
                    throw new TranslatorServiceException(error.Message);
                }

                _storedTokenTime = DateTime.Now;
                _storedTokenValue = $"Bearer {content}";

                return _storedTokenValue;
            }
        }
    }
}
